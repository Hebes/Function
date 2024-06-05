using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace 宏控制面板
{
    /// <summary>
    /// https://blog.csdn.net/xinjay1992/article/details/108112010
    /// </summary>
    public class MacroHelper : EditorWindow
    {
        [MenuItem("Tools/MacroHelper")]
        static void Open()
        {
            EditorWindow.GetWindow<MacroHelper>("MacroHelper");
        }

        private Macro macro;
        private List<BuildTargetGroupItem> supportItems;
        private Vector2 scroll;

        private void OnEnable()
        {
            macro = LoadOrCreate();
            supportItems = Macro.GetSupportGroup();
        }

        private void OnGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("宏名", EditorStyles.boldLabel, GUILayout.Width(100));
            foreach (var item in supportItems)
            {
                GUILayout.Label(item.smallIcon, EditorStyles.boldLabel, GUILayout.Width(50));
                foreach (var macroitem in macro.MacroItems)
                {
                    if (!macroitem.SwitchItems.Exists(sitem => sitem.targetGroup == item.targetGroup))
                    {
                        macroitem.SwitchItems.Add(new SwitchItem()
                        {
                            isOn = false,
                            targetGroup = item.targetGroup
                        });
                        EditorUtility.SetDirty(macro);
                    }
                }
            }

            GUILayout.Label("备注", EditorStyles.boldLabel, GUILayout.Width(200));
            GUILayout.EndHorizontal();
            scroll = GUILayout.BeginScrollView(scroll);
            GUILayout.BeginVertical(EditorStyles.textArea);
            for (var index = 0; index < macro.MacroItems.Count; index++)
            {
                var macroitem = macro.MacroItems[index];
                GUILayout.BeginHorizontal();
                var name = GUILayout.TextField(macroitem.MacroName, GUILayout.Width(100));
                if (name != macroitem.MacroName && macro.CheckName(name))
                {
                    if (macro.ReplaceMacro(macroitem.MacroName, name))
                        EditorUtility.SetDirty(macro);
                }

                foreach (var sitem in macroitem.SwitchItems)
                {
                    foreach (var item in supportItems)
                    {
                        if (item.targetGroup == sitem.targetGroup)
                        {
                            var ison = GUILayout.Toggle(sitem.isOn, sitem.isOn ? "关闭" : "开启", GUILayout.Width(50));
                            if (ison != sitem.isOn)
                            {
                                sitem.isOn = MacroItem.SwitchMicro(sitem.targetGroup, ison, macroitem.MacroName);
                                EditorUtility.SetDirty(macro);
                            }
                        }
                    }
                }

                var description = GUILayout.TextField(macroitem.Description, GUILayout.Width(200));
                if (description != macroitem.Description)
                {
                    macroitem.Description = description;
                    EditorUtility.SetDirty(macro);
                }

                if (GUILayout.Button("-", GUILayout.Width(50)) && EditorUtility.DisplayDialog("Warning", "确认删除？", "确认", "取消"))
                {
                    if (macro.RemoveMacro(macroitem.MacroName))
                        EditorUtility.SetDirty(macro);
                }

                GUILayout.EndHorizontal();
            }

            GUILayout.EndVertical();
            GUILayout.EndScrollView();
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("+"))
            {
                var name = macro.GetRandomName();
                if (macro.AddMacro(name))
                {
                    EditorUtility.SetDirty(macro);
                }
            }

            if (GUILayout.Button("-") && EditorUtility.DisplayDialog("Warning", "确认删除？", "确认", "取消"))
            {
                var index = macro.MacroItems.Count - 1;
                var name = macro.MacroItems[index].MacroName;
                if (macro.RemoveMacro(name))
                {
                    EditorUtility.SetDirty(macro);
                }
            }

            GUILayout.EndHorizontal();
            GUILayout.Space(10);
        }

        private Macro LoadOrCreate()
        {
            if (!File.Exists(Macro.path))
            {
                var asset = ScriptableObject.CreateInstance<Macro>();
                AssetDatabase.CreateAsset(asset, Macro.path);
            }

            var macro = AssetDatabase.LoadAssetAtPath<Macro>(Macro.path);
            macro.Init();
            return macro;
        }
    }
}