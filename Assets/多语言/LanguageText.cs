using UnityEngine;
using UnityEngine.UI;

namespace ACLanguage
{
    public class LanguageText : MonoBehaviour
    {
        public string key;

        private Text m_Text;
        //private TMP_Text m_MeshText;
        private void Awake()
        {
            m_Text = GetComponent<Text>();
            //m_MeshText = GetComponent<TMP_Text>();
        }

        private void OnEnable()
        {
            //如果是放在UI界面中的话开启这段代码
            //OnSwitchLanguage();
            //LanaguageBridge.Instance.OnLanguageChangeEvt += OnSwitchLanguage;
        }
        private void Start()
        {
            //一下代码在LanguageManagerAwake之后执行
            //测试时候用的放在UI界面中的可以删除
            OnSwitchLanguage();
            LanguageBridge.Instance.OnLanguageChangeEvt += OnSwitchLanguage;
        }

        private void OnDisable()
        {
            LanguageBridge.Instance.OnLanguageChangeEvt -= OnSwitchLanguage;
        }

        private void OnSwitchLanguage()
        {
            if (m_Text != null)
                m_Text.text = LanguageBridge.Instance.GetText(key);
            //if (m_MeshText != null)
                //m_MeshText.text = LanaguageBridge.Instance.GetText(key);
        }
    }
}
