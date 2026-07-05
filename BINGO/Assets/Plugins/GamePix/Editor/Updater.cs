using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace GamePix.Editor
{
    public static class Updater
    {
        private static readonly string gpxBackend = "https://rt.gamepix.com/unity/plugin/get?unity={0}&plugin={1}";
        public static readonly string defaultUrl = "https://my.gamepix.com/sdk/doc/unity-plugin";

        private static readonly string title = "GamePix Update";
        private static readonly string errorMessage = "GamePix update service is unavailable. More info in console";
        private static readonly string errorButton = "Ok";
        private static readonly string newVersionMessage = "A new version of GamePix Unity3D plugin is available\n" +
                                                           "*Please remove old version of GamePix Unity3D plugin before update";

        private static readonly string newVersionButton = "Download";
        private static readonly string newVersionButtonCancel = "Cancel";

        public static void TryUpdatePlugin(Action onUseLatest)
        {
            string url = string.Format(gpxBackend, ApplicationInfo.unityVersion, ApplicationInfo.pluginVersion);
            UnityWebRequest request = UnityWebRequest.Get(url);
            UnityWebRequestAsyncOperation requestOperation = request.SendWebRequest();

            requestOperation.completed += operation =>
            {
#if UNITY_2020_1_OR_NEWER
                if (request.result != UnityWebRequest.Result.Success)
#else
                if (request.isHttpError || request.isNetworkError || !string.IsNullOrEmpty(request.error))
#endif
                {
                    Debug.LogError(errorMessage + ": " + request.error);
                    EditorUtility.DisplayDialog(title, errorMessage, errorButton);
                    return;
                }

                string responseText = request.downloadHandler.text;
                try
                {
                    PluginUpdateResponse response = JsonUtility.FromJson<PluginUpdateResponse>(responseText);
                    if (response != null)
                    {
                        if (response.lastVersion)
                        {
                            onUseLatest();
                            return;
                        }

                        if (!string.IsNullOrWhiteSpace(response.url))
                        {
                            Debug.LogWarning(newVersionMessage + ": " + response.url);
                            if (EditorUtility.DisplayDialog(title, newVersionMessage, newVersionButton,
                                    newVersionButtonCancel))
                            {
                                Application.OpenURL(response.url);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError("Can't parse GamePix update service response: " + responseText + " Details: " + ex);
                }
            };
        }

        [Serializable]
        private class PluginUpdateResponse
        {
            public bool lastVersion;
            public string url;
        }
    }
}