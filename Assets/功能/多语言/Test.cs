using UnityEngine;
using UnityEngine.UI;

namespace Assets.多语言
{
    public class Test : MonoBehaviour
    {
        public Toggle toggle1; //中文
        public Toggle toggle2; //英文

        private void Awake()
        {
            var languageManager = new LanguageManager();
            languageManager.Init();

            var languageData1 = new LanguageData
            {
                chinese = "你好",
                english = "Hello"
            };
            languageManager.AddLanguageDataDicData(languageData1);
            
            toggle1.onValueChanged.AddListener(Ontoggle1);
            toggle2.onValueChanged.AddListener((bool isOn) =>
            {
                if (!isOn) return;
                languageManager.OnChangeLanguage(ELanguageMode.English);
            });
        }
        
        private void Ontoggle1(bool isOn)
        {
            if (!isOn) return;
            LanguageManager.Instance.OnChangeLanguage(ELanguageMode.Chinese);
        }
    }
}