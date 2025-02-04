﻿using BepInEx;
using HarmonyLib;
using LBoL.Core;
using LBoL.Core.Cards;
using LBoL.Core.Stations;
using LBoL.Core.Stats;
using LBoL.Core.Units;
using LBoL.EntityLib.Adventures.FirstPlace;
using LBoL.EntityLib.Adventures.Stage2;
using RunLogger.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RunLogger.Patches
{
    [HarmonyPatch(typeof(GameRunController))]
    static class GameRunControllerPatch
    {
        public static bool isAfterBossReward;

        public static bool isOverridingStartingDeck;
        public static IEnumerable<Card> startingDeckOverride;
        public static IEnumerable<Card> startingCards;
        public static List<Exhibit> startingExhibits = new List<Exhibit>();

        private static void ResetStartingDeckOverride()
        {
            isOverridingStartingDeck = false;
            startingDeckOverride = null;
        }

        [HarmonyPatch(nameof(GameRunController.Create)), HarmonyPrefix]
        static void PreCreatePatch()
        {
            ResetStartingDeckOverride();
        }

        [HarmonyPatch(nameof(GameRunController.Create)), HarmonyPostfix]
        static void CreatePatch(GameRunStartupParameters parameters, GameRunController __result)
        {
            RunDataController.Reset();

            string Character = parameters.Player.Id;
            string PlayerType = parameters.PlayerType.ToString().Replace("Type", "");
            bool HasClearBonus = __result.HasClearBonus;
            bool ShowRandomResult = parameters.ShowRandomResult;
            bool IsAutoSeed = __result.IsAutoSeed;
            string Difficulty = parameters.Difficulty.ToString();
            IEnumerable<PuzzleFlag> AllPuzzleFlags = PuzzleFlags.EnumerateComponents(parameters.Puzzles);
            List<string> Requests = AllPuzzleFlags.Select(puzzleFlag => puzzleFlag.ToString()).ToList();
            List<string> JadeBoxes = parameters.JadeBoxes.Select(JadeBox => JadeBox.Id).ToList();

            Dictionary<string, PluginInfo> PluginInfos = BepInEx.Bootstrap.Chainloader.PluginInfos;
            List<Mod> Mods = new List<Mod>();
            string[] priorities = new string[] { PInfo.GUID, "neo.lbol.fix.rngFix" };
            string[] excludes = new string[] { "com.bepis.bepinex.scriptengine", "neo.lbol.tools.watermark" };

            foreach (string GUID in priorities)
            {
                if (PluginInfos.TryGetValue(GUID, out PluginInfo PluginInfo)) Mods.Add(HandleMod(PluginInfo));
            }
            foreach ((string GUID, PluginInfo PluginInfo) in PluginInfos)
            {
                if (priorities.Contains(GUID) || excludes.Contains(GUID)) continue;
                if (GUID == "neo.lbol.qol.helpMeEirin") HasClearBonus = true;
                Mod Mod = HandleMod(PluginInfo);
                if (GUID == "rmrfmaxx.lbol.PatchouliCharacterMod") Mod.Configs = PatchouliModWrapper.GetConfigs();
                Mods.Add(Mod);
            }

            RunDataController.Create();
            RunDataController.RunData.Version = VersionInfo.Current.Version;
            if (BepinexPlugin.saveProfileName.Value) RunDataController.RunData.Name = parameters.UserProfile.Name;
            Settings Settings = new Settings()
            {
                Character = Character,
                PlayerType = PlayerType,
                HasClearBonus = HasClearBonus,
                ShowRandomResult = ShowRandomResult,
                IsAutoSeed = IsAutoSeed,
                Difficulty = Difficulty,
                Requests = Requests,
                Mods = Mods
            };
            if (JadeBoxes.Count > 0) Settings.JadeBoxes = JadeBoxes;
            RunDataController.RunData.Settings = Settings;

            RunDataController.Save();
        }

        private static Mod HandleMod(PluginInfo PluginInfo)
        {
            string GUID = PluginInfo.Metadata.GUID;
            string Name = PluginInfo.Metadata.Name;
            string Version = PluginInfo.Metadata.Version.ToString();
            Mod Mod = new Mod()
            {
                GUID = GUID,
                Name = Name,
                Version = Version
            };
            return Mod;
        }

        [HarmonyPatch(nameof(GameRunController.Save)), HarmonyPostfix]
        static void SavePatch(GameRunController __instance)
        {
            StationObj station = RunDataController.CurrentStation;
            Station s = __instance.CurrentStation;
            int Hp = __instance.Player.Hp;

            if (s != null)
            {
                if (s.IsStageEnd) isAfterBossReward = true;
            }
            else
            {
                if (isAfterBossReward)
                {
                    Hp = station.Status.Hp;
                    isAfterBossReward = false;
                }
            }

            Status Status = new Status
            {
                Money = __instance.Money,
                Hp = Hp,
                MaxHp = __instance.Player.MaxHp,
                Power = __instance.Player.Power,
                MaxPower = __instance.Player.MaxPower
            };

            if (station == null) RunDataController.RunData.Settings.Status = Status;
            else station.Status = Status;

            RunDataController.Save();
        }

        [HarmonyPatch(nameof(GameRunController.Restore)), HarmonyPostfix]
        static void RestorePatch()
        {
            RunDataController.Restore();
        }

        [HarmonyPatch(nameof(GameRunController.EnterStage)), HarmonyPostfix]
        static void EnterStagePatch(GameRunController __instance)
        {
            if (RunDataController.CurrentStation != null) isAfterBossReward = true;
            int Act = __instance.CurrentStage.Level;
            GameMap gameMap = __instance.CurrentMap;
            string bossId = gameMap.BossId;
            List<Node> Nodes = new List<Node>();
            for (int x = 0; x < gameMap.Nodes.GetLength(0); x++)
            {
                for (int y = 0; y < gameMap.Nodes.GetLength(1); y++)
                {
                    MapNode mapNode = gameMap.Nodes[x, y];
                    if (mapNode == null) continue;
                    Node Node = new Node
                    {
                        X = mapNode.X,
                        Y = mapNode.Y,
                        Followers = mapNode.FollowerList,
                        Type = mapNode.StationType.ToString()
                    };
                    Nodes.Add(Node);
                }
            }
            ActObj ActObj = new ActObj
            {
                Act = Act,
                Nodes = Nodes,
                Boss = bossId
            };
            RunDataController.RunData.Acts.Add(ActObj);
            RunDataController.Save();
        }

        [HarmonyPatch(nameof(GameRunController.EnterMapNode)), HarmonyPostfix, HarmonyPriority(2)]
        static void EnterMapNodePatch(MapNode node, GameRunController __instance)
        {
            InteractionViewerPatch.Listener = null;
            StationPatch.RewardListener = null;
            StationPatch.AddRewardsPatch.isAfterAddRewards = false;
            SeijaPatch.BattleActionPatch.isPlayerTrunStarted = false;
            AdventurePatch.NarumiOfferCardPatch.isNarumi = false;

            int Act = __instance.CurrentStage.Level;
            Station currentStation = __instance.CurrentStation;
            int Level = currentStation.Level;
            int Y = node.Y;
            string Type = node.StationType.ToString();
            StationObj station = new StationObj
            {
                Type = Type,
                Node = new NodeObj
                {
                    Act = Act,
                    Level = Level,
                    Y = Y
                }
            };
            string Id = RunDataController.GetAdventureId(currentStation);
            if (Id != null) station.Id = Id;
            else
            {
                Id = RunDataController.GetEnemyGroupId(currentStation);
                if (Id != null) station.Id = Id;
            }
            RunDataController.RunData.Stations.Add(station);

            if (isOverridingStartingDeck)
            {
                RunDataController.AddCardChange(startingDeckOverride, ChangeType.Add);
                ResetStartingDeckOverride();
            }

            if (startingCards != null)
            {
                RunDataController.AddCardChange(startingCards, ChangeType.Add);
                startingCards = null;
            }

            if (startingExhibits.Any())
            {
                foreach (Exhibit exhibit in startingExhibits)
                {
                    RunDataController.AddExhibitChange(exhibit, ChangeType.Add);
                }
                startingExhibits.Clear();
            }
        }

        [HarmonyPatch(nameof(GameRunController.InternalAddDeckCards)), HarmonyPostfix]
        static void InternalAddDeckCardsPatch(Card[] cards)
        {
            if (isOverridingStartingDeck) return;
            if (RunDataController.RunData == null)
            {
                startingCards = cards;
                return;
            }
            RunDataController.AddCardChange(cards, ChangeType.Add);
        }

        [HarmonyPatch(nameof(GameRunController.RemoveDeckCards)), HarmonyPrefix]
        static void RemoveDeckCardsPatch(IEnumerable<Card> cards)
        {
            if (isOverridingStartingDeck) return;
            RunDataController.AddCardChange(cards, ChangeType.Remove);
        }

        [HarmonyPatch(nameof(GameRunController.UpgradeDeckCards)), HarmonyPostfix]
        static void UpgradeDeckCardsPatch(IEnumerable<Card> cards)
        {
            RunDataController.AddCardChange(cards, ChangeType.Upgrade);
        }

        [HarmonyPatch(nameof(GameRunController.GainExhibitRunner)), HarmonyPostfix, HarmonyPriority(2)]
        static void GainExhibitRunnerPatch(Exhibit exhibit)
        {
            if (RunDataController.RunData == null)
            {
                startingExhibits.Add(exhibit);
                return;
            }
            RunDataController.AddExhibitChange(exhibit, ChangeType.Add);
        }

        [HarmonyPatch(nameof(GameRunController.LoseExhibit)), HarmonyPostfix]
        static void LoseExhibitPatch(Exhibit exhibit)
        {
            RunDataController.AddExhibitChange(exhibit, ChangeType.Remove);
        }

        [HarmonyPatch(nameof(GameRunController.LeaveBattle)), HarmonyPostfix]
        static void LeaveBattlePatch(EnemyGroup enemyGroup, BattleStats __result, GameRunController __instance)
        {
            int Rounds = __result.TotalRounds;
            RunDataController.AddData("Rounds", Rounds);
            string id = RunDataController.GetAdventureId(__instance.CurrentStation);
            if (id != null)
            {
                RunDataController.AddData("Id", enemyGroup.Id);
                switch (id)
                {
                    case nameof(YachieOppression):
                        AdventurePatch.YachieOppressionPatch.HandleBattle();
                        break;
                    case nameof(MiyoiBartender):
                        AdventurePatch.MiyoiBartenderPatch.HandleBattle();
                        break;
                }
            }
            RunDataController.Save();
        }

        [HarmonyPatch(nameof(GameRunController.RollNormalExhibit)), HarmonyPostfix]
        static void RollNormalExhibitPatch(Exhibit __result)
        {
            string RewardListener = StationPatch.RewardListener;
            BepinexPlugin.log.LogDebug($"RewardListener in {System.Reflection.MethodBase.GetCurrentMethod().Name}: {RewardListener}");
            if (RewardListener != StationPatch.Listener) return;
            string exhibit = __result.Id;
            RunDataController.Exhibits.Add(exhibit);
            StationPatch.RewardListener = null;
        }
    }
}
