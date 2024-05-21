using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.多语言
{
    public class Test : MonoBehaviour
    {
        public Toggle toggle1; //中文
        public Toggle toggle2; //英文

        private Dictionary<string, string> EnglishLanguageDic { get; set; } = new Dictionary<string, string>()
        {
            { "1", "Hello" },
        };

        private Dictionary<string, string> ChineseLanguageDic { get; set; } = new Dictionary<string, string>()
        {
            { "1", "你好" },
        };

        private void Awake()
        {
            // var languageManager = new LanguageManager();
            // languageManager.Init();
            //
            // toggle1.onValueChanged.AddListener(Ontoggle1);
            // toggle2.onValueChanged.AddListener((bool isOn) =>
            // {
            //     if (isOn)
            //     {
            //         Debug.Log("切换英文");
            //         OnChangeLanuage(ELanguageMode.English);
            //     }
            // });
        }
        
        private void Ontoggle1(bool isOn)
        {
            // if (isOn)
            // {
            //     Debug.Log("切换中文");
            //     OnChangeLanuage(ELanguageMode.Chinese);
            // }
        }
    }
}