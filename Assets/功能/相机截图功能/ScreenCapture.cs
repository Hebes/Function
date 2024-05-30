using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace 相机截图功能
{
    /// <summary>
    /// https://www.cnblogs.com/Jimm/p/5951362.html
    /// https://www.jianshu.com/p/460803bbd5a9 截屏 ReadPixels was called to read pixels from system frame buffer, while not inside drawing frame.
    /// https://zhuanlan.zhihu.com/p/110050200 Unity3d | 关于RenderTexture/Texture/2d/3d的创建、赋值、保存
    /// https://blog.csdn.net/li1214661543/article/details/105360226  Unity3D 局部截图、全屏截图、带UI截图三种方法
    /// https://blog.csdn.net/li1214661543/article/details/125600054 Unity 四种截图方法（相机视图、无UI、有UI、Game窗口）
    /// https://blog.csdn.net/f_957995490/article/details/109693432 Unity中的截图方法（包括全屏截图、区域截图、Camera截图和摄像头截图）
    /// https://docs.unity.cn/cn/2019.4/ScriptReference/TextureFormat.html TextureFormat
    /// https://docs.unity3d.com/Manual/class-TextureImporterOverride.html
    /// https://docs.unity.cn/cn/2021.2/Manual/class-TextureImporterOverride.html
    /// https://www.cnblogs.com/123ing/p/3704078.html Unity游戏开发之“屏幕截图”
    /// https://gwb.tencent.com/community/detail/120243 Unity3d屏幕截图方法
    /// </summary>
    public class ScreenCapture : MonoBehaviour
    {
        private readonly string directory = $"{Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)}/ScreenCaptureEditor/";

        public Canvas uiCanvas;

        public Camera camera;

        public TextureFormat temp=TextureFormat.RGB24;

        private void Awake()
        {
            Directory.CreateDirectory(directory);
            Application.targetFrameRate = 1;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
                StartCoroutine(CaptureScreenshotWithoutUI());
        }


       

        /// <summary>
        /// 这种在帧率很低可以运行。利用RenderTexture设置
        /// </summary>
        public IEnumerator CaptureScreenshotWithoutUI()
        {
            Profiler.BeginSample("CaptureScreenshotWithoutUI1");
            // 隐藏UI
            //uiCanvas.enabled = false;
            uiCanvas.gameObject.SetActive(false);

            // 创建RenderTexture，并设置为截图相机的目标纹理
            RenderTexture renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
            camera.targetTexture = renderTexture;

            // 渲染截图相机的内容
            camera.Render();
            // 激活目标纹理并读取像素数据  创建一个临时的非压缩纹理，用于截图：
            RenderTexture.active = renderTexture;
            Texture2D screenshot = new Texture2D(Screen.width, Screen.height, temp, false);
            //将屏幕像素读取到临时纹理中
            screenshot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            screenshot.Apply();
            
            // 释放资源
            RenderTexture.active = null;
            camera.targetTexture = null;
            Destroy(renderTexture);
            
            // 恢复UI可见性
            //uiCanvas.enabled = true;
            uiCanvas.gameObject.SetActive(true);
            Profiler.EndSample();
            yield return null;
            Profiler.BeginSample("CaptureScreenshotWithoutUI2");
            // 保存截图
            byte[] bytes = screenshot.EncodeToPNG();
            var filename = System.DateTime.Now.ToString().Replace('/', '-').Replace(':', '_') + ".png";
            System.IO.File.WriteAllBytes($"{directory}/{filename}", bytes);
            
            Destroy(screenshot);

            Debug.Log($"截图已保存：{filename} 大小为：{bytes.Length/1024f}KB");
            Profiler.EndSample();

        }
        
        /// <summary>
        /// 这种在帧率很低可以运行。利用RenderTexture设置
        /// </summary>
        public void CaptureScreenshotWithoutUI1()
        {
            Profiler.BeginSample("CaptureScreenshotWithoutUI");
            // 隐藏UI
            //uiCanvas.enabled = false;
            uiCanvas.gameObject.SetActive(false);

            // 创建RenderTexture，并设置为截图相机的目标纹理
            RenderTexture renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
            camera.targetTexture = renderTexture;

            // 渲染截图相机的内容
            camera.Render();
            // 激活目标纹理并读取像素数据  创建一个临时的非压缩纹理，用于截图：
            RenderTexture.active = renderTexture;
            Texture2D screenshot = new Texture2D(Screen.width, Screen.height, temp, false);
            //将屏幕像素读取到临时纹理中
            screenshot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            screenshot.Apply();
            // 恢复UI可见性
            //uiCanvas.enabled = true;
            uiCanvas.gameObject.SetActive(true);
            
            // 保存截图
            byte[] bytes = screenshot.EncodeToPNG();
            var filename = System.DateTime.Now.ToString().Replace('/', '-').Replace(':', '_') + ".png";
            System.IO.File.WriteAllBytes($"{directory}/{filename}", bytes);

            // 释放资源
            RenderTexture.active = null;
            camera.targetTexture = null;
            Destroy(renderTexture);
            Destroy(screenshot);

            Debug.Log($"截图已保存：{filename} 大小为：{bytes.Length/1024f}KB");
            Profiler.EndSample();

        }
        
        
        /// <summary>
        /// 通过Unity自带的截图(帧率过低会出问题,如果要剔除Ui的话就会有问题)
        /// </summary>
        /// <param name="fileName"></param>
        public void CaptureScreenByUnity()
        {
            var currentTime = System.DateTime.Now;
            var filename = currentTime.ToString().Replace('/', '-').Replace(':', '_') + ".png";
            UnityEngine.ScreenCapture.CaptureScreenshot($"{directory}/{filename}");
        }
        
        /// <summary>
        /// 压缩
        /// </summary>
        private void CaptureScreenshot()
        {
            Texture2D tempTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGBA32, false);
            tempTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            tempTexture.Apply();

            //转换
            Texture2D compressedTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.DXT1, false);
            compressedTexture.SetPixels32(tempTexture.GetPixels32());
            compressedTexture.Apply();

            byte[] screenshotBytes = compressedTexture.EncodeToPNG();
            string fileName = $"screenshot_{System.DateTime.Now:yyyyMMdd_HHmmss}.png";
            System.IO.File.WriteAllBytes(fileName, screenshotBytes);

            Destroy(tempTexture);
            Destroy(compressedTexture);

            Debug.Log($"截图已保存：{fileName}");
        }
    }
}