using System.Collections.Generic;
using ACLanguage;
using UnityEngine;

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
    
    public class LanguageManager
    {
        public static LanguageManager Instance{ get; set; }
        List<(string,string)> LanguageDataList{ get; set; }
        public ELanguageMode LanguageMode { get; set; }

        private readonly string saveKey = "Language";
        public Font Font { get; set; } //字体

        public void Init()
        {
            Instance = this;
            //可以用PlayerPrefs存储语言类型,这里直接首次用的英文
            LanguageDataList = new List<(string, string)>();
        }
        
        /// <summary>
        /// 切换语言
        /// </summary>
        public void OnChangeLanguage(ELanguageMode languageMode,List<(string,string)> languageDataListValue)
        {
            LanguageMode = languageMode;
            LanguageDataList = languageDataListValue;
            //保存数据
            PlayerPrefs.SetInt(saveKey, (int)languageMode);
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
        public string GetText(string key)
        {
            // LanguageDataList.con
            // if (languageTextKeyDic.ContainsKey(key))
            //     return languageTextKeyDic[key];
            // Debug.Log("多语言未配置：" + key);
            // return key;
            return null;
        }
    }
}