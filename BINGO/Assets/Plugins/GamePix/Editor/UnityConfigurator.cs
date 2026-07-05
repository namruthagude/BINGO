using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.Rendering;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;

namespace GamePix.Editor
{
    public partial class UnityConfigurator
    {
        private static int webGLMemorySize = 128;
        private static string platform = "WebGL";
        private static readonly string GlobalDefinesFileName = "csc.rsp";
        private static AddRequest addPackageRequest;

        public static void SetWebGLSettings(bool safeMode = false, AssetsCompression assetsCompression = AssetsCompression.NoOverride)
        {
            SetBuildProfile();
            SetPlayerSettings(safeMode);
            SetEditorUserBuildSettings();
            SetWebGLMemorySize(webGLMemorySize);
            Emscripten.SetArguments();
            SetTexturesWebGLSettings(assetsCompression);
            SetAtlasWebGLSettings(assetsCompression);
            SetSoundsWebGLSettings(assetsCompression);
            DisableUnityAds();
        }

        private static void SetBuildProfile()
        {
#if UNITY_6000_0_OR_NEWER
            UnityEditor.Build.Profile.BuildProfile.SetActiveBuildProfile(null);
#endif
        }

        public static void SetWebGLMemorySize(int memorySize)
        {
            var currentMemorySize = PlayerSettings.WebGL.memorySize;
            if (currentMemorySize > 0 && !memorySize.Equals(currentMemorySize))
            {
                Debug.Log(String.Format("WebGL memory size:\n{0}\nreplaced to:\n{1}", currentMemorySize, memorySize));
            }
            PlayerSettings.WebGL.memorySize = memorySize;

#if UNITY_2022_1_OR_NEWER     
            PlayerSettings.WebGL.initialMemorySize = memorySize;
            PlayerSettings.WebGL.memoryGrowthMode = WebGLMemoryGrowthMode.Geometric;
            PlayerSettings.WebGL.geometricMemoryGrowthStep = 0.2f;
            PlayerSettings.WebGL.memoryGeometricGrowthCap = 96;
            PlayerSettings.WebGL.maximumMemorySize = 2048;
            PlayerSettings.WebGL.powerPreference = WebGLPowerPreference.HighPerformance;
#endif
            Debug.Log(String.Format("Set WebGL memorySize: {0}.", PlayerSettings.WebGL.memorySize));
        }

        public static void SetPlayerSettings(bool safeMode)
        {
            // Resolution and Presentation
            PlayerSettings.runInBackground = true;
            PlayerSettings.WebGL.template = "APPLICATION:Minimal";

            // Splash Image
            PlayerSettings.SplashScreen.show = false;

            // Other settings
#if UNITY_2022
            PlayerSettings.SetUseDefaultGraphicsAPIs(BuildTarget.WebGL, false);
            PlayerSettings.SetGraphicsAPIs(
                BuildTarget.WebGL, 
                new [] {
                    GraphicsDeviceType.OpenGLES3,
                    GraphicsDeviceType.OpenGLES2 
            });
#else
            PlayerSettings.SetUseDefaultGraphicsAPIs(BuildTarget.WebGL, true);
#endif

#if UNITY_2023_1_OR_NEWER
            PlayerSettings.SetIl2CppCompilerConfiguration(NamedBuildTarget.WebGL, Il2CppCompilerConfiguration.Master);
            PlayerSettings.SetManagedStrippingLevel(NamedBuildTarget.WebGL, ManagedStrippingLevel.High);
#else
            PlayerSettings.SetIl2CppCompilerConfiguration(BuildTargetGroup.WebGL, Il2CppCompilerConfiguration.Master);
            PlayerSettings.SetManagedStrippingLevel(BuildTargetGroup.WebGL, ManagedStrippingLevel.High);

            // WebGL 1 support only gamma color space
            PlayerSettings.colorSpace = ColorSpace.Gamma;
#endif

            PlayerSettings.stripEngineCode = true;
            PlayerSettings.stripUnusedMeshComponents = true;
            PlayerSettings.bakeCollisionMeshes = true;

#if UNITY_2020_1_OR_NEWER
            PlayerSettings.mipStripping = false;
#endif
            var settingsContent = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/ProjectSettings.asset")[0];
            var serializedManager = new SerializedObject(settingsContent);
            serializedManager.FindProperty("submitAnalytics").boolValue = false;
            serializedManager.FindProperty("keepLoadedShadersAlive").boolValue = true;
            serializedManager.FindProperty("VertexChannelCompressionMask").intValue = -1;
            serializedManager.ApplyModifiedProperties();

            // WebGL
            PlayerSettings.WebGL.exceptionSupport = safeMode
                ? WebGLExceptionSupport.FullWithoutStacktrace
                : WebGLExceptionSupport.ExplicitlyThrownExceptionsOnly;
            PlayerSettings.WebGL.linkerTarget = WebGLLinkerTarget.Wasm;
            PlayerSettings.WebGL.dataCaching = false;
#if UNITY_2021_2_OR_NEWER 
            PlayerSettings.WebGL.debugSymbolMode = WebGLDebugSymbolMode.External;
#else
            PlayerSettings.WebGL.debugSymbols = true;
#endif
            PlayerSettings.WebGL.nameFilesAsHashes = false;
            PlayerSettings.WebGL.threadsSupport = false;
            PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Disabled;
#if UNITY_2019
            PlayerSettings.WebGL.wasmStreaming = false;
#endif
#if UNITY_2020_1_OR_NEWER
            PlayerSettings.WebGL.decompressionFallback = false;
#endif
#if UNITY_2023_2_OR_NEWER
            PlayerSettings.WebGL.showDiagnostics = false;
            PlayerSettings.WebGL.powerPreference = WebGLPowerPreference.HighPerformance;
            PlayerSettings.WebGL.webAssemblyTable = true;
            PlayerSettings.WebGL.webAssemblyBigInt = true;
            PlayerSettings.WebGL.threadsSupport = false;
#endif
            Debug.Log("Set player settings");
        }

        public static void SetEditorUserBuildSettings()
        {
#if UNITY_2021_2_OR_NEWER
            EditorUserBuildSettings.webGLBuildSubtarget = WebGLTextureSubtarget.Generic;
#endif
            EditorUserBuildSettings.development = false;
#if UNITY_2021_2 || UNITY_2021_3
            EditorUserBuildSettings.il2CppCodeGeneration = Il2CppCodeGeneration.OptimizeSize;
#elif UNITY_2022_1_OR_NEWER
            PlayerSettings.SetIl2CppCodeGeneration(NamedBuildTarget.WebGL, Il2CppCodeGeneration.OptimizeSize);
#endif
#if UNITY_2020_2_OR_NEWER
            EditorUserBuildSettings.SetPlatformSettings(BuildPipeline.GetBuildTargetName(BuildTarget.WebGL), "CodeOptimization", "size");
#endif
#if UNITY_2023_1_OR_NEWER
            PlayerSettings.SetIl2CppStacktraceInformation(NamedBuildTarget.WebGL, Il2CppStacktraceInformation.MethodOnly);
#endif

            Debug.Log("Set editor user build settings");
        }

        public static void SetTexturesWebGLSettings(AssetsCompression assetsCompression) 
        {
#if UNITY_2021_2_OR_NEWER
            if (assetsCompression == AssetsCompression.Uncompressed)
            {
                EditorUserBuildSettings.overrideTextureCompression = OverrideTextureCompression.ForceUncompressed;
                EditorUserBuildSettings.overrideMaxTextureSize = 2048;
            }
            else
            {
                EditorUserBuildSettings.overrideTextureCompression = OverrideTextureCompression.NoOverride;
                EditorUserBuildSettings.overrideMaxTextureSize = 0;
            }
#endif
            if (assetsCompression == AssetsCompression.NoOverride)
            {
                return;
            }

            string[] textureGuids = AssetDatabase.FindAssets("t:texture");
            foreach (string guid in textureGuids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
                if (textureImporter == null)
                {
                    continue;
                }

                TextureImporterPlatformSettings platformTextureSettings = textureImporter.GetPlatformTextureSettings(platform);
                platformTextureSettings.overridden = true;
                platformTextureSettings.compressionQuality = 100;
                platformTextureSettings.maxTextureSize = 2048;
                if (assetsCompression == AssetsCompression.Uncompressed)
                {
                    platformTextureSettings.format = textureImporter.textureType == TextureImporterType.SingleChannel
                        ? TextureImporterFormat.Alpha8
                        : TextureImporterFormat.RGBA32;
                }
                else
                {
                    platformTextureSettings.format = TextureImporterFormat.DXT5Crunched;
                }

                textureImporter.SetPlatformTextureSettings(platformTextureSettings);
                textureImporter.SaveAndReimport();
            }
            Debug.Log("Set textures settings");
        }

        public static void SetAtlasWebGLSettings(AssetsCompression assetsCompression) 
        {
            if (assetsCompression == AssetsCompression.NoOverride)
            {
                return;
            }

            string[] atlasGuids = AssetDatabase.FindAssets("t:spriteatlas");
            foreach (string guid in atlasGuids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                SpriteAtlas atlas = AssetDatabase.LoadAssetAtPath(path, typeof(SpriteAtlas)) as SpriteAtlas;

                if (atlas == null)
                {
                    continue;
                }

                TextureImporterPlatformSettings platformAtlasSettings = atlas.GetPlatformSettings(platform);
                platformAtlasSettings.overridden = true;
                platformAtlasSettings.compressionQuality = 100;
                platformAtlasSettings.maxTextureSize = 2048;
                if (assetsCompression == AssetsCompression.Uncompressed)
                {
                    platformAtlasSettings.textureCompression = TextureImporterCompression.Uncompressed;
                    platformAtlasSettings.format = TextureImporterFormat.RGBA32;
                }
                else
                {
                    platformAtlasSettings.textureCompression = TextureImporterCompression.Compressed;
                }

                atlas.SetPlatformSettings(platformAtlasSettings);
            }

            Debug.Log("Set sprite atlas settings");
        }

        public static void SetSoundsWebGLSettings(AssetsCompression assetsCompression) 
        {
            if (assetsCompression == AssetsCompression.NoOverride)
            {
                return;
            }

            string[] guids = AssetDatabase.FindAssets("t:audioClip");
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                AudioImporter audioImporter = AssetImporter.GetAtPath(path) as AudioImporter;
                if (audioImporter == null)
                {
                    continue;
                }

                AudioImporterSampleSettings platformAudioSettings = audioImporter.GetOverrideSampleSettings(platform);
#if UNITY_2022_2_OR_NEWER
                platformAudioSettings.preloadAudioData = false;
#else
                audioImporter.preloadAudioData = false;
#endif
                
                platformAudioSettings.quality = assetsCompression == AssetsCompression.Compressed ? 0.5f : 1f;
                platformAudioSettings.compressionFormat = AudioCompressionFormat.AAC;
                platformAudioSettings.loadType = AudioClipLoadType.DecompressOnLoad;
                audioImporter.SetOverrideSampleSettings(platform, platformAudioSettings);
                audioImporter.SaveAndReimport();
            }
            Debug.Log("Set audio clips settings");
        }

        private static void DisableUnityAds()
        {
            string unityConnectSettingsPath = "ProjectSettings/UnityConnectSettings.asset";
            UnityEngine.Object[] unityConnectSettings =
               AssetDatabase.LoadAllAssetsAtPath(unityConnectSettingsPath);
            if (unityConnectSettings == null || unityConnectSettings.Length == 0 || unityConnectSettings[0] == null)
            {
                Debug.LogErrorFormat("Can't load UnityConnectSettings.asset at path: {0}", unityConnectSettingsPath);
                return;
            }
            SerializedObject serializedObject = new(unityConnectSettings[0]);
            serializedObject.FindProperty("m_Enabled").boolValue = false;

            DisableChildProperty(serializedObject, "UnityPurchasingSettings", "m_Enabled");
            DisableChildProperty(serializedObject, "UnityAnalyticsSettings", "m_Enabled");
            DisableChildProperty(serializedObject, "UnityAdsSettings", "m_Enabled");
            DisableChildProperty(serializedObject, "PerformanceReportingSettings", "m_Enabled");
#if UNITY_6000_2_OR_NEWER
            DisableChildProperty(serializedObject, "InsightsSettings", "m_Enabled");
            DisableChildProperty(serializedObject, "InsightsSettings", "m_EngineDiagnosticsEnabled");
            DisableChildProperty(serializedObject, "CrashReportingSettings", "m_EnableCloudDiagnosticsReporting");
#else
            DisableChildProperty(serializedObject, "CrashReportingSettings", "m_Enabled");
#endif

            serializedObject.ApplyModifiedProperties();
            AssetDatabase.SaveAssets();
        }

        private static void DisableChildProperty(SerializedObject serializedObject, string rootProperty, string childProperty)
        {
            SerializedProperty root = serializedObject.FindProperty(rootProperty);
            if (root != null)
            {
                SerializedProperty child = root.FindPropertyRelative(childProperty);
                if (child != null)
                {
                    child.boolValue = false;
                }
                else
                {
                    Debug.LogErrorFormat("Can't find child property: {0} for property: {1}", childProperty, rootProperty);
                }
            }
            else
            {
                Debug.LogErrorFormat("Can't find property: {0}", rootProperty);
            }
        }

        public static void AddCustomDefine(string customDefine)
        {
            string define = "-define:" + customDefine;
            string globalDefinesPath = Path.Combine("Assets", GlobalDefinesFileName);
            string globalDefinesContent = "";
            if (File.Exists(globalDefinesPath))
            {
                using (StreamReader streamReader = new StreamReader(globalDefinesPath, System.Text.Encoding.UTF8))
                {
                    globalDefinesContent = streamReader.ReadToEnd();
                }
                if (globalDefinesContent.Contains(define))
                {
                    return;
                }
            }

            globalDefinesContent = string.IsNullOrWhiteSpace(globalDefinesContent)
                ? define
                : globalDefinesContent + Environment.NewLine + define;

            using (StreamWriter streamWriter = new StreamWriter(globalDefinesPath, false, System.Text.Encoding.UTF8))
            {
                streamWriter.Write(globalDefinesContent);
            }
        }

        public static void RemoveCustomDefine(string customDefine)
        {
            string define = "-define:" + customDefine;
            string globalDefinesPath = Path.Combine("Assets", GlobalDefinesFileName);
            if (File.Exists(globalDefinesPath))
            {
                string globalDefinesContent;
                using (StreamReader streamReader = new StreamReader(globalDefinesPath, System.Text.Encoding.UTF8))
                {
                    globalDefinesContent = streamReader.ReadToEnd();
                }

                int defineStartAt = globalDefinesContent.IndexOf(define, StringComparison.InvariantCulture);
                if (defineStartAt > -1)
                {
                    using (StreamWriter streamWriter =
                           new StreamWriter(globalDefinesPath, false, System.Text.Encoding.UTF8))
                    {
                        streamWriter.Write(globalDefinesContent.Remove(defineStartAt, define.Length).Trim());
                    }
                }
            }
        }
                
        public static void InstallPackage(string packageName)
        {
            addPackageRequest = Client.Add(packageName);
            EditorApplication.update += InstallPackageProgress;
        }

        private static void InstallPackageProgress()
        {
            if (addPackageRequest.IsCompleted)
            {
                if (addPackageRequest.Status == StatusCode.Success)
                {
                    Debug.Log("Installed: " + addPackageRequest.Result.packageId);
                }
                else if (addPackageRequest.Status >= StatusCode.Failure)
                {
                    Debug.Log(addPackageRequest.Error.message);
                }

                EditorApplication.update -= InstallPackageProgress;
            }
        }
    }
}
