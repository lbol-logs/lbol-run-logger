using HarmonyLib;
using LBoL.Core;
using LBoL.Core.Stations;
using LBoL.EntityLib.Adventures;
using LBoL.EntityLib.Exhibits.Shining;
using LBoL.EntityLib.Stages;
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
        [HarmonyPatch(typeof(BattleAdvTest))]
        class BattleAdvTestPatch
        {
            [HarmonyPatch(nameof(BattleAdvTest.CreateMap)), HarmonyPrefix]
            static void CreateMapPatch(BattleAdvTest __instance)
            {
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
                //__instance.TradeAdventureType = typeof(RinnosukeTrade);
                List<StationType> stationTypes = new List<StationType>();
                stationTypes.Add(StationType.Boss);
                for (int i = 0; i < 10; i++)
                {
                    //stationTypes.Add(StationType.Trade);
                    //stationTypes.Add(StationType.Entry);

                    stationTypes.Add(StationType.Enemy);
                    stationTypes.Add(StationType.Enemy);
                    stationTypes.Add(StationType.Entry);
                }
                __result = GameMap.CreateSingleRoute(__instance.Boss.Id, stationTypes.ToArray());
                return false;
            }

            [HarmonyPatch(typeof(GameRunController), nameof(GameRunController.RollShiningExhibit)), HarmonyPrefix]
            static bool RollShiningExhibitPatch(ref Exhibit __result)
            {
                __result = Library.CreateExhibit(typeof(Gongjuxiang));
                return false;
            }
        }
    }
}
