using System;
using System.Collections.Generic;
using UnityEngine;

namespace ACLanguage
{
    /// <summary>
    /// 语言的中间组件
    /// </summary>
    public class LanguageBridge
    {
        public static LanguageBridge Instance { get; private set; }
        public Dictionary<string, string> languageTextKeyDic; //语言字典
        public event Action OnLanguageChangeEvt; //回调事件
        public Font Font { get; set; } //字体

        public LanguageBridge()
        {
            Instance = this;
        }

        /// <summary>
        /// 切换语言
        /// </summary>
        public void OnLanguageChange()
        {
            OnLanguageChangeEvt?.Invoke();
        }

        /// <summary>
        /// 获取文字
        /// </summary>
        public string GetText(string key)
        {
            if (languageTextKeyDic.ContainsKey(key))
                return languageTextKeyDic[key];
            Debug.Log("多语言未配置：" + key);
            return key;
        }
    }
}