using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 多语言工具
/// </summary>
[CustomEditor(typeof(LanguageComponent), true)]
public class CustomEditorLanguage : UnityEditor.Editor
{
    private LanguageComponent _languageText;

    private void OnEnable()
    {
        _languageText = (LanguageComponent)target;
        _languageText.text = _languageText.GetComponent<Text>();
        //_languageText.key = _languageText.GetComponent<Text>().text;
    }

    public override void OnInspectorGUI()
    {
        if (Application.isPlaying) return;
        EditorGUILayout.LabelField("移除组件需要先禁用Text", EditorStyles.boldLabel);
        //base.DrawDefaultInspector();
        EditorGUILayout.ObjectField("ObjectField", _languageText.text, typeof(Object), true);
        //_languageText.IsAddManager = EditorGUILayout.Toggle("是否启用多语言组件", _languageText.IsAddManager);
        GUILayout.Label("Text", EditorStyles.boldLabel);
        //_languageText.key = EditorGUILayout.TextArea(string.IsNullOrEmpty(_languageText.text.text) ? string.Empty : _languageText.text.text);
    }
}