using System;
using System.Collections.Generic;
using UnityEditor;
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

    public interface ILanguage
    {
        
    }

    public class LanguageManager
    {
        public static LanguageManager Instance { get; private set; }
        private Dictionary<string, LanguageData> LanguageDataDic { get; set; }
        private List<LanguageComponent> LanguageComponentList { get; set; }
        public ELanguageMode LanguageMode { get; set; } = ELanguageMode.Chinese;
        public Font Font { get; set; }

        public void Init()
        {
            Instance = this;
            LanguageDataDic = new Dictionary<string, LanguageData>();
            LanguageComponentList = new List<LanguageComponent>();
        }

        /// <summary>
        /// 切换语言
        /// </summary>
        public void OnChangeLanguage(ELanguageMode languageMode)
        {
            LanguageMode = languageMode;
            foreach (var languageComponent in LanguageComponentList)
                languageComponent.Change();
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

            Debug.LogError($"多语言未配置：{key}");
            return key;
        }

        /// <summary>
        /// 添加多语言数据
        /// </summary>
        public void AddLanguageDataDicData(LanguageData languageData)
        {
            if (LanguageDataDic.TryAdd(languageData.chinese, languageData)) return;
            Debug.LogError($"当前已包含字符串{languageData.chinese}");
        }

        /// <summary>
        /// 添加组件数据
        /// </summary>
        public void AddLanguageComponent(LanguageComponent languageComponent)
        {
            if (LanguageComponentList.Contains(languageComponent))
            {
                Debug.LogError($"当前内存已有{languageComponent.key}组件");
                return;
            }
            LanguageComponentList.Add(languageComponent);
        }
    }

    
    //CustomEditor(typeof()) 用于关联要自定义的脚本
    //CanEditMultipleObjects 支持多物体同时编辑
    [CustomEditor(typeof(LanguageComponent), true),CanEditMultipleObjects]
    public class LanguageEditor : Editor
    {
        private LanguageComponent languageText;
        private Vector2 scrollPosition;

        private void OnEnable()
        {
            languageText = (LanguageComponent)target;
            languageText.text = languageText.GetComponent<Text>();
        }

        public override void OnInspectorGUI()
        {
            base.DrawDefaultInspector();
            if (Application.isPlaying)return;
            GUILayout.Label("Key", EditorStyles.boldLabel);
            languageText.key = EditorGUILayout.TextArea(languageText.text.text,GUILayout.Height(40));
        }
    }
}