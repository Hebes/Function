using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Assets.多语言
{
    /// <summary>
    /// 语言类型
    /// </summary>
    public enum ELanguageMode
    {
        Chinese = 0,
        English = 1,
    }

    public class LanguageData
    {
        public string chinese;

        public string english;
    }

    public class LanguageManager
    {
        public static LanguageManager Instance { get; set; }
        private Dictionary<string, LanguageData> LanguageDataDic { get; set; }
        public ELanguageMode LanguageMode { get; set; } = ELanguageMode.Chinese;
        public Font Font { get; set; }

        public void Init()
        {
            Instance = this;
            LanguageDataDic = new Dictionary<string, LanguageData>();
        }

        /// <summary>
        /// 切换语言
        /// </summary>
        public void OnChangeLanguage(ELanguageMode languageMode)
        {
            LanguageMode = languageMode;
        }

        /// <summary>
        /// 设置字体
        /// </summary>
        /// <param name="fontValue"></param>
        public void SetFont(Font fontValue)
        {
            Font = fontValue;
        }

        /// <summary>
        /// 获取文字
        /// </summary>
        public string Get(string key)
        {
            if (LanguageDataDic.TryGetValue(key, out var value))
            {
                switch (LanguageMode)
                {
                    default:
                    case ELanguageMode.Chinese:
                        return value.chinese;
                    case ELanguageMode.English:
                        return value.english;
                }
            }

            Debug.Log("多语言未配置：" + key);
            return key;
        }
    }

    public class LanguageComponent : MonoBehaviour
    {
        public Text text;       //组件
        public string key;      //关键字

        private void Awake()
        {
            text = GetComponent<Text>();
            text.text = LanguageManager.Instance.Get(text.text);
        }
    }
}