using HarmonyLib;
using LBoL.Core;
using LBoL.Core.SaveData;
using LBoL.EntityLib.Exhibits.Common;
using RunLogger.Utils;
using RunLogger.Utils.Managers;
using RunLogger.Utils.RunLogLib.Entities;
using System.Collections.Generic;
using System.Linq;

namespace RunLogger.Patches.RunLogPatches
{
    [HarmonyPatch]
    internal static class ExhibitChangePatch
    {
        [HarmonyPatch(typeof(GameRunController), nameof(GameRunController.GainExhibitRunner)), HarmonyPostfix, HarmonyPriority(Priority.Normal)]
        private static void Add(Exhibit exhibit)
        {
            if (!Instance.IsInitialized) return;
            EntitiesManager.AddExhibitChange(exhibit, ChangeType.Add);
        }

        [HarmonyPatch(typeof(GameRunController), nameof(GameRunController.LoseExhibit)), HarmonyPostfix]
        private static void Remove(Exhibit exhibit)
        {
            if (!Instance.IsInitialized) return;
            EntitiesManager.AddExhibitChange(exhibit, ChangeType.Remove);
        }

        [HarmonyPatch(typeof(Exhibit), nameof(Exhibit.Counter), MethodType.Setter)]
        private static class UseAndUpgrade
        {
            private static readonly string[] Exhibits = { nameof(GanzhuYao), nameof(ChuRenou), nameof(TiangouYuyi), nameof(Moping), nameof(Baota) };

            private static void Prefix(int value, Exhibit __instance)
            {
                if (!Instance.IsInitialized) return;

                Exhibit exhibit = __instance;
                if (!ExhibitChangePatch.UseAndUpgrade.Exhibits.Contains(exhibit.Id)) return;
                if (exhibit.GameRun == null) return;

                int change = value - exhibit.Counter;
                if (change < 0)
                {
                    if (exhibit is TiangouYuyi)
                    {
                        bool isTiangouYuyiFirstUsed = Controller.CurrentStation.Type == exhibit.GameRun.StageRecords.LastOrDefault().Stations.LastOrDefault().Type.ToString();
                        if (!isTiangouYuyiFirstUsed) return;
                    }
                    EntitiesManager.AddExhibitChange(exhibit, ChangeType.Use, value);
                }
                else if (change > 0)
                {
                    int offset = 0;
                    if (exhibit is Moping)
                    {
                        bool isMopingFirstUpgraded = Controller.CurrentStation.Type != exhibit.GameRun.CurrentStation.Type.ToString();
                        if (!isMopingFirstUpgraded) return;
                        offset = 1;
                    }
                    EntitiesManager.AddExhibitChange(exhibit, ChangeType.Upgrade, value, offset);
                }
            }

            private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                return instructions;
            }
        }
    }
}