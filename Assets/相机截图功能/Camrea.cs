using System;
using UnityEngine;

namespace 相机截图功能
{
    public class Camrea : MonoBehaviour
    {
        private readonly string directory = $"{Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)}/ScreenCaptureEditor/";
        public Canvas uiCanvas;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
                isScreenShot = true;
        }

        bool isScreenShot = false;

        /// <summary>
        /// 这种截图再帧率很低也可以运行
        /// </summary>
        private void OnPostRender()
        {
            if (isScreenShot)
            {
                uiCanvas.enabled = false;
                Rect rect = new Rect(0, 0, Screen.width, Screen.height);
                // 先创建一个的空纹理，大小可根据实现需要来设置
                Texture2D screenShot = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);

                // 读取屏幕像素信息并存储为纹理数据，
                screenShot.ReadPixels(rect, 0, 0);
                screenShot.Apply();

                // 然后将这些纹理数据，成一个png图片文件
                byte[] bytes = screenShot.EncodeToPNG();
                print("isScreenShot " + bytes.Length);

                // 保存截图
                var filename = System.DateTime.Now.ToString().Replace('/', '-').Replace(':', '_') + ".png";
                System.IO.File.WriteAllBytes($"{directory}/{filename}", bytes);
                isScreenShot = false;
                uiCanvas.enabled = true;
                Destroy(screenShot);
            }
        }
    }
}