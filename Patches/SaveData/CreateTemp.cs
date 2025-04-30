using BepInEx;
using HarmonyLib;
using LBoL.Core;
using RunLogger.Utils;
using RunLogger.Utils.RunLogLib;
using RunLogger.Wrappers;
using System.Collections.Generic;
using System.Linq;

namespace RunLogger.Patches.SaveData
{
    internal static class CreateTemp
    {
        [HarmonyPatch(typeof(GameRunController), nameof(GameRunController.Create))]
        private static class StartRun
        {
            private static void Prefix()
            {
                BepinexPlugin.log.LogDebug("Start run");
                Controller.CreateInstance(new RunLog());
            }

            private static void Postfix(GameRunStartupParameters parameters, GameRunController __result)
            {
                string character = parameters.Player.Id;
                string playerType = parameters.PlayerType.ToString().Replace("Type", "");
                bool hasClearBonus = __result.HasClearBonus;
                bool showRandomResult = parameters.ShowRandomResult;
                bool isAutoSeed = __result.IsAutoSeed;
                string difficulty = parameters.Difficulty.ToString();
                IEnumerable<PuzzleFlag> allPuzzleFlags = PuzzleFlags.EnumerateComponents(parameters.Puzzles);
                List<string> requests = allPuzzleFlags.Select(puzzleFlag => puzzleFlag.ToString()).ToList();
                List<string> jadeBoxes = parameters.JadeBoxes.Select(jadeBox => jadeBox.Id).ToList();

                Dictionary<string, PluginInfo> pluginInfos = BepInEx.Bootstrap.Chainloader.PluginInfos;
                List<Mod> mods = new List<Mod>();

                foreach (string guid in Configs.PriorityMods)
                {
                    if (pluginInfos.TryGetValue(guid, out PluginInfo pluginInfo)) mods.Add(ParseMod(pluginInfo));
                }

                foreach ((string guid, PluginInfo pluginInfo) in pluginInfos)
                {
                    if (Configs.PriorityMods.Contains(guid) || Configs.ExcludedMods.Contains(guid)) continue;
                    if (guid == Configs.HelpMeEirinMod) hasClearBonus = true;
                    Mod mod = ParseMod(pluginInfo);
                    if (guid == Configs.PatchouliMod) mod.Configs = PatchouliModWrapper.GetConfigs();
                    mods.Add(mod);
                }

                Controller.Instance.RunLog.Version = VersionInfo.Current.Version;
                if (BepinexPlugin.saveProfileName.Value) Controller.Instance.RunLog.Name = parameters.UserProfile.Name;
                Settings settings = new Settings()
                {
                    Character = character,
                    PlayerType = playerType,
                    HasClearBonus = hasClearBonus,
                    ShowRandomResult = showRandomResult,
                    IsAutoSeed = isAutoSeed,
                    Difficulty = difficulty,
                    Requests = requests,
                    Mods = mods
                };
                if (jadeBoxes.Count > 0) settings.JadeBoxes = jadeBoxes;
                Controller.Instance.RunLog.Settings = settings;
            }

            private static Mod ParseMod(PluginInfo pluginInfo)
            {
                string guid = pluginInfo.Metadata.GUID;
                string name = pluginInfo.Metadata.Name;
                string version = pluginInfo.Metadata.Version.ToString();
                Mod mod = new Mod()
                {
                    GUID = guid,
                    Name = name,
                    Version = version
                };
                return mod;
            }
        }
    }
}