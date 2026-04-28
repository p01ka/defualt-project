using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace IdxZero.Utils
{
    public class UserUtils
    {
        private static bool? _isFirstRun;
        public static bool IsFirstRun()
        {
            if (_isFirstRun.HasValue)
                return _isFirstRun.Value;

            if (!PlayerPrefs.HasKey(PrefConstKeys.FirstRunTimeKey))
            {
                _isFirstRun = true;
                PlayerPrefs.SetInt(PrefConstKeys.FirstRunTimeKey, default);
            }
            else
            {
                _isFirstRun = false;
            }
            return _isFirstRun.Value;
        }

        public static IEnumerator<float> Post(string url, string json, Action<string> response = null)
        {
            UnityEngine.Debug.Log("START TO POST");
            var request = new UnityWebRequest(url, "POST");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            yield return MEC.Timing.WaitUntilDone(request.SendWebRequest());
            string responseText = request.downloadHandler.text;
            Debug.Log("Status Code: " + request.responseCode);

            if (!String.IsNullOrEmpty(responseText))
            {
                Debug.Log("Response " + responseText);
                response?.Invoke(responseText);
            }
            else
            {
                response?.Invoke(request.responseCode.ToString());
            }
        }
    }
}