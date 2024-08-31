using HarmonyLib;
using LBoL.Core;
using LBoL.Core.Randoms;
using LBoL.Core.Stations;
using LBoL.EntityLib.Adventures.FirstPlace;
using LBoL.EntityLib.Exhibits.Shining;
using LBoL.EntityLib.Stages;
using LBoL.EntityLib.Stages.NormalStages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RunLogger.Utils
{
    public static class Debugger
    {
        private const string _dir = "runLogger";
        private static bool _initialized;
        private static StreamWriter _streamWriter;
        public static bool isDebug = true;

        public static void Initialize()
        {
            if (_initialized)
            {
                return;
            }
            Reload();
            _initialized = true;
        }

        public static void Reload()
        {
            Directory.CreateDirectory(_dir);
            FileStream fileStream = File.Open($"{_dir}/debug.txt", FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
            StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.UTF8);
            _streamWriter = streamWriter;
        }

        public static void Write(string line)
        {
            Initialize();
            _streamWriter.WriteLine(line);
            _streamWriter.Flush();
        }

        [HarmonyDebug]
        [HarmonyPatch(typeof(Stage))]
        class StagePatch
        {
            //private static Type adv = typeof(YachieOppression);
            private static Type adv = typeof(MiyoiBartender);

            [HarmonyPatch(nameof(Stage.CreateStation)), HarmonyPrefix]
            static bool CreateStationPatch(MapNode node, ref Station __result, Stage __instance)
            {
                if (!isDebug) return true;
                if (!(__instance is WindGodLake)) return true;

                UniqueRandomPool<Type> pool = new UniqueRandomPool<Type>(true);
                for (int i = 0; i < 10; i++) pool.Add(adv);
                __instance.AdventurePool = pool;
                __result = __instance.CreateStationFromType(node, StationType.Adventure);
                return false;
            }

            [HarmonyPatch(nameof(Stage.GetAdventure)), HarmonyPrefix]
            static bool GetAdventurePatch(ref Type __result)
            {
                if (!isDebug) return true;

                __result = adv;
                return false;
            }
        }

        [HarmonyDebug]
        [HarmonyPatch(typeof(BattleAdvTest))]
        class BattleAdvTestPatch
        {
            [HarmonyPatch(nameof(BattleAdvTest.CreateMap)), HarmonyPrefix]
            static void CreateMapPatch(BattleAdvTest __instance)
            {
                if (!isDebug) return;
                __instance.Level = 1;
            }
        }

        [HarmonyDebug]
        [HarmonyPatch(typeof(AllStations))]
        class AllStationsPatch
        {
            [HarmonyPatch(nameof(AllStations.CreateMap)), HarmonyPrefix]
            static bool CreateMapPatch(AllStations __instance, ref GameMap __result)
            {
                if (!isDebug) return true;

                __instance.Level = 3;
                __instance.EnemyPoolAct3 = new UniqueRandomPool<string>(true) 
                {
                    { "37", 1f },
                    { "38", 1f },
                    { "39", 1f }
                };

                //__instance.TradeAdventureType = typeof(RinnosukeTrade);
                List<StationType> stationTypes = new List<StationType>();
                //stationTypes.Add(StationType.Boss);
                for (int i = 0; i < 10; i++)
                {
                    //stationTypes.Add(StationType.Trade);
                    //stationTypes.Add(StationType.Entry);

                    //stationTypes.Add(StationType.Enemy);
                    stationTypes.Add(StationType.BattleAdvTest);
                    stationTypes.Add(StationType.BattleAdvTest);
                    stationTypes.Add(StationType.BattleAdvTest);
                }
                __result = GameMap.CreateSingleRoute(__instance.Boss.Id, stationTypes.ToArray());
                return false;
            }

            [HarmonyPatch(typeof(GameRunController), nameof(GameRunController.RollShiningExhibit)), HarmonyPrefix]
            static bool RollShiningExhibitPatch(ref Exhibit __result)
            {
                if (!isDebug) return true;

                __result = Library.CreateExhibit(typeof(Gongjuxiang));
                return false;
            }
        }
    }
}
