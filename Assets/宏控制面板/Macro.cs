using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace 宏控制面板
{
    public class Macro : ScriptableObject
    {
        //unity内置宏命令，不可以重复设定
        private static List<string> BuiltMacro = new List<string>()
        {
            "UNITY_EDITOR", "UNITY_EDITOR_WIN", "UNITY_EDITOR_OSX", "UNITY_EDITOR_LINUX", "UNITY_STANDALONE_OSX",
            "UNITY_STANDALONE_WIN", "UNITY_STANDALONE_LINUX", "UNITY_STANDALONE", "UNITY_WII", "UNITY_IOS", "UNITY_IPHONE",
            "UNITY_ANDROID", "UNITY_PS4", "UNITY_XBOXONE", "UNITY_LUMIN", "UNITY_TIZEN", "UNITY_TVOS", "UNITY_WSA", "UNITY_WSA_10_0",
            "UNITY_WINRT", "UNITY_WINRT_10_0", "UNITY_WEBGL", "UNITY_FACEBOOK", "UNITY_ADS", "UNITY_ANALYTICS", "UNITY_ASSERTIONS", "UNITY_64"
        };

        public const string path = "Assets/macro.asset";
        public List<MacroItem> MacroItems = new List<MacroItem>();

        public void Init()
        {
            var validPlatforms = GetSupportGroup();
            var list = new List<string>();
            foreach (var platform in validPlatforms)
            {
                var group = platform.targetGroup;
                var defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(group).Split(';');
                list.AddRange(defines);
            }

            list = list.Distinct().ToList();
            foreach (var item in MacroItems)
            {
                if (list.Contains(item.MacroName))
                {
                    list.Remove(item.MacroName);
                }
            }

            foreach (var macro in list)
            {
                AddMacro(macro);
            }
        }

        /// <summary>
        /// 反射获取当前支持平台
        /// </summary>
        /// <returns></returns>
        public static List<BuildTargetGroupItem> GetSupportGroup()
        {
            var result = new List<BuildTargetGroupItem>();
            var buildPlatforms = typeof(Editor).Assembly.GetType("UnityEditor.Build.BuildPlatforms");
            var instance = buildPlatforms?.GetProperty("instance", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)?.GetValue(null);
            var temp = buildPlatforms?.InvokeMember("GetValidPlatforms", System.Reflection.BindingFlags.InvokeMethod, null, instance, null);
            var items = temp as IEnumerable;
            var buildPlatform = typeof(Editor).Assembly.GetType("UnityEditor.Build.BuildPlatform");
            foreach (var item in items)
            {
                var name = buildPlatform?.GetField("name", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public)?.GetValue(item) as string;
                var targetGroup = (BuildTargetGroup)buildPlatform?.GetField("targetGroup", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public)?.GetValue(item);
                var title1 = buildPlatform.GetField("title", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
                var title = buildPlatform.GetProperty("title", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public)?.GetValue(item) as GUIContent;
                var smallIcon = buildPlatform.GetProperty("smallIcon", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public)?.GetValue(item) as Texture2D;
                result.Add(new BuildTargetGroupItem { name = name, targetGroup = targetGroup, title = title, smallIcon = smallIcon });
            }

            return result;
        }

        /// <summary>
        /// 添加宏命令
        /// </summary>
        /// <param name="macroname"></param>
        /// <returns></returns>
        public bool AddMacro(string macroname)
        {
            if (string.IsNullOrEmpty(macroname.Trim()))
            {
                Debug.Log("宏名不能为空！");
                return false;
            }

            if (!MacroItems.Exists(mitem => mitem.MacroName == macroname))
            {
                var item = new MacroItem { MacroName = macroname };
                item.Init();
                MacroItems.Add(item);
                return true;
            }
            else
            {
                Debug.LogWarning("该宏已存在！");
                return false;
            }
        }

        /// <summary>
        /// 移除宏命令
        /// </summary>
        /// <param name="macroname"></param>
        /// <returns></returns>
        public bool RemoveMacro(string macroname)
        {
            for (var index = 0; index < MacroItems.Count; index++)
            {
                var item = MacroItems[index];
                if (item.MacroName == macroname)
                {
                    foreach (var group in item.SwitchItems)
                    {
                        if (group.isOn)
                        {
                            MacroItem.SwitchMicro(group.targetGroup, false, macroname);
                        }
                    }

                    MacroItems.RemoveAt(index);
                    return true;
                }
            }

            Debug.LogWarning("该宏不存在！");
            return false;
        }

        /// <summary>
        /// 替换宏命令
        /// </summary>
        /// <param name="srcMacro"></param>
        /// <param name="targetMacro"></param>
        /// <returns></returns>
        public bool ReplaceMacro(string srcMacro, string targetMacro)
        {
            if (!CheckName(targetMacro))
                return false;
            foreach (var item in MacroItems)
            {
                var srcname = item.MacroName;
                if (srcname == srcMacro)
                {
                    item.MacroName = targetMacro;
                    foreach (var sitem in item.SwitchItems)
                    {
                        if (sitem.isOn)
                        {
                            MacroItem.SwitchMicro(sitem.targetGroup, false, srcname);
                            MacroItem.SwitchMicro(sitem.targetGroup, true, targetMacro);
                        }
                    }

                    break;
                }
            }

            return true;
        }

        /// <summary>
        /// 检测宏命令是否合法
        /// </summary>
        /// <param name="macroName"></param>
        /// <returns></returns>
        public bool CheckName(string macroName)
        {
            if (string.IsNullOrEmpty(macroName))
            {
                Debug.LogError("宏名不能为空！");
                return false;
            }

            if (BuiltMacro.Contains(macroName))
            {
                Debug.LogError("内置宏不可重复定义！");
                return false;
            }

            foreach (var item in MacroItems)
            {
                if (item.MacroName == macroName)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 随机获取宏命令
        /// </summary>
        /// <returns></returns>
        public string GetRandomName()
        {
            var startIndex = MacroItems.Count;
            while (true)
            {
                var name = $"MACRO{startIndex++}";
                if (CheckName(name))
                {
                    return name;
                }
            }
        }
    }

    [System.Serializable]
    public class MacroItem
    {
        public List<SwitchItem> SwitchItems = new List<SwitchItem>();
        public string MacroName;
        public string Description;

        public void Init()
        {
            var groups = System.Enum.GetValues(typeof(BuildTargetGroup)) as BuildTargetGroup[];
            var validPlatforms = Macro.GetSupportGroup();
            foreach (var group in groups)
            {
                var isOn = false;
                if (validPlatforms.Exists(item => item.targetGroup == group))
                {
                    var defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(group).Split(';').ToList();
                    isOn = defines.Contains(MacroName);
                }

                var switchitem = new SwitchItem()
                {
                    targetGroup = group,
                    isOn = isOn
                };
                SwitchItems.Add(switchitem);
            }
        }

        /// <summary>
        /// 针对指定平台开启或关闭指定宏命令
        /// </summary>
        /// <param name="targetGroup"></param>
        /// <param name="isOn"></param>
        /// <param name="macroName"></param>
        /// <returns></returns>
        public static bool SwitchMicro(BuildTargetGroup targetGroup, bool isOn, string macroName)
        {
            var macro = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);
            var defines = macro.Split(';').Distinct().ToList();
            defines.Sort();
            if (isOn)
                defines.Add(macroName);
            else
                defines.Remove(macroName);
            macro = "";
            defines.ForEach(item => macro += $"{item};");
            PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, macro);
            return isOn;
        }
    }

    [System.Serializable]
    public class SwitchItem
    {
        public BuildTargetGroup targetGroup;
        public bool isOn;
    }

    public class BuildTargetGroupItem
    {
        public string name;
        public BuildTargetGroup targetGroup;
        public GUIContent title;
        public Texture2D smallIcon;
    }
}