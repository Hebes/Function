using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class FindObject : EditorWindow
{
    private static Object targetAsset;
    private static List<ObjectData> _objectDatas = new();
    private Vector2 scrollPosition;
    private Dictionary<string, string> guidToPath = new Dictionary<string, string>();
    private List<string> searchResults = new List<string>();
    private static string searchFilter = "t:";
    private string exportPath = "";


    [MenuItem("Tools/GUID操作/验证GUID有效性")]
    public static void ValidateGUIDs()
    {
        List<string> invalidGuids = new List<string>();
        List<string> validGuids = new List<string>();

        // 获取所有GUID进行验证
        string[] allGuids = AssetDatabase.FindAssets("t:Prefab");

        foreach (string guid in allGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            if (string.IsNullOrEmpty(path))
            {
                invalidGuids.Add(guid);
            }
            else
            {
                validGuids.Add(guid);
            }
        }

        Debug.Log($"有效GUID: {validGuids.Count} 个");
        Debug.Log($"无效GUID: {invalidGuids.Count} 个");

        if (invalidGuids.Count > 0)
        {
            Debug.LogWarning("发现无效GUID:");
            foreach (string invalidGuid in invalidGuids)
            {
                Debug.LogWarning(invalidGuid);
            }
        }
    }

    [MenuItem("Tools/GUID操作/查找重复资产")]
    public static void FindDuplicateAssets()
    {
        Dictionary<string, List<string>> pathToGuids = new Dictionary<string, List<string>>();

        string[] allGuids = AssetDatabase.FindAssets("t:Prefab");

        foreach (string guid in allGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);

            if (!pathToGuids.ContainsKey(path))
            {
                pathToGuids[path] = new List<string>();
            }

            pathToGuids[path].Add(guid);
        }

        // 查找有多个GUID指向同一路径的情况
        foreach (var kvp in pathToGuids)
        {
            if (kvp.Value.Count > 1)
            {
                Debug.LogWarning($"路径 '{kvp.Key}' 有 {kvp.Value.Count} 个GUID:");
                foreach (string guid in kvp.Value)
                {
                    Debug.LogWarning($"  - {guid}");
                }
            }
        }
    }

    [MenuItem("Tools/快速示例/获取选中预制体GUID")]
    public static void QuickExample()
    {
        // 获取所有选中的预制体
        foreach (Object obj in Selection.objects)
        {
            if (obj is GameObject)
            {
                string path = AssetDatabase.GetAssetPath(obj);
                string guid = AssetDatabase.AssetPathToGUID(path);

                Debug.Log($"预制体: {obj.name}");
                Debug.Log($"路径: {path}");
                Debug.Log($"GUID: {guid}");

                // 复制GUID到剪切板
                EditorGUIUtility.systemCopyBuffer = guid;
                Debug.Log("GUID已复制到剪切板");
            }
        }
    }

    [MenuItem("Tools/快速示例/按文件夹获取GUID")]
    public static void GetGUIDsByFolder()
    {
        string folderPath = "Assets/Art"; // 修改为你的文件夹路径

        string[] guids = AssetDatabase.FindAssets("", new[] { folderPath });

        Debug.Log($"在文件夹 {folderPath} 中找到 {guids.Length} 个资产");

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Debug.Log($"{Path.GetFileName(path)} - {guid}");
        }
    }

    [MenuItem("Assets/查找脚本引用", false, 25)]
    public static void Find()
    {
        //GetWindow<FindObject>("GUID收集器");
        // targetAsset = Selection.activeObject;
        // if (targetAsset == null)
        // {
        //     EditorUtility.DisplayDialog("提示", "请选择一个资源（Prefab、脚本、贴图、材质等）", "确定");
        //     return;
        // }
        // FindObject window = GetWindow<FindObject>("资源引用结果");
        // window.Show();
        //window.FindReferences();


        // 获取所有预制体的GUID
        string[] prefabGuids = AssetDatabase.FindAssets("t:Prefab");
        Debug.Log($"找到 {prefabGuids.Length} 个预制体");
        //
        // // 获取所有场景的GUID
        // string[] sceneGuids = AssetDatabase.FindAssets("t:Scene");
        // Debug.Log($"找到 {sceneGuids.Length} 个场景");
        //
        // // 获取所有材质的GUID
        // string[] materialGuids = AssetDatabase.FindAssets("t:Material");
        // Debug.Log($"找到 {materialGuids.Length} 个材质");
        //
        // // 获取所有脚本的GUID
        // string[] scriptGuids = AssetDatabase.FindAssets("t:Script");
        // Debug.Log($"找到 {scriptGuids.Length} 个脚本");
        //
        // 获取所有纹理的GUID
        // string[] textureGuids = AssetDatabase.FindAssets("t:Texture");
        // Debug.Log($"找到 {textureGuids.Length} 个纹理");
        
        // 组合显示
        foreach (string guid in prefabGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Debug.Log($"预制体 - GUID: {guid}, 路径: {path}");
            _objectDatas.Add(new ObjectData(guid,path));
        }
    }

    private void SearchAssets()
    {
        searchResults.Clear();
        guidToPath.Clear();

        string[] guids = AssetDatabase.FindAssets(searchFilter);

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            searchResults.Add(guid);
            guidToPath[guid] = path;
        }

        // 按路径排序
        searchResults.Sort((a, b) => guidToPath[a].CompareTo(guidToPath[b]));
    }

    private void SelectAsset(string guid)
    {
        string path = AssetDatabase.GUIDToAssetPath(guid);
        Object asset = AssetDatabase.LoadAssetAtPath<Object>(path);
        Selection.activeObject = asset;
        EditorGUIUtility.PingObject(asset);
    }

    private void CopyToClipboard(string text)
    {
        EditorGUIUtility.systemCopyBuffer = text;
        Debug.Log($"已复制到剪切板: {text}");
    }

    private void FindReferences()
    {
        string[] allAssets = AssetDatabase.GetAllAssetPaths();
        string assetPath = AssetDatabase.GetAssetPath(targetAsset);
        string targetGUID = AssetDatabase.AssetPathToGUID(assetPath);

        int count = 0;
        int total = allAssets.Length;

        try
        {
            foreach (string path in allAssets)
            {
                count++;
                if (count % 500 == 0)
                    EditorUtility.DisplayProgressBar("搜索引用中", $"检查 {count}/{total}", (float)count / total);
            }
        }
        finally
        {
            EditorUtility.ClearProgressBar();
        }

        //Debug.Log($"🔍 找到 {results.Count} 个引用 {targetAsset.name} 的资源。");
        Repaint();
    }

    // private void OnGUI()
    // {
    //     GUILayout.Space(10);
    //
    //     // 搜索筛选
    //     GUILayout.Label("资产筛选", EditorStyles.boldLabel);
    //     searchFilter = EditorGUILayout.TextField("筛选条件:", searchFilter);
    //
    //     GUILayout.BeginHorizontal();
    //     {
    //         if (GUILayout.Button("搜索资产", GUILayout.Height(30)))
    //             SearchAssets();
    //
    //         if (GUILayout.Button("清空结果", GUILayout.Height(30)))
    //         {
    //             searchResults.Clear();
    //             guidToPath.Clear();
    //         }
    //     }
    //     GUILayout.EndHorizontal();
    //
    //     GUILayout.Space(10);
    //
    //     // 结果显示
    //     GUILayout.Label($"找到 {searchResults.Count} 个资产", EditorStyles.boldLabel);
    //
    //     scrollPosition = GUILayout.BeginScrollView(scrollPosition);
    //     {
    //         foreach (string guid in searchResults)
    //         {
    //             string path = guidToPath[guid];
    //             GUILayout.BeginHorizontal();
    //             {
    //                 GUILayout.Label(guid, GUILayout.Width(280));
    //                 GUILayout.Label(path, GUILayout.ExpandWidth(true));
    //
    //                 if (GUILayout.Button("选择", GUILayout.Width(50)))
    //                 {
    //                     SelectAsset(guid);
    //                 }
    //
    //                 if (GUILayout.Button("复制", GUILayout.Width(50)))
    //                 {
    //                     CopyToClipboard(guid);
    //                 }
    //             }
    //             GUILayout.EndHorizontal();
    //             GUILayout.Space(2);
    //         }
    //     }
    //     GUILayout.EndScrollView();
    //
    //     GUILayout.Space(10);
    //
    //     // 导出功能
    //     GUILayout.Label("导出功能", EditorStyles.boldLabel);
    //     GUILayout.BeginHorizontal();
    //     {
    //         exportPath = EditorGUILayout.TextField("导出路径:", exportPath);
    //         if (GUILayout.Button("浏览", GUILayout.Width(50)))
    //         {
    //             exportPath = EditorUtility.SaveFolderPanel("选择导出目录", "", "");
    //         }
    //     }
    //     GUILayout.EndHorizontal();
    //
    //     if (GUILayout.Button("导出到CSV", GUILayout.Height(30)))
    //     {
    //         ExportToCSV();
    //     }
    // }

    private void ExportToCSV()
    {
        if (string.IsNullOrEmpty(exportPath))
        {
            EditorUtility.DisplayDialog("错误", "请先选择导出路径", "确定");
            return;
        }

        string filePath = Path.Combine(exportPath, "asset_guids.csv");
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine("GUID,AssetPath,FileName,FileExtension");

            foreach (string guid in searchResults)
            {
                string path = guidToPath[guid];
                string fileName = Path.GetFileName(path);
                string extension = Path.GetExtension(path);

                writer.WriteLine($"\"{guid}\",\"{path}\",\"{fileName}\",\"{extension}\"");
            }
        }

        Debug.Log($"已导出到: {filePath}");
        EditorUtility.RevealInFinder(filePath);
    }


    //private Vector2 scrollPosition;
    private Dictionary<string, List<string>> sceneDependencies = new Dictionary<string, List<string>>();

    // [MenuItem("Tools/场景引用分析器")]
    // public static void ShowWindow()
    // {
    //     GetWindow<FindObject>("场景引用分析");
    // }
    // void OnGUI()
    // {
    //     if (GUILayout.Button("分析所有场景引用", GUILayout.Height(40)))
    //     {
    //         AnalyzeSceneReferences();
    //     }
    //
    //     scrollPosition = GUILayout.BeginScrollView(scrollPosition);
    //     {
    //         foreach (var kvp in sceneDependencies)
    //         {
    //             string sceneGuid = kvp.Key;
    //             string scenePath = AssetDatabase.GUIDToAssetPath(sceneGuid);
    //
    //             EditorGUILayout.LabelField($"场景: {scenePath}", EditorStyles.boldLabel);
    //             EditorGUILayout.LabelField($"GUID: {sceneGuid}");
    //
    //             EditorGUI.indentLevel++;
    //             foreach (string dependencyGuid in kvp.Value)
    //             {
    //                 string dependencyPath = AssetDatabase.GUIDToAssetPath(dependencyGuid);
    //                 EditorGUILayout.LabelField(dependencyPath);
    //             }
    //
    //             EditorGUI.indentLevel--;
    //
    //             EditorGUILayout.Space();
    //         }
    //     }
    //     GUILayout.EndScrollView();
    // }

    private void AnalyzeSceneReferences()
    {
        sceneDependencies.Clear();

        string[] sceneGuids = AssetDatabase.FindAssets("t:Scene");

        foreach (string sceneGuid in sceneGuids)
        {
            string scenePath = AssetDatabase.GUIDToAssetPath(sceneGuid);
            string[] dependencies = AssetDatabase.GetDependencies(scenePath, true);

            List<string> dependencyGuids = new List<string>();
            foreach (string dependencyPath in dependencies)
            {
                if (dependencyPath != scenePath) // 排除自身
                {
                    string dependencyGuid = AssetDatabase.AssetPathToGUID(dependencyPath);
                    dependencyGuids.Add(dependencyGuid);
                }
            }

            sceneDependencies[sceneGuid] = dependencyGuids;
        }

        Debug.Log($"分析了 {sceneGuids.Length} 个场景的引用关系");
    }
}

/// <summary>
/// 物体数据
/// </summary>
public class ObjectData
{
    public string Guid;
    public string Path;

    public ObjectData(string guid, string path)
    {
        Guid = guid;
        Path = path;
    }
}