using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.PackageManager.Requests;
using UnityEditor.PackageManager;
using UnityEngine;

namespace ToolEditor
{
    public class UIInstall : EditorWindow
    {
        public AddRequest request { get; private set; }

        [MenuItem("Tools/安装packageManager")]
        public static void ShowConfigToolUI()
        {
            if (!EditorWindow.HasOpenInstances<UIInstall>())
                GetWindow(typeof(UIInstall), false, "生成配置文件").Show();
            else
                GetWindow(typeof(UIInstall)).Close();
        }

        private void OnGUI()
        {
            EditorGUILayout.TextField("https://github.com/Cysharp/UniTask.git?path=src/UniTask/Assets/Plugins/UniTask");

            if (GUILayout.Button("安装UniTask"))
            {
                string gitURL = "https://github.com/Cysharp/UniTask.git?path=src/UniTask/Assets/Plugins/UniTask";
                request = Client.Add(gitURL);
                EditorApplication.update += Progress;
            }

            EditorGUILayout.TextField("package.openupm.com");
            EditorGUILayout.TextField("https://package.openupm.com");
            EditorGUILayout.TextField("com.tuyoogame.yooasset");
        }

         void Progress()
        {
            if (request.IsCompleted)
            {
                if (request.Status == StatusCode.Success)
                    Debug.Log($"获取包名成功Package name: {request.Result.name}");
                else if (request.Status >= StatusCode.Failure)
                    Debug.Log(request.Error.message);
                EditorApplication.update -= Progress;
            }
        }
    }
}
