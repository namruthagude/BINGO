using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace GamePix.Editor
{
    public static class Emscripten
    {
        private static readonly string emscriptenArgs =
            "-Oz " +
#if UNITY_2021_2_OR_NEWER
            "-s \"EXPORTED_FUNCTIONS=[" +
            "'_gpxLoadTextureAsync'" +
            ",'_gpxAllocNextChannel','_gpxSetSoundVolume','_gpxPlaySound','_gpxStopSound','_gpxResumeSounds','_gpxGetSoundDuration'" +
            ",'_main','_malloc','_free','_abort'" +
            "]\" " +
            "-s MIN_WEBGL_VERSION=1 -s MAX_WEBGL_VERSION=2 " +
#endif
            "--profiling-funcs " +
            "-s GL_PREINITIALIZED_CONTEXT=1 " +
            "-s USE_WEBGL2=1 " +
            "-s PRECISE_F32=0 " +
            "-s ALLOW_MEMORY_GROWTH={0} -s TOTAL_MEMORY=134217728 " +
            "-s FORCE_FILESYSTEM=1 " +
            "-s \"EXPORTED_RUNTIME_METHODS=['getMemory','stackTrace','addRunDependency','removeRunDependency'" +
            ",'FS_createPath','FS_createDataFile','FS_createPreloadedFile','ccall','cwrap','callMain','dynCall'" +
            ",'lengthBytesUTF8','stringToUTF8','UTF8ToString','UTF16ToString','Pointer_stringify']\"";

        private static readonly string editorVersionPath = Path.Combine(
            Path.GetDirectoryName(EditorApplication.applicationPath),
#if UNITY_EDITOR_WIN || UNITY_EDITOR_LINUX
            "Data",
#endif
            "PlaybackEngines", "WebGLSupport", "BuildTools", "Emscripten",
#if UNITY_2021_2_OR_NEWER
            "emscripten",
#endif
            "emscripten-version.txt");

        private static readonly string pluginVersionPath = Path.Combine(
            Application.dataPath,
            "Plugins", "GamePix", "impl",
            "emscripten-version.txt");

        private static readonly char[] trimChars = { '"', ' ', '\n', '\r' };

        public static string editorVersion => GetEmscriptenVersion(editorVersionPath);

        public static string pluginVersion => GetEmscriptenVersion(pluginVersionPath);

        public static void SetArguments(bool allowMemoryGrow = true)
        {
            string args = string.Format(emscriptenArgs, allowMemoryGrow ? 1 : 0);
            string currentArgs = PlayerSettings.WebGL.emscriptenArgs;
            if (!string.IsNullOrEmpty(currentArgs) && !currentArgs.Equals(args))
            {
                Debug.LogFormat("Emscripten arguments:\n{0}\nreplaced to:\n{1}", currentArgs, args);
            }

            PlayerSettings.WebGL.emscriptenArgs = args;
            Debug.LogFormat("Set Emscripten arguments: {0}.", PlayerSettings.WebGL.emscriptenArgs);
        }

        private static string GetEmscriptenVersion(string path)
        { 
            return File.ReadAllText(path).Trim(trimChars);
        }
    }
}