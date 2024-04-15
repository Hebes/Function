// using UnityEditor;
// using System.IO;
// using UnityEngine;
// using UnityEditor.Build.Player;
//
// namespace 热重载
// {
//     /// <summary>
//     /// https://blog.csdn.net/qq_30163099/article/details/126263499
//     /// https://zhuanlan.zhihu.com/p/585595271
//     /// https://blog.csdn.net/m0_46712616/article/details/121732995
//     /// </summary>
//     public class CompileDLLHelper
//     {
//         [MenuItem("HTools/CompileDlls")]
//         public static void CompileDll()
//         {
//
//             var tempOutputPath = $"{Application.dataPath}/../Dlls";
//             Directory.CreateDirectory(tempOutputPath);
//
//             ScriptCompilationSettings scriptCompilationSettings = new ScriptCompilationSettings();
//             scriptCompilationSettings.group = BuildPipeline.GetBuildTargetGroup(BuildTarget.StandaloneWindows64);
//             scriptCompilationSettings.target = BuildTarget.StandaloneWindows64;
//
//             PlayerBuildInterface.CompilePlayerScripts(scriptCompilationSettings, tempOutputPath);
//         }
//     }
// }