using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace 反射内存实力类查看数据
{
    class ClassReflectiveMemoryShow : Attribute
    {
        public ClassReflectiveMemoryShow()
        {
        }
    }

    public class ReflectiveMemory : EditorWindow
    {
        private static ReflectiveMemory _window; //窗口实例对象，必须是一个static

        [MenuItem("Tools/反射内存实力类查看数据")] //定义菜单栏位置
        public static void OpenWindow() //打开窗口函数，必须是static
        {
            _window = GetWindow<ReflectiveMemory>(false, "TutorialWindow", true); //实例化窗口
            _window.Show(); //显示窗口
        }

        /// <summary>
        /// 窗口内显示的GUI面板
        /// </summary>
        private void OnGUI()
        {
            if (GUILayout.Button("获取数据"))
            {
                var assembly = Assembly.Load("Assembly-CSharp");
                Type[] types = assembly.GetTypes();
                foreach (Type type in types)
                {
                    ClassReflectiveMemoryShow obj = type.GetCustomAttribute<ClassReflectiveMemoryShow>();
                    if (obj != null) {
                        Debug.Log(type.Name);
                        PropertyInfo[] properties = type.GetProperties();
                        foreach (PropertyInfo property in properties)
                        {
                            if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
                            {
                                object referenceValue = property.GetValue(obj);
                                Console.WriteLine($"{property.Name} 的值: {referenceValue}");
                            }
                        }

                        // FieldInfo[] fields = type.GetFields();
                        // foreach (FieldInfo field in fields)
                        // {
                        //     if (field.FieldType.IsClass && field.FieldType != typeof(string))
                        //     {
                        //         object referenceValue = field.GetValue(obj);
                        //         Console.WriteLine($"{field.Name} 的值: {referenceValue}");
                        //     }
                        // }
                    }
                }
            }
        }
    }
}