using GamePix.Ads;
using GamePix.Events;
using UnityEngine;
using System;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace GamePix
{
    public class Gpx: MonoBehaviour
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void gpxCallback();

        public static SystemLanguage CurrentLanguage { get; private set; } = SystemLanguage.English;
        
        public static bool IsGamePaused { get; private set; }

#if UNITY_EDITOR || !UNITY_WEBGL
        public static IAds Ads = new UnityAds();
        public static IEvents Events = new UnityEvents();
        
        public static void Log(string message)
        {
            Debug.Log(message);
        }
#else
        private static GameObject gpx = null;

        public static IAds Ads = new GamepixAds();
        public static IEvents Events = new GamepixEvents();

        private static float gpxTimeScaleBeforePause; 
        private static bool gpxAudioListenerPausedBeforePause;

        [DllImport("__Internal")] 
        private static extern void gpxLog(string message);

        [DllImport("__Internal")]
        private static extern void gpxReady(int argc,
                                            string[] args,
                                            [MarshalAs(UnmanagedType.LPTStr)]string projectName, 
                                            [MarshalAs(UnmanagedType.FunctionPtr)] gpxCallback gamePause, 
                                            [MarshalAs(UnmanagedType.FunctionPtr)] gpxCallback gameResume);

            
        [DllImport("__Internal")]
        private static extern void gpxTick();

        public static void Log(string message)
        {
            gpxLog(message);
        }

        private void Update() 
        {
            gpxTick();
        }
            
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnBeforeSceneLoadRuntimeMethod()
        {
            if (gpx == null) 
            {
                Ready(Environment.GetCommandLineArgs()); 
                gpx = new GameObject("gpx");
                gpx.AddComponent<Gpx>();
                DontDestroyOnLoad(gpx);
            }
        }

        private static void Ready(string[] args)
        {
            gpxTimeScaleBeforePause = Time.timeScale;
            gpxAudioListenerPausedBeforePause = AudioListener.pause;
            IsGamePaused = false;
            string applicationName = Regex.Replace(Application.productName, @"\W", "");
            gpxReady(args.Length, args, applicationName, GamePause, GameResume);
            InitEnvironment(args);
        }

        private static void InitEnvironment(string[] args)
        {
            if (args == null)
            {
                return;
            } 
            if (args.Length > 3)
            {
                SetLanguage(args[3]);
            }
        }

        [AOT.MonoPInvokeCallback(typeof(gpxCallback))]
        private static void GamePause()
        {
            if (IsGamePaused)
            {
                return;
            }
            gpxAudioListenerPausedBeforePause = AudioListener.pause;
            AudioListener.pause = true;
            gpxTimeScaleBeforePause = Time.timeScale;
            Time.timeScale = 0; 
            IsGamePaused = true;
        }
        
        [AOT.MonoPInvokeCallback(typeof(gpxCallback))]
        private static void GameResume()
        {
            if (!IsGamePaused) 
            {
                return;
            }
            Time.timeScale = gpxTimeScaleBeforePause;
            AudioListener.pause = gpxAudioListenerPausedBeforePause;
            IsGamePaused = false;
        }

        private static void SetLanguage(string languageName)
        {
            CurrentLanguage = SystemLanguage.English;
            string language = languageName.Trim().Substring(0, 2);
            if (String.IsNullOrEmpty(language) || language.Length != 2) 
            {
                return;
            }

            var cultures = CultureInfo.GetCultures(CultureTypes.NeutralCultures);
            if (cultures == null)
            {
               Log("ERR!! Unity not found list of cultures");
                return;
            }

            var culture = cultures.Where(c => c.TwoLetterISOLanguageName.Equals(language, StringComparison.OrdinalIgnoreCase))
                                .FirstOrDefault();
            if (culture == null) 
            {
               Log("ERR!! Unity not found culture for language: " + languageName);
                return;
            }

            foreach(var systemLanguage in Enum.GetValues(typeof(SystemLanguage)))
            {
                if (culture.EnglishName.Equals(systemLanguage.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    CurrentLanguage = (SystemLanguage)systemLanguage;
                    return; 
                }
            }
           Log("ERR!! Unity not found system language by culture: " + culture.EnglishName);
        }
#endif
    }
}