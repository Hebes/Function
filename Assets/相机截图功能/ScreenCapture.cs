using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace 相机截图功能
{
    /// <summary>
    /// https://www.cnblogs.com/Jimm/p/5951362.html
    /// </summary>
    public class ScreenCapture : MonoBehaviour
    {
        private readonly string directory = $"{Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)}/ScreenCaptureEditor/";

        public Canvas uiCanvas;

        public Camera camera;

        private void Awake()
        {
            Directory.CreateDirectory(directory);
            Application.targetFrameRate = 1;
        }

        // private void Update()
        // {
        //     if (Input.GetKeyDown(KeyCode.A))
        //         CaptureScreenshotWithoutUI();
        // }


        /// <summary>
        /// 这种在帧率很低可以运行。利用RenderTexture设置
        /// </summary>
        public void CaptureScreenshotWithoutUI()
        {
            // 隐藏UI
            //uiCanvas.enabled = false;
            uiCanvas.gameObject.SetActive(false);

            // 创建RenderTexture，并设置为截图相机的目标纹理
            RenderTexture renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
            camera.targetTexture = renderTexture;

            // 渲染截图相机的内容
            camera.Render();

            // 激活目标纹理并读取像素数据
            RenderTexture.active = renderTexture;
            Texture2D screenshot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
            screenshot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            screenshot.Apply();

            // 保存截图
            byte[] bytes = screenshot.EncodeToPNG();
            var filename = System.DateTime.Now.ToString().Replace('/', '-').Replace(':', '_') + ".png";
            System.IO.File.WriteAllBytes($"{directory}/{filename}", bytes);

            // 恢复UI可见性
            //uiCanvas.enabled = true;
            uiCanvas.gameObject.SetActive(true);

            // 释放资源
            RenderTexture.active = null;
            camera.targetTexture = null;
            Destroy(renderTexture);
            Destroy(screenshot);

            Debug.Log($"截图已保存：{filename}");
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
    }
}