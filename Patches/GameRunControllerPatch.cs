using HarmonyLib;
using LBoL.Core;
using LBoL.Core.Cards;
using LBoL.Core.Stations;
using LBoL.Core.Stats;
using RunLogger.Utils;
using System.Collections.Generic;
using System.Linq;

namespace RunLogger.Patches
{
    [HarmonyDebug]
    [HarmonyPatch(typeof(GameRunController))]
    class GameRunControllerPatch
    {
        [HarmonyPatch(nameof(GameRunController.Create)), HarmonyPostfix]
        static void CreatePatch(GameRunStartupParameters parameters)
        {
            string Character = parameters.Player.Id;
            string PlayerType = parameters.PlayerType.ToString().Replace("Type", "");
            bool HasClearBonus = parameters.UserProfile.HasClearBonus;
            bool ShowRandomResult = parameters.ShowRandomResult;
            bool IsAutoSeed = parameters.Seed == null;
            IEnumerable<PuzzleFlag> AllPuzzleFlags = PuzzleFlags.EnumerateComponents(parameters.Puzzles);
            List<string> Requests = AllPuzzleFlags.Select(puzzleFlag => puzzleFlag.ToString()).ToList();
            string Difficulty = parameters.Difficulty.ToString();
            RunDataController.Create();
            RunDataController.RunData.Version = VersionInfo.Current.Version;
            Settings Settings = new Settings()
            {
                Character = Character,
                PlayerType = PlayerType,
                ShowRandomResult = ShowRandomResult,
                IsAutoSeed = IsAutoSeed,
                Difficulty = Difficulty,
                Requests = Requests
            };
            RunDataController.RunData.Settings = Settings;
            RunDataController.Save();
        }

        [HarmonyPatch(nameof(GameRunController.Save)), HarmonyPostfix]
        static void SavePatch(GameRunController __instance)
        {
            StationObj StationObj = RunDataController.CurrentStation;
            if (StationObj != null && StationObj.Status != null) return;
            Status Status = new Status
            {
                Money = __instance.Money,
                Hp = __instance.Player.Hp,
                MaxHp = __instance.Player.MaxHp,
                Power = __instance.Player.Power,
                MaxPower = __instance.Player.MaxPower
            };
            if (StationObj == null)
            {
                RunDataController.RunData.Settings.Status = Status;
            }
            else {

                StationObj.Status = Status;
            }
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

        [HarmonyPatch(nameof(GameRunController.EnterMapNode)), HarmonyPostfix]
        static void EnterMapNodePatch(MapNode node, GameRunController __instance)
        {
            int Act = __instance.CurrentStage.Level;
            Station CurrentStation = __instance.CurrentStation;
            int Level = CurrentStation.Level;
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
            if (CurrentStation is AdventureStation AdventureStation)
            {
                string Id = AdventureStation.Adventure.Id;
                station.Id = Id;
            }
            else if (CurrentStation is BattleStation BattleStation)
            {
                string Id = BattleStation.EnemyGroup.Id;
                station.Id = Id;
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
        static void LeaveBattlePatch(BattleStats __result)
        {
            int Rounds = __result.TotalRounds;
            RunDataController.AddData("Rounds", Rounds);
            RunDataController.Save();
        }
    }
}
