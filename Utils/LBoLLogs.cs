using System.Text;
using UnityEngine.Networking;
using System.Collections;
using Newtonsoft.Json;
using UnityEngine;
using BepInEx;
using System.Collections.Generic;
using LBoL.Core;
using LBoL.Presentation;
using RunLogger.Utils.Enums;

namespace RunLogger.Utils
{
    internal class LBoLLogs : MonoBehaviour
    {
        internal static void Upload(string description = null)
        {
            if (!description.IsNullOrWhiteSpace()) Controller.Instance.RunLog.Description = description;
            Singleton<GameMaster>.Instance.StartCoroutine(LBoLLogs.Post());
        }

        private static IEnumerator Post()
        {
            ObjectsManager.Object.Upload?.SetActive(false);
            LBoLLogs.Log(UploadStatus.Uploading);
            UnityWebRequest request = new UnityWebRequest(Configs.GasUrl, "POST");
            byte[] data = Encoding.UTF8.GetBytes(Logger.Encode(Controller.Instance.RunLog, false));
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(data);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            bool isNew = LBoLLogs.HandleResponse(request.downloadHandler.text, out Dictionary<string, object> result);
            if (isNew)
            {
                result.TryGetValue("url", out object value);
                string url = (string)value;
                BepinexPlugin.log.LogDebug(url);
                LBoLLogs.Log(UploadStatus.Uploaded, url);
                Logger.DeleteLog(Controller.Instance.Path);
            }
            else
            {
                LBoLLogs.Log(UploadStatus.Failed);
            }
            Controller.DestroyInstance();

            yield break;
        }

        private static bool HandleResponse(string response, out Dictionary<string, object> result)
        {
            result = null;
            if (response.IsNullOrWhiteSpace()) return false;
            result = JsonConvert.DeserializeObject<Dictionary<string, object>>(response);
            if (!result.TryGetValue("isNew", out object value)) return false;
            bool isNew = (bool)value;
            return isNew;
        }

        private static void Log(string uploadStatus, string url = null)
        {
            ObjectsManager.UpdateStatus(uploadStatus, url);
            BepinexPlugin.log.LogDebug(uploadStatus);
        }
    }
}