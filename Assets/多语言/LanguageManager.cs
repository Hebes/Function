using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ACLanguage
{
    /// <summary>
    /// 语言类型
    /// </summary>
    public enum ELanguageMode
    {
        Chinese = 0,
        English = 1,
    }

    /// <summary>
    /// 多语言控制脚本
    /// </summary>
    public class LanguageManager : MonoBehaviour
    {
        public Toggle toggle1;//中文
        public Toggle toggle2;//英文
        private Dictionary<string, string> LanguageDic { get; set; }

        public ELanguageMode LanguageMode { get; set; }

        private Dictionary<string, string> EnglishLanguageDic { get; set; } = new Dictionary<string, string>()
        {
            {"1","Hello" },
        };
        private Dictionary<string, string> ChineseLanguageDic { get; set; } = new Dictionary<string, string>()
        {
            {"1","你好" },
        };

        private void Awake()
        {
            new LanguageBridge();
            
            //可以用PlayerPrefs存储语言类型,这里直接首次用的英文
            LanguageDic = new Dictionary<string, string>();

            OnChangeLanuage(ELanguageMode.Chinese);//初始化第一次中文

            toggle1.onValueChanged.AddListener(Ontoggle1);
            toggle2.onValueChanged.AddListener((bool isOn) =>
            {
                if (isOn)
                {
                    Debug.Log("切换英文");
                    OnChangeLanuage(ELanguageMode.English);
                }
            });
        }

        private void Ontoggle1(bool isOn)
        {
            if (isOn)
            {
                Debug.Log("切换中文");
                OnChangeLanuage(ELanguageMode.Chinese);
            }
        }

        /// <summary>
        /// 添加多语言数据
        /// </summary>
        public void OnAddLanguageData(ELanguageMode languageMode)
        {
            //TODO 这里可以添加多语言数据.可以从读取数据加载,这里直接用的测试
            switch (languageMode)
            {
                case ELanguageMode.Chinese:
                    LanguageDic = ChineseLanguageDic;
                    break;
                case ELanguageMode.English:
                    LanguageDic = EnglishLanguageDic;
                    break;
                default:
                    LanguageDic = ChineseLanguageDic;
                    break;
            }
        }

        /// <summary>
        /// 切换语言
        /// </summary>
        public void OnChangeLanuage(ELanguageMode languageMode)
        {
            //添加数据
            OnAddLanguageData(languageMode);

            //保存数据
            PlayerPrefs.SetInt("Language", (int)languageMode);
            LanguageMode = languageMode;
            //调用中间LanaguageBridge切换语言
            LanguageBridge.Instance.languageTextKeyDic = LanguageDic;
            LanguageBridge.Instance.OnLanguageChange();
        }
    }
}
