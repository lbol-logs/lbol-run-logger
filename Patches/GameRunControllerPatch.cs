using HarmonyLib;
using LBoL.Core;
using LBoL.Core.Cards;
using LBoL.Core.Stations;
using LBoL.Core.Stats;
using LBoL.Core.Units;
using RunLogger.Utils;
using System.Collections.Generic;
using System.Linq;

namespace RunLogger.Patches
{
    [HarmonyDebug]
    [HarmonyPatch(typeof(GameRunController))]
    static class GameRunControllerPatch
    {
        private static bool isAfterBossReward = false;

        [HarmonyPatch(nameof(GameRunController.Create)), HarmonyPostfix]
        static void CreatePatch(GameRunStartupParameters parameters, GameRunController __result)
        {
            string Character = parameters.Player.Id;
            string PlayerType = parameters.PlayerType.ToString().Replace("Type", "");
            bool HasClearBonus = __result.HasClearBonus;
            bool ShowRandomResult = parameters.ShowRandomResult;
            bool IsAutoSeed = __result.IsAutoSeed;
            IEnumerable<PuzzleFlag> AllPuzzleFlags = PuzzleFlags.EnumerateComponents(parameters.Puzzles);
            List<string> Requests = AllPuzzleFlags.Select(puzzleFlag => puzzleFlag.ToString()).ToList();
            string Difficulty = parameters.Difficulty.ToString();
            RunDataController.Create();
            RunDataController.RunData.Version = VersionInfo.Current.Version;
            Settings Settings = new Settings()
            {
                Character = Character,
                PlayerType = PlayerType,
                HasClearBonus = HasClearBonus,
                ShowRandomResult = ShowRandomResult,
                IsAutoSeed = IsAutoSeed,
                Difficulty = Difficulty,
                Requests = Requests
            };
            RunDataController.RunData.Settings = Settings;
            RunDataController.Save();
        }

        [HarmonyDebug]
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

            if (s == null && isAfterBossReward)
            {
                Hp = station.Status.Hp;
                isAfterBossReward = false;
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

            StagePatch.waitForSave = false;

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
        }

        [HarmonyPatch(nameof(GameRunController.InternalAddDeckCards)), HarmonyPostfix]
        static void InternalAddDeckCardsPatch(Card[] cards)
        {
            RunDataController.AddCardChange(cards, ChangeType.Add);
        }

        [HarmonyPatch(nameof(GameRunController.RemoveDeckCards)), HarmonyPostfix]
        static void RemoveDeckCardsPatch(IEnumerable<Card> cards)
        {
            RunDataController.AddCardChange(cards.ToArray<Card>(), ChangeType.Remove);
        }

        [HarmonyPatch(nameof(GameRunController.UpgradeDeckCards)), HarmonyPostfix]
        static void UpgradeDeckCardsPatch(IEnumerable<Card> cards)
        {
            RunDataController.AddCardChange(cards.ToArray<Card>(), ChangeType.Upgrade);
        }

        [HarmonyPatch(nameof(GameRunController.GainExhibitRunner)), HarmonyPostfix]
        static void GainExhibitRunnerPatch(Exhibit exhibit)
        {
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
            if (RunDataController.GetAdventureId(__instance.CurrentStation) != null)
            {
                RunDataController.AddData("Id", enemyGroup.Id);
            }
            RunDataController.Save();
        }

        [HarmonyPatch(nameof(GameRunController.RollNormalExhibit)), HarmonyPostfix]
        static void RollNormalExhibitPatch(Exhibit __result)
        {
            if (RunDataController.Listener != StationPatch.Listener) return;
            string exhibit = __result.Id;
            RunDataController.Exhibits.Add(exhibit);
            RunDataController.Listener = null;
        }
    }
}
