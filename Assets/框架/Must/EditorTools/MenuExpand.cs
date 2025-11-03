using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEditor;

/// <summary>
/// 菜单扩展
/// </summary>
public class MenuExpand
{
    [MenuItem("Assets/输出当前选中代码的信息", false, 80)]
    public static void PrintNowSelectionScriptInfo()
    {
        var fileNameList = new List<string>();
        foreach (var path in Selection.objects.Select(t => AssetDatabase.GetAssetPath(t)))
        {
            if (Directory.Exists(path))
                fileNameList.AddRange(Directory.GetFiles(path, "*.cs", SearchOption.AllDirectories));
            else if (File.Exists(path))
            {
                var fileInfo = new FileInfo(path);
                if (fileInfo.Extension == ".cs")
                    fileNameList.Add(path);
            }
        }

        var totalLine = 0; //总行数
        var totalWord = 0; //总字数
        foreach (var temp in fileNameList)
        {
            var nowLine = 0;
            var sr = new StreamReader(temp);
            var readLine = sr.ReadLine();
            while (readLine != null)
            {
                nowLine++;
                totalWord += readLine.Length;
                readLine = sr.ReadLine();
            }

            //文件名+文件行数
            //Debug.Log($"{temp}——{nowLine}");
            totalLine += nowLine;
        }

        EditorUtility.DisplayDialog("Unity",
            $"总代码行数：{totalLine}\n" +
            $"总代码字数：{totalWord}\n" +
            $"总cs文件数量：{fileNameList.Count}", "ok");
    }


    [MenuItem("Tools / 创建常用文件夹")]
    public static void CreateFolder()
    {
        return;
        string path = Application.dataPath + "/";
        Directory.CreateDirectory(path + "Resources");
        Directory.CreateDirectory(path + "Assets");
        Directory.CreateDirectory(path + "Plugins");
        Directory.CreateDirectory(path + "StreamingAssets");
        Directory.CreateDirectory(path + "Editor");
        Directory.CreateDirectory(path + "Scenes");
        Directory.CreateDirectory(path + "Scripts");
    }


    /// <summary>
    /// shader引用查找
    /// </summary>
    [MenuItem("Tools / shader引用查找")]
    public static void ShaderCiteFind()
    {
        UnityEngine.Shader selectedShader = Selection.activeObject as UnityEngine.Shader;
        if (selectedShader != null)
        {
            string[] guids = AssetDatabase.FindAssets("t:Material");
            bool use = false;
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                Material material = AssetDatabase.LoadAssetAtPath<Material>(path);
                if (material.shader == selectedShader)
                {
                    use = true;
                    Debug.Log("Material " + material.name + " 在项目中的路径是：" + path);
                }
            }

            if (!use)
                Debug.Log("选中的" + selectedShader + " 未被任何材质球使用");
        }
        else
        {
            Debug.Log("请在Project面板中选中一个Shader");
        }
    }

    /// <summary>
    /// 获取当前项目某个脚本的路径
    /// </summary>
    /// <param name="scriptName">脚本的名字</param>
    /// <returns></returns>
    static string GetPath(string scriptName)
    {
        var paths = AssetDatabase.FindAssets(scriptName);
        if (paths.Length > 1)
        {
            Debug.LogError("有同名文件" + scriptName + "获取路径失败");
            return null;
        }

        //将字符串中得脚本后缀去除掉
        return AssetDatabase.GUIDToAssetPath(paths[0]);
    }
}