using System.Text;
using UnityEngine.Networking;
using System.Collections;
using Newtonsoft.Json;
using UnityEngine;
using BepInEx;
using System.Collections.Generic;
using LBoL.Core;
using LBoL.Presentation;

namespace RunLogger.Utils
{
    internal class LBoLLogs : MonoBehaviour
    {
        internal static void Upload(string description = null)
        {
            Controller.Instance.RunLog.Description = description;
            Singleton<GameMaster>.Instance.StartCoroutine(LBoLLogs.Post());
        }

        private static IEnumerator Post()
        {
            UnityWebRequest request = new UnityWebRequest(Configs.GasUrl, "POST");
            byte[] data = Encoding.UTF8.GetBytes(Logger.Encode(Controller.Instance.RunLog, false));
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(data);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            bool isNew = LBoLLogs.HandleResponse(request.downloadHandler.text);
            if (isNew)
            {
                BepinexPlugin.log.LogDebug("Uploaded");
                Logger.DeleteLog(Controller.Instance.Path);
            }
            else
            {
                BepinexPlugin.log.LogDebug("Upload failed");
            }
            Controller.DestroyInstance();

            yield break;
        }

        private static bool HandleResponse(string response)
        {
            if (response.IsNullOrWhiteSpace()) return false;
            Dictionary<string, object> result = JsonConvert.DeserializeObject<Dictionary<string, object>>(response);
            if (!result.TryGetValue("isNew", out object value)) return false;
            bool isNew = (bool)value;
            return isNew;
        }
    }
}