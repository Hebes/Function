using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Farm2D
{
    internal class RunBat
    {
        private static string _message;

        private static void Run()
        {
            string path = Application.dataPath.Replace("Assets","");
            //// 执行bat脚本
            //RunMyBat("http资源包测试启动.bat", path);

            string cmd = "/c http资源包测试启动.bat /path:\"{0}\" /closeonend 2";
            //var path = Application.dataPath + "/../";
            cmd = string.Format(cmd, path);
            //UnityEngine.Debug.LogError(cmd);
            ProcessStartInfo proc = new ProcessStartInfo("cmd.exe", cmd);
            proc.WindowStyle = ProcessWindowStyle.Normal;
            Process.Start(proc);
        }
        
        private static void Run1()
        {
            string folderPath = "D:/Preject/client/GameTable";
            string batFilePath = "D:/Preject/client/GameTable/Generated.bat";


            // 设置要执行的.bat文件和工作目录
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = batFilePath;
            startInfo.WorkingDirectory = folderPath;

            // 创建进程对象
            Process process = new Process();
            process.StartInfo = startInfo;
            // // 设置进程文件路径
            // process.StartInfo.FileName = "cmd.exe";
            // // 设置传递给 cmd.exe 的参数，包括批处理文件路径和执行参数（如果有）
            // process.StartInfo.Arguments = $"/C {batchFilePath}";

            // 隐藏命令行窗口
            //process.StartInfo.CreateNoWindow = true;
            //process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

            // 启动进程
            process.Start();

            // 等待进程完成执行
            process.WaitForExit();

            // 获取批处理文件的退出码
            int exitCode = process.ExitCode;

            // 关闭进程
            process.Close();

            // 检查退出码以确定批处理文件的执行结果
            if (exitCode == 0)
                _message = "批处理文件执行成功.";
            else
                _message = "批处理文件执行失败。退出代码: " + exitCode;
        }
    }
}
