using System.IO;
using UnityEditor;
using UnityEngine;

    internal class LogTool : EditorWindow
    {
        public static string LogPath = $"{Application.dataPath}/LogOut";

        //[MenuItem("Tool/清空日志")]//#E
        public static void ClearLog()
        {
            if (!Directory.Exists(LogPath)) return;
            File.Delete($"{LogPath}.meta");
            Directory.Delete(LogPath, true );
            AssetDatabase.Refresh();
        }
    }

