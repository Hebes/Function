using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.CodeEditor;
using UnityEditor.SceneManagement;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using Debug = UnityEngine.Debug;

namespace Tool
{
    /// <summary>
    /// 工具拓展
    /// </summary>
    public static class ToolExpansion
    {
        #region AB

        /// <summary>
        /// 设置ab包名称
        /// </summary>
        public static void SetAssetBundleName(string path, string assetBundleName)
        {
            // 设置ab包
            AssetImporter assetImporter1 = AssetImporter.GetAtPath(path);
            assetImporter1.assetBundleName = assetBundleName;
            AssetDatabase.Refresh();
            Debug.Log("设置AB包名称成功!");
        }

        /// <summary>
        /// 设置单个资源的ABName
        /// </summary>
        /// <param name="abName"></param>
        /// <param name="path">资源路径</param>
        public static string SetAbName(this string path, string abName)
        {
            AssetImporter ai = AssetImporter.GetAtPath(path);
            if (ai != null)
                ai.assetBundleName = abName;
            return abName;
        }

        #endregion

        #region AssetDatabase

        /// <summary>
        /// 重命名API
        /// </summary>
        /// <param name="pathName"></param>
        /// <param name="newName"></param>
        public static void RenameAsset(this string pathName, string newName)
        {
            AssetDatabase.RenameAsset(pathName, newName);//改名API
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        public static void Refresh()
        {
            AssetDatabase.Refresh();
        }

        #endregion

        #region RunBat

        /// <summary>
        /// 执行Bat
        /// </summary>
        /// <param name="batPath">bat的exe路径</param>
        private static void RunBat(string batPath)
        {
            // 设置要执行的.bat文件和工作目录
            var startInfo = new ProcessStartInfo();
            startInfo.FileName = batPath;
            var directoryPath = Path.GetDirectoryName(startInfo.FileName);
            if (string.IsNullOrEmpty(directoryPath))
                throw new Exception("当前路径为空");
            startInfo.WorkingDirectory = directoryPath;

            // 创建进程对象
            var process = new Process();
            process.StartInfo = startInfo;
            // // 设置进程文件路径
            // process.StartInfo.FileName = "cmd.exe";
            // // 设置传递给 cmd.exe 的参数，包括批处理文件路径和执行参数（如果有）
            // process.StartInfo.Arguments = $"/C {batchFilePath}";

            // 隐藏命令行窗口
            //process.StartInfo.CreateNoWindow = true;
            //process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

            process.Start(); // 启动进程
            process.WaitForExit(); // 等待进程完成执行
            var exitCode = process.ExitCode; // 获取批处理文件的退出码
            process.Close(); // 关闭进程
            Debug.LogError(exitCode == 0 ? "批处理文件执行成功." : $"批处理文件执行失败。退出代码: {exitCode}");
        }

        #endregion

        #region RunExe

        /// <summary>
        /// 执行Exe
        /// </summary>
        /// <param name="str"></param>
        private static void RunExe(string str)
        {
            var process = new Process();
            process.StartInfo.FileName = str;
            process.Start();

            // // 可选：等待进程执行完成
            // process.WaitForExit();
            //
            // // 关闭进程
            // process.Close();
        }

        #endregion

        #region Copy

        /// <summary>
        /// Copy到剪切板 https://blog.csdn.net/LLLLL__/article/details/114463650
        /// </summary>
        public static void Copy(this string str)
        {
            TextEditor te = new TextEditor();
            te.text = str;
            te.SelectAll();
            te.Copy();
        }

        /// <summary>
        /// Copy到剪切板-Unity3D自带版本 https://blog.csdn.net/LLLLL__/article/details/114463650
        /// </summary>
        public static void Copy_Unity(this string str)
        {
            GUIUtility.systemCopyBuffer = str;
        }

        #endregion

        #region DateSave

        /// <summary>
        /// 保存修改
        /// </summary>
        public static void SaveModification(this Object[] objs)
        {
            Array.ForEach(objs, (obj) =>
            {
                Undo.RecordObject(obj, obj.name);
                EditorUtility.SetDirty(obj);
            });
        }

        /// <summary>
        /// 保存修改
        /// </summary>
        public static void Save()
        {
            EditorSceneManager.SaveScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        }

        /// <summary>
        /// 若是assets文件夹资源, 则刷新assets
        /// </summary>
        public static void ReAssets(this Object obj)
        {
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 若是assets文件夹资源, 则刷新assets
        /// </summary>
        public static void ReAssets()
        {
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 若是assets文件夹资源, 则刷新assets
        /// </summary>
        public static void ReAssets(this Object[] objects)
        {
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 刷新编辑器
        /// </summary>
        public static void RefreshAssetDatabase()
        {
            //刷新unity编辑器
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 刷新编辑器
        /// </summary>
        public static void RefreshAssetDatabase(this Object obj)
        {
            //刷新unity编辑器
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 刷新编辑器
        /// </summary>
        public static void RefreshAssetDatabase(this Object[] obj)
        {
            //刷新unity编辑器
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 刷新编辑器
        /// </summary>
        public static void RefreshAssetDatabase(this GameObject obj)
        {
            //刷新unity编辑器
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 刷新编辑器
        /// </summary>
        public static void RefreshAssetDatabase(this string folderPath)
        {
            //刷新unity编辑器
            AssetDatabase.Refresh();
        }

        #endregion

        #region Debug

        /// <summary>
        /// 清空日志
        /// </summary>
        public static void ClearConsole()
        {
            Assembly assembly = Assembly.GetAssembly(typeof(SceneView));
            System.Type logEntries = assembly.GetType("UnityEditor.LogEntries");
            MethodInfo clearConsoleMethod = logEntries.GetMethod("Clear");
            if (clearConsoleMethod==null) throw new Exception("执行的方法为空");
            clearConsoleMethod.Invoke(new object(), null);
        }

        #endregion

        #region File

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="filePath"></param>
        public static void DelFile(this string filePath)
        {
            File.Delete(filePath);
        }

        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="filePath"></param>
        public static void CreatFile(this string filePath)
        {
            if (File.Exists(filePath)) return;
            Debug.Log("文件不存在,开始创建!");
            File.Create(filePath);
        }

        /// <summary>
        /// 检查文件
        /// </summary>
        /// <param name="filePath">文件夹路径</param>
        public static bool CheckFile(this string filePath)
        {
            return File.Exists(filePath);//是否存在这个文件
        }

        /// <summary>
        /// 生成文件并写入内容
        /// </summary>
        /// <param name="folderPath">文件夹路径</param>
        /// <param name="fileName">文件名</param>
        /// <param name="content">内容</param>
        public static void CreatCSharpScript(string folderPath, string fileName, string content)
        {
            //创建并写入内容
            string filePath = $"{folderPath}/{fileName}";
            if (!File.Exists(filePath))
            {
                Debug.Log("文件不存在,进行创建...");
                using StreamWriter writer = File.CreateText(filePath);
                writer.Write(content);
                Debug.Log("内容写入成功!");
            }
            folderPath.RefreshAssetDatabase();
        }

        /// <summary>
        /// 文件以追加写入的方式
        /// https://wenku.baidu.com/view/a8fdb767fd4733687e21af45b307e87100f6f85b.html
        /// 显示IO异常请在创建文件的时候Close下
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="content">内容</param>
        private static void FileWriteContent(this string path, string content)
        {
            byte[] myByte = System.Text.Encoding.UTF8.GetBytes(content);
            using var fsWrite = new FileStream(path, FileMode.Append, FileAccess.Write);
            fsWrite.Write(myByte, 0, myByte.Length);
        }

        /// <summary>
        /// 生成文件并写入内容
        /// </summary>
        /// <param name="folderPath">文件夹路径</param>
        /// <param name="fileName">文件名</param>
        /// <param name="content">内容</param>
        public static void CreatScript(string folderPath, string fileName, string content)
        {
            //创建并写入内容
            var filePath = $"{folderPath}/{fileName}";
            if (!File.Exists(filePath))
            {
                Debug.Log("文件不存在,进行创建...");
                using var writer = File.CreateText(filePath);
                writer.Write(content);
                Debug.Log("内容写入成功!");
            }
            folderPath.RefreshAssetDatabase();
        }

        /// <summary>
        /// 创建文件并写入
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="str"></param>
        public static void CreateFileText(this string filePath, string str)
        {
            using var writer = File.CreateText(filePath);
            writer.Write(str); Debug.Log("内容写入成功!");
        }

        #endregion

        #region Folder

        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="folderPath"></param>
        /// <returns></returns>
        public static void CreatFolder(this string folderPath)
        {
            if (Directory.Exists(folderPath)) return;
            Debug.Log("文件不存在,开始创建!");
            Directory.CreateDirectory(folderPath);//创建
        }

        /// <summary>
        /// 通过路径检文件夹是否存在，如果不存在则创建
        /// </summary>
        /// <param name="folderPath">文件夹路径</param>
        public static bool CheckFolder(this string folderPath)
        {
            return Directory.Exists(folderPath);//是否存在这个文件
        }

        /// <summary>
        /// 打开文件夹
        /// </summary>
        /// <param name="folderPath"></param>
        public static void OpenFolder(string folderPath)
        {
            if (!Directory.Exists(folderPath))
                throw new Exception("当前文件夹路径错误");
            System.Diagnostics.Process.Start(folderPath);
        }

        #endregion

        #region TXT

        /// <summary>
        /// txt读取
        /// </summary>
        /// <param name="txtPath"></param>
        /// <returns></returns>
        public static Dictionary<string, string> TxtRead(this string txtPath)
        {
            Dictionary<string, string> DesDic = new Dictionary<string, string>();
            FileStream fileStream = File.Open(txtPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            StreamReader reader = new StreamReader(fileStream);
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] strings = line.Split(',');
                DesDic.Add(strings[0], strings[1]);
            }
            reader.Close();
            fileStream.Close();
            return DesDic;
        }

        #endregion
        
        /// <summary>
        /// 开打.sln路径,
        /// </summary>
        public static void OSOpenFile(this string path)
        {
            CodeEditor.OSOpenFile(CodeEditor.CurrentEditorInstallation, Path.Combine(Application.dataPath, path));
        }

        /// <summary>
        /// 打开路径
        /// </summary>
        /// <param name="folderPath">路径</param>
        public static void OpenPath(this string folderPath)
        {
            if (!Directory.Exists(folderPath)) return;
            EditorUtility.RevealInFinder(folderPath);
        }

        /// <summary>
        /// 删除文件夹
        /// </summary>
        public static void DelFolder(this string folderPath)
        {
            Directory.Delete(folderPath);
        }

        /// <summary>
        /// 删除文件夹
        /// </summary>
        public static void DelFolder(this string folderPath, bool recursive)
        {
            Directory.Delete(folderPath, recursive);
        }

        public static string[] GetFiles(this string folderPath)
        {
            return Directory.GetFiles(folderPath);
        }

        #region Find

        /// <summary>
        /// 获取选中
        /// </summary>
        /// <returns></returns>
        public static UnityEngine.Object[] GetObjArray()
        {
            return Selection.objects;
        }

        /// <summary>
        /// 获取选中
        /// </summary>
        /// <returns></returns>
        public static UnityEngine.Object[] GetObjArray(this Object obj)
        {
            return Selection.objects;
        }

        /// <summary>
        /// 获取一个
        /// </summary>
        /// <returns></returns>
        public static UnityEngine.Object GetObj()
        {
            return Selection.activeObject;
        }

        /// <summary>
        /// 获取返回的
        /// </summary>
        /// <returns></returns>
        public static List<GameObject> GetGos(this UnityEngine.Object[] objects)
        {
            List<GameObject> gos = new List<GameObject>();
            Array.ForEach(objects, (obj) => { gos.Add(obj as GameObject); });
            return gos;
        }

        /// <summary>
        /// 查找物体(PS：包含隐藏版、关键词开头,Hierarchy面板)
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="keyValue">关键词</param>
        /// <param name="goList"></param>
        public static void LoopGetKeywordGO(this Transform transform, string keyValue, ref List<GameObject> goList)
        {
            if (transform.name.StartsWith(keyValue))
                goList.Add(transform.gameObject);
            for (int i = 0; i < transform?.childCount; i++)
                transform.GetChild(i).LoopGetKeywordGO(keyValue, ref goList);
        }

        /// <summary>
        /// 查找物体(PS：包含隐藏版、关键词开头,Hierarchy面板,子物体)
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="keyValue">关键词</param>
        /// <param name="goList"></param>
        public static List<GameObject> LoopGetKeywordGO(this GameObject transform, string keyValue)
        {
            List<GameObject> goList = new List<GameObject>();
            if (transform.name.StartsWith(keyValue))
                goList.Add(transform.gameObject);
            for (int i = 0; i < transform?.transform.childCount; i++)
                transform.transform.GetChild(i).LoopGetKeywordGO(keyValue, ref goList);
            return goList;
        }

        /// <summary>
        /// 查找物体(PS：包含隐藏版,Hierarchy面板)
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="goList"></param>
        public static void LoopGetAllGameObject(this Transform transform, ref List<GameObject> goList)
        {
            goList.Add(transform.gameObject);
            for (int i = 0; i < transform?.childCount; i++)
                transform.GetChild(i).LoopGetAllGameObject(ref goList);
        }

        /// <summary>
        /// 查找物体(PS：包含隐藏版,Hierarchy面板)
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="transformPrefix"></param>
        /// <param name="gameObjects"></param>
        public static void LoopGetAllTransform(this Transform transform, ref List<Transform> transforms)
        {
            transforms.Add(transform);
            for (int i = 0; i < transform?.childCount; i++)
                transform.GetChild(i).LoopGetAllTransform(ref transforms);
        }



        /// <summary>
        /// 获取Hierarchy全部物体
        /// </summary>
        /// <returns></returns>
        public static Object[] GetHierarchyAllGameObject()
        {
            return Resources.FindObjectsOfTypeAll<Object>();
        }

        /// <summary>
        /// 查找子物体.(PS:适用于单个,代码中可直接用,可找到隐藏物体)
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="childName">物体名称</param>
        /// <returns></returns>
        public static Transform LoopGetOneTransform(this Transform transform, string childName)
        {
            //递归:方法内部又调用自身的过程。
            //1.在子物体中查找
            Transform childTF = transform.Find(childName);
            if (childTF != null) return childTF;
            for (int i = 0; i < transform?.childCount; i++)
            {
                childTF = transform.GetChild(i).LoopGetOneTransform(childName); // 2.将任务交给子物体
                if (childTF != null) return childTF;
            }
            Debug.Log($"没有找到物体{childName}"); return null;
        }

        /// <summary>
        /// 找到子对象的对应控件 返回一本字典(P:组件类型为Key)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="gameObject"></param>
        /// <param name="keyValue"></param>
        /// <param name="controlDic"></param>
        public static void GetAllToDic<T>(this GameObject gameObject, string keyValue, ref Dictionary<string, List<Component>> controlDic) where T : Component
        {
            //返回所有keyValue开头的物品
            List<GameObject> transforms = gameObject.LoopGetKeywordGO(keyValue);

            //添加进字典
            for (int i = 0; i < transforms?.Count; i++)
            {
                GameObject tempGo = transforms[i];//组件的物体
                T go = tempGo.GetComponent<T>();
                if (go == null) continue;//如果获取组件空 跳过
                string objType = go.GetType().Name;//组件的类型
                if (controlDic.ContainsKey(objType))//添加组件
                    controlDic[objType].Add(go);
                else
                    controlDic.Add(objType, new List<Component>() { go });
            }
        }

        /// <summary>
        /// 查找物体,包含隐藏的
        /// </summary>
        /// <param name="gameObject">物体</param>
        /// <param name="keyWord">关键词</param>
        /// <returns></returns>
        public static List<Transform> GetTransforms(this GameObject gameObject, string keyWord)
        {
            List<Transform> gos = gameObject.GetComponentsInChildren<Transform>(true).ToList();
            return gos.FindAll((go) => { return go.name.StartsWith(keyWord); });
        }

        /// <summary>
        /// 获取组件路径
        /// </summary>
        /// <param name="transformTF">需要获取路径的子物体</param>
        /// <param name="selectGoName">选择的父物体(transformTF要在这个父物体下)</param>
        /// <returns>返回的路径</returns>
        public static string GetPathTransform(this Transform transformTF, string selectGoName)
        {
            //临时变量-存放路径
            List<string> strs = new List<string>();
            string path = string.Empty;
            //获取路径
            strs.Add(transformTF.name);
            while (transformTF.parent != null)
            {
                transformTF = transformTF.parent;
                if (transformTF.name == selectGoName) break;
                strs.Add(transformTF.name);
            }
            //转换成路径
            for (int j = strs.Count - 1; j >= 0; j--)
            {
                path += j != 0 ? $"{strs[j]}/" : $"{strs[j]}";
            }
            return path;
        }

        /// <summary>
        /// 获取资源路径(PS:Project面板选中的物体)
        /// </summary>
        public static string GetAssetDataPath(this UnityEngine.Object @object)
        {
            return AssetDatabase.GetAssetPath(@object);
        }

        #endregion

        #region Script

        /// <summary>
        /// 移除脚本(PS:自定义版本,一个物体里面)
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="InputCustom">自定义脚本名称</param>
        public static void ACRemoveScript(this GameObject gameObject, String InputCustom)
        {
            //正常来说肯定有一个
            Array.ForEach(gameObject.GetComponents<Component>(), (component) =>
            {
                Type type = component.GetType();
                if (type.Name == InputCustom)
                {
                    UnityEngine.Object.DestroyImmediate(gameObject.GetComponent(type));
                    Debug.Log($"{gameObject.name} 移除 {InputCustom}脚本成功");
                }
            });
        }

        /// <summary>
        /// 移除丢失脚本
        /// https://blog.csdn.net/SendSI/article/details/114369256
        /// </summary>
        /// <param name="gameObject">需要移除丢失脚本的物体</param>
        public static void ACRemoveMissScriptAll(this UnityEngine.Object obj)
        {
            //获取所有的物体
            List<GameObject> gameObjects = new List<GameObject>();
            (obj as GameObject).transform.LoopGetAllGameObject(ref gameObjects);
            //移除Miss脚本
            gameObjects?.ForEach(gameObject =>
            {
                GameObjectUtility.RemoveMonoBehavioursWithMissingScript(gameObject);
            });
            AssetDatabase.Refresh();
            Debug.Log("清理完成!");
        }

        /// <summary>
        /// 移除丢失脚本
        /// https://blog.csdn.net/SendSI/article/details/114369256
        /// </summary>
        /// <param name="gameObject">需要移除丢失脚本的物体</param>
        public static void ACRemoveMissScriptAll(this UnityEngine.Object[] objs)
        {
            Array.ForEach(objs, (obj) =>
            {
                //获取所有的物体
                List<GameObject> gameObjects = new List<GameObject>();
                (obj as GameObject).transform.LoopGetAllGameObject(ref gameObjects);
                //移除Miss脚本
                gameObjects?.ForEach(gameObject =>
                {
                    GameObjectUtility.RemoveMonoBehavioursWithMissingScript(gameObject);
                });
                objs.RefreshAssetDatabase();
                Debug.Log("清理完成!");
            });

        }

        /// <summary>
        /// 移除丢失脚本
        /// https://blog.csdn.net/SendSI/article/details/114369256
        /// </summary>
        /// <param name="gameObject">需要移除丢失脚本的物体</param>
        public static void ACRemoveMissScriptAll(this GameObject gameObject)
        {
            //获取所有的物体
            List<GameObject> gameObjects = new List<GameObject>();
            gameObject.transform.LoopGetAllGameObject(ref gameObjects);
            //移除Miss脚本
            gameObjects?.ForEach(gameObject =>
            {
                GameObjectUtility.RemoveMonoBehavioursWithMissingScript(gameObject);
            });
            gameObject.RefreshAssetDatabase();
            Debug.Log("清理完成!");
        }

        /// <summary>
        /// 移除Miss的脚本
        /// </summary>
        /// <param name="gameObject"></param>
        public static void ACRemoveMissScriptOne(this GameObject gameObject)
        {
            GameObjectUtility.RemoveMonoBehavioursWithMissingScript(gameObject);
        }


        /// <summary>
        /// 添加脚本(PS:自定义版本,一个物体里面)
        /// </summary>
        public static void ACAddScript(this GameObject gameObject, String InputCustom)
        {
            Component[] components = gameObject.GetComponents<Component>();
            bool isExist = false;//是否存在
            foreach (Component comp in components)//正常来说肯定有一个
            {
                Type type = comp.GetType();
                if (type.Name == InputCustom)
                {
                    isExist = true;
                    break;
                }
            }

            if (!isExist)
            {
                //Animator
                Type type = null;
                if (type == null) type = InputCustom.ReflectClass("UnityEngine.UI");
                if (type == null) type = InputCustom.ReflectClass("UnityEngine");
                gameObject.AddComponent(type);
                Debug.Log($"{gameObject.name}添加{type.Name}完成");
            }
        }

        #endregion

        #region String

        /// <summary>
        /// 添加一个物体名字前缀
        /// </summary>
        /// <param name="gameObject">物体</param>
        /// <param name="prefix">前缀</param>
        private static void AddPrefix(this GameObject gameObject, string prefix)
        {
            gameObject.name = gameObject.name.StartsWith(prefix) ? gameObject.name : $"{prefix}{gameObject.name}";
        }

        /// <summary>
        /// 删除一个物体名字前缀
        /// </summary>
        /// <param name="gameObject">物体</param>
        /// <param name="prefix">前缀</param>
        private static void RemovePrefix(this GameObject gameObject, string prefix)
        {
            gameObject.name = gameObject.name.StartsWith(prefix) ? gameObject.name.Replace(prefix, "") : gameObject.name;
        }


        /// <summary>
        /// 循环删除或者添加物体前缀
        /// </summary>
        /// <param name="objs">通常是Selection.objects</param>
        /// <param name="prefix">前缀</param>
        public static void ChangePrefixLoop(this UnityEngine.Object[] objs, string prefix, bool isAdd = true)
        {
            if (objs.Length == 0) { Debug.Log("没有物体"); return; }
            Array.ForEach(objs, (obj) =>
            {
                if (isAdd)
                    AddPrefix(obj as GameObject, prefix);
                else
                    RemovePrefix(obj as GameObject, prefix);
            });
        }

        /// <summary>
        /// 获取选择的物品的名称
        /// </summary>
        public static string ACGetPrefix(this UnityEngine.Object obj)
        {
            return $"{obj.name}";
        }

        /// <summary>
        /// 字符串去除特殊符号空白等
        /// </summary>
        /// <param name="str"></param>
        /// <param name="keyWord1"></param>
        /// <param name="keyWord2"></param>
        /// <returns></returns>
        public static string ACClearSpecificSymbolOne(this string str, string newString, params string[] strings)
        {
            if (strings == null || strings.Length == 0) return str;
            Array.ForEach(strings, (s) =>
            {
                str = str.Replace(s, newString);
            });
            return str.Trim();
        }

        /// <summary>
        /// 去除指定内容
        /// </summary>
        public static void ACClearSpecificSymbolLoop(this UnityEngine.Object[] objs, string newString, params string[] strings)
        {
            if (objs.Length == 0) { Debug.Log("没有物体"); return; }
            Array.ForEach(objs, (obj) => { obj.name = obj.name.ACClearSpecificSymbolOne(newString, strings); });
        }

        #endregion

        #region Tag
        
        /// <summary>
        /// 反射获取当前Game视图，提示编译完成
        /// </summary>
        /// <param name="tips"></param>
        public static void ShowNotification(string tips)
        {
            var game = EditorWindow.GetWindow(typeof(EditorWindow).Assembly.GetType("UnityEditor.GameView"));
            game?.ShowNotification(new GUIContent($"{tips}"));
        }

        #endregion

        #region Type

        /// <summary>
        /// 类型转换
        /// </summary>
        /// <param name="typeStr"></param>
        /// <returns></returns>
        public static string ChangeType(this string typeStr)
        {
            switch (typeStr)
            {
                case "RectTransform":
                    return "Transform";
                default:
                    return typeStr;
            }
        }

        /// <summary>
        /// 反射命名空间
        /// </summary>
        /// <param name="className">类名</param>
        /// <param name="namespaceName">空间名</param>
        public static Type ReflectClass(this string className, string namespaceName = "UnityEngine.UI")
        {
            Assembly assem = Assembly.Load(namespaceName);
            Type type = assem.GetType($"{namespaceName}.{className}");
            return type;
        }

        /// <summary>
        /// 反射命名空间
        /// </summary>
        /// <param name="className">类名</param>
        /// <param name="namespaceName">空间名</param>
        public static Type ReflectClass<T>(this string className, string namespaceName = "UnityEngine.UI") where T : Component
        {
            Assembly assem = Assembly.Load(namespaceName);
            Type type = assem.GetType($"{namespaceName}.{typeof(T).Name}");
            return type;
        }

        /// <summary>
        /// 反射命名空间
        /// </summary>
        /// <param name="className">类名</param>
        /// <param name="namespaceName">空间名</param>
        public static Type ReflectClass<T>(this UnityEngine.Object obj, string namespaceName = "UnityEngine.UI") where T : Component
        {
            Assembly assem = Assembly.Load(namespaceName);
            Type type = assem.GetType($"{namespaceName}.{typeof(T).Name}");
            return type;
        }

        /// <summary>
        /// 反射命名空间
        /// </summary>
        /// <param name="className">类名</param>
        /// <param name="namespaceName">空间名</param>
        public static Type ReflectClass<T>(this string namespaceName) where T : Component
        {
            Assembly assem = Assembly.Load(namespaceName);
            Type type = assem.GetType($"{namespaceName}.{typeof(T).Name}");
            return type;
        }

        #endregion

        #region UnityComponent

        /// <summary>
        /// 去除组件RayCast Target
        /// </summary>
        public static void ClearRayCastTarget(this UnityEngine.Object[] objs)
        {
            Array.ForEach(objs, (obj) =>
            {
                GameObject go = obj as GameObject;
                if (go.GetComponent<Text>() != null) { go.GetComponent<Text>().raycastTarget = false; }
                if (go.GetComponent<Image>() != null) { go.GetComponent<Image>().raycastTarget = false; }
                if (go.GetComponent<RawImage>() != null) { go.GetComponent<RawImage>().raycastTarget = false; }
                if(go.GetComponent<Text>() == null&& go.GetComponent<Image>() == null&& go.GetComponent<RawImage>() == null)
                    if (EditorUtility.DisplayDialog("消息提示", go.name + "没有找到需要去除的RayCast Target选项", "确定")) { }
            });
        }

        /// <summary>
        /// 设置字体
        /// </summary>
        /// <param name="objs"></param>
        /// <param name="font"></param>
        public static void SetFonts(this UnityEngine.Object[] objs,Font font)
        {
            Array.ForEach(objs, (obj) =>
            {
                GameObject go = obj as GameObject;
                if (go.GetComponent<Text>() != null)
                    go.GetComponent<Text>().font = font;
            });
        }


        /// <summary>
        /// 设置字体
        /// </summary>
        /// <param name="objs"></param>
        /// <param name="font"></param>
        public static void SetFont(this GameObject go, Font font)
        {
            if (go.GetComponent<Text>() != null)
                go.GetComponent<Text>().font = font;
        }

        #endregion
    }
}