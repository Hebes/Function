using System;
using System.IO;
using UnityEngine;

namespace 文件夹操作
{
    public class DirectoryOperation : MonoBehaviour
    {
        private void Awake()
        {
            //Directory.Delete(strpath); //如果目录内的内容不为空时会报错
            //Directory.Delete(strPath,true); //第二个参数代表如果内容不为空是否也要删除，这样就不会报错了
        }

        /// <summary>
        /// 功能：删除指定文件夹下面的文件
        /// </summary>
        /// <returns></returns>
        private int SetDeleteTex()
        {
            // string str_pictureFileName = GameObject.FindWithTag("ScriptsHold").GetComponent<MyButtonOnClick_myWrite>().canvas_takePicture
            //     .GetComponent<MyCanvas_takePicture>().str_pictureFileName;
            // string str_filePath = Application.streamingAssetsPath + "/" + str_pictureFileName;
            //
            // //获取指定路径下面的所有资源文件  
            // if (Directory.Exists(str_filePath))
            // {
            //     DirectoryInfo direction = new DirectoryInfo(str_filePath);
            //     FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);
            //     for (int i = 0; i < files.Length; ++i)
            //     {
            //         if (files[i].Name.Contains(".jpg") || files[i].Name.Contains(".png") || files[i].Name.Contains(".gif"))
            //         {
            //             File.Delete(str_filePath + "/" + files[i].Name);
            //         }
            //     }
            // }

            return 0;
        }
    }
}