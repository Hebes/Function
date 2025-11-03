using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic; 

public class FindScriptReferences : EditorWindow
{
    private static MonoScript targetScript;
    private Vector2 scrollPos;
    private List<string> results = new List<string>();

    //[MenuItem("Assets/查找脚本引用", false, 25)]
    private static void FindScriptUsage()
    {
        targetScript = Selection.activeObject as MonoScript;

        if (targetScript == null)
        {
            EditorUtility.DisplayDialog("提示", "请选择一个脚本文件 (.cs)", "确定");
            return;
        }

        // 打开结果窗口
        FindScriptReferences window = GetWindow<FindScriptReferences>("脚本引用结果");
        window.Show();

        window.FindReferences();
    }

    private void OnGUI()
    {
        if (targetScript == null)
        {
            EditorGUILayout.HelpBox("请在 Project 窗口中右键一个脚本文件，然后选择“查找脚本引用”", MessageType.Info);
            return;
        }

        GUILayout.Space(10);
        EditorGUILayout.LabelField("当前查找脚本:", EditorStyles.boldLabel);
        EditorGUILayout.ObjectField(targetScript, typeof(MonoScript), false);

        GUILayout.Space(10);
        GUILayout.Label($"引用结果 ({results.Count})", EditorStyles.boldLabel);

        scrollPos = GUILayout.BeginScrollView(scrollPos);
        foreach (var path in results)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(path, EditorStyles.linkLabel))
            {
                var obj = AssetDatabase.LoadAssetAtPath<Object>(path);
                Selection.activeObject = obj;
                EditorGUIUtility.PingObject(obj);
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndScrollView();
    }

    private void FindReferences()
    {
        results.Clear();

        string scriptPath = AssetDatabase.GetAssetPath(targetScript);
        string targetGUID = AssetDatabase.AssetPathToGUID(scriptPath);

        string[] allAssets = AssetDatabase.GetAllAssetPaths();
        int count = 0;
        int total = allAssets.Length;

        try
        {
            foreach (string path in allAssets)
            {
                count++;
                if (count % 500 == 0)
                    EditorUtility.DisplayProgressBar("搜索引用中", $"检查 {count}/{total}", (float)count / total);

                // 限定类型，避免扫描无关文件
                if (path.EndsWith(".prefab") || path.EndsWith(".unity") || path.EndsWith(".asset") || path.EndsWith(".mat"))
                {
                    string text = File.ReadAllText(path);
                    if (text.Contains(targetGUID))
                        results.Add(path);
                }
            }
        }
        finally
        {
            EditorUtility.ClearProgressBar();
        }

        Debug.Log($"🔍 找到 {results.Count} 个引用 {targetScript.name} 的资源。");
        Repaint();
    }
}

