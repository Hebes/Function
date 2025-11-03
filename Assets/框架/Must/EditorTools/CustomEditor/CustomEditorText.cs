using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace CustomEditorExpansion
{
    [CustomEditor(typeof(Text), true)]
    public class CustomEditorText : UnityEditor.Editor
    {
        public LanguageComponent value;
        //private bool IsAdd;

        private void OnEnable()
        {
            value = target.GetComponent<LanguageComponent>();
            if (value) return;
            if (!((Text)target).enabled) return;
            target.AddComponent<LanguageComponent>();
        }

        public override void OnInspectorGUI()
        {
            base.DrawDefaultInspector();
            //IsAdd = EditorGUILayout.Toggle("启用添加多语言组件", IsAdd);
            // if (!IsAdd) DestroyImmediate(value);
            // if (GUI.changed)
            // {
            //     EditorUtility.SetDirty(value);
            //     AssetDatabase.SaveAssets();
            // }
        }
    }
}