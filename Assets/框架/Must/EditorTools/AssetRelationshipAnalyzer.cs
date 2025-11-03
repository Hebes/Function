#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 资产关系分析器
/// </summary>
public class AssetRelationshipAnalyzer : EditorWindow
{
    private Object targetAsset;
    private Vector2 scrollPosition;
    private List<AssetInfo> dependencies = new List<AssetInfo>();
    private List<AssetInfo> references = new List<AssetInfo>();

    [System.Serializable]
    public class AssetInfo
    {
        public string name;
        public string path;
        public string guid;
        public string type;
    }

    [MenuItem("Tools/资产关系分析器")]
    public static void ShowWindow()
    {
        GetWindow<AssetRelationshipAnalyzer>("关系分析器");
    }

    void OnGUI()
    {
        GUILayout.Label("资产关系分析", EditorStyles.boldLabel);
        
        // 资产选择
        EditorGUI.BeginChangeCheck();
        targetAsset = EditorGUILayout.ObjectField("目标资产", targetAsset, typeof(Object), false);
        if (EditorGUI.EndChangeCheck() && targetAsset != null)
        {
            AnalyzeRelationships();
        }
        
        if (targetAsset == null)
        {
            EditorGUILayout.HelpBox("请选择一个资产进行分析", MessageType.Info);
            return;
        }
        
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        
        // 显示资产基本信息
        string assetPath = AssetDatabase.GetAssetPath(targetAsset);
        string guid = AssetDatabase.AssetPathToGUID(assetPath);
        
        EditorGUILayout.Space();
        GUILayout.Label("基本信息", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("名称", targetAsset.name);
        EditorGUILayout.LabelField("路径", assetPath);
        EditorGUILayout.LabelField("GUID", guid);
        EditorGUILayout.LabelField("类型", targetAsset.GetType().Name);
        
        // 依赖关系
        EditorGUILayout.Space();
        GUILayout.Label($"依赖项 ({dependencies.Count})", EditorStyles.boldLabel);
        foreach (var dep in dependencies)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(dep.name, $"{dep.type} - {dep.guid}");
            if (GUILayout.Button("选择", GUILayout.Width(50)))
            {
                SelectAssetByGUID(dep.guid);
            }
            EditorGUILayout.EndHorizontal();
        }
        
        // 引用关系
        EditorGUILayout.Space();
        GUILayout.Label($"引用项 ({references.Count})", EditorStyles.boldLabel);
        foreach (var refAsset in references)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(refAsset.name, $"{refAsset.type} - {refAsset.guid}");
            if (GUILayout.Button("选择", GUILayout.Width(50)))
            {
                SelectAssetByGUID(refAsset.guid);
            }
            EditorGUILayout.EndHorizontal();
        }
        
        EditorGUILayout.EndScrollView();
    }
    
    private void AnalyzeRelationships()
    {
        dependencies.Clear();
        references.Clear();
        
        string targetPath = AssetDatabase.GetAssetPath(targetAsset);
        string targetGUID = AssetDatabase.AssetPathToGUID(targetPath);
        
        // 分析依赖项
        string[] dependencyPaths = AssetDatabase.GetDependencies(targetPath, true);
        foreach (string depPath in dependencyPaths)
        {
            if (depPath != targetPath) // 排除自己
            {
                var assetInfo = CreateAssetInfo(depPath);
                if (assetInfo != null)
                    dependencies.Add(assetInfo);
            }
        }
        
        // 分析引用项
        string[] allAssetPaths = AssetDatabase.GetAllAssetPaths();
        
        foreach (string assetPath in allAssetPaths)
        {
            if (assetPath.StartsWith("Assets/") && assetPath != targetPath)
            {
                string[] assetDependencies = AssetDatabase.GetDependencies(assetPath, false);
                if (System.Array.Exists(assetDependencies, path => path == targetPath))
                {
                    var assetInfo = CreateAssetInfo(assetPath);
                    if (assetInfo != null)
                        references.Add(assetInfo);
                }
            }
        }
        Repaint();
    }
    
    private AssetInfo CreateAssetInfo(string assetPath)
    {
        Object asset = AssetDatabase.LoadAssetAtPath<Object>(assetPath);
        if (asset == null) return null;
        
        return new AssetInfo
        {
            name = asset.name,
            path = assetPath,
            guid = AssetDatabase.AssetPathToGUID(assetPath),
            type = asset.GetType().Name
        };
    }
    
    private void SelectAssetByGUID(string guid)
    {
        string path = AssetDatabase.GUIDToAssetPath(guid);
        Object asset = AssetDatabase.LoadAssetAtPath<Object>(path);
        if (asset != null)
        {
            Selection.activeObject = asset;
            EditorGUIUtility.PingObject(asset);
        }
    }
}
#endif