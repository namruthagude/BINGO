using UnityEditor;
using UnityEngine;

namespace GamePix.Editor
{
    public class BuildWindow
    {
        private static readonly string buildDirectory = "html5";
        private static readonly string dialogTitle = "GamePix";
        private static readonly string webglIsNotInstalled = "Can't build the game. Unity WebGL Platform is not installed";
        private static readonly string latestVersionMessage = "The latest GamePix Unity3D plugin version is used";
        private static readonly string okButton = "Ok";

        [MenuItem("GamePix/Build and Run", false, -1)]
        static void Build()
        {
            Build(true, AssetsCompression.Uncompressed);
        }

        [MenuItem("GamePix/Safe builds/Build and Run (No override textures\\sounds)", false, 2)]
        static void DefaultResourceBuild()
        {
            Build(true, AssetsCompression.NoOverride);
        }
        
        [MenuItem("GamePix/Safe builds/Build and Run (Compress textures\\sounds)", false, 3)]
        static void CompressedResourceBuild()
        {
            Build(true, AssetsCompression.Compressed);
        }

        private static void Build(bool safeMode, AssetsCompression assetsCompression)
        {
            if (!BuildPipeline.IsBuildTargetSupported(BuildTargetGroup.WebGL, BuildTarget.WebGL)) {
                EditorUtility.DisplayDialog(dialogTitle, webglIsNotInstalled, okButton);
                return;
            }

            string emscriptenEditorVersion = Emscripten.editorVersion;
            string emscriptenPluginVersion = Emscripten.pluginVersion;
            
            if (emscriptenEditorVersion != emscriptenPluginVersion)
            {
                Debug.LogError("This Gamepix plugin version [emscripten: " + emscriptenPluginVersion +
                               "] cannot be used for Unity " + ApplicationInfo.unityVersion +
                               " [emscripten: " + emscriptenEditorVersion +
                               "]. Check Gamepix plugin for Unity: " + Updater.defaultUrl);
                return;
            }
            Updater.TryUpdatePlugin(() =>
            {
                UnityConfigurator.SetWebGLSettings(safeMode, assetsCompression);
                Builder.BuildArchive(buildDirectory, assetsCompression, DataFileCompression.Uncompressed, true);
            });
        }

        [MenuItem("GamePix/Check update", false, 2)]
        static void CheckUpdate()
        {
            Updater.TryUpdatePlugin(() =>
            {
                Debug.Log(latestVersionMessage);
                EditorUtility.DisplayDialog(dialogTitle, latestVersionMessage, okButton);
            });
        }
    }
}