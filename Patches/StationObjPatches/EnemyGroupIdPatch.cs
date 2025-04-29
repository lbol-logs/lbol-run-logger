using HarmonyLib;
using LBoL.Core.Stats;
using LBoL.Core.Units;
using LBoL.Core;
using LBoL.EntityLib.Adventures.FirstPlace;
using LBoL.EntityLib.Adventures.Stage2;
using RunLogger.Utils;

namespace RunLogger.Patches.StationObjPatches
{
    [HarmonyPatch]
    public static class EnemyGroupIdPatch
    {
        [HarmonyPatch(typeof(GameRunController), nameof(GameRunController.LeaveBattle)), HarmonyPostfix]
        private static void AddEnemyGroupId(EnemyGroup enemyGroup, GameRunController __instance)
        {
            GameRunController gameRun = __instance;
            string adventureId = Helpers.GetAdventureId(gameRun.CurrentStation);
            if (adventureId != null)
            {
                Helpers.AddDataValue("Id", enemyGroup.Id);
                switch (adventureId)
                {
                    case nameof(YachieOppression):
                        //AdventurePatch.YachieOppressionPatch.HandleBattle();
                        break;
                    case nameof(MiyoiBartender):
                        //AdventurePatch.MiyoiBartenderPatch.HandleBattle();
                        break;
                }
            }
            //RunDataController.Save();
        }
    }
}