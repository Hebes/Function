using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// <summary>
// @Author: zrh
// @Date: 2022,12,06,16:52
// @Description:
// </summary>

namespace zhaorh
{
    public partial class Tools
    {
        /// <summary>
        /// 获取unity场景中的ui画布组件
        /// </summary>
        /// <returns>The canvas game object.</returns>
        public static GameObject getCanvasGameObject ()
        {
            GameObject canvasGo = GameObject.Find ("Canvas");
            if (canvasGo == null) {
                canvasGo = GameObject.Find ("EasyTouchControlsCanvas");
            }
            if (canvasGo == null) {
                Canvas cCanvas = GameObject.FindObjectOfType<Canvas> ();
                if (cCanvas != null) {
                    canvasGo = cCanvas.gameObject;
                }
            }
            if (canvasGo == null) {
                canvasGo = new GameObject ("Canvas");
                canvasGo.AddComponent<Canvas> ();
            }
            return canvasGo;
        }
        public static bool setText (string gameObjectName, string textValue)
        {
            GameObject go =	GameObject.Find (gameObjectName);
            if (go != null) {
                Text text =	go.GetComponent<Text> ();
                if (text != null) {
                    text.text = textValue;
                    return true;
                }
            }
            return false;
        }
        public static int GetLineCount(Text text, string content, float width)
        {
            text.cachedTextGenerator.Populate(content, text.GetGenerationSettings(new Vector2(width, 600)));

            return text.cachedTextGenerator.lineCount;
        }   
    }
}