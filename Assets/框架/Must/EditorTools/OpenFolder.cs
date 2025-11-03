using UnityEditor;
using UnityEngine;

namespace ToolEditor
{
    /// <summary>
    /// 打开文件夹
    /// </summary>
    public class OpenFolder
    {
        [MenuItem("Tools/打开ApplicationpersistentDataPath")]
        private static void Run()
        {
            //System.Diagnostics.Process.Start("explorer.exe", Application.persistentDataPath);
            EditorUtility.RevealInFinder(Application.persistentDataPath);
        }
    }
}
