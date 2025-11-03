// using System;
// using System.Collections.Generic;
// using System.Reflection;
// using System.Text;
// using Tool;
// using UnityEditor;
// using UnityEngine;
// using UnityEngine.UI;
// using Object = UnityEngine.Object; //Object并非C#基础中的Object，而是 UnityEngine.Object
//
// /*--------脚本描述-----------
//
// 描述:
//     修改UIComponent样式
//
//     SerializedProperty  以完全通用的方式编辑对象上的属性（可自动处理撤销），同时还能调整预制件的 UI 样式
//     https://docs.unity.cn/cn/2021.1/ScriptReference/SerializedProperty.html
//     EditorGUILayout  EditorGUI 的自动布局版本
//     https://docs.unity.cn/cn/2021.1/ScriptReference/EditorGUILayout.html
//     SerializedObject     以完全通用的方式编辑 Unity 对象上的可序列化字段。
//     https://docs.unity.cn/cn/2021.1/ScriptReference/SerializedObject.html
//     https://blog.csdn.net/qq_14812585/article/details/107921039 菜单选项
//
// -----------------------*/
// namespace CustomEditorExpansion
// {
//     /// <summary> 自定义ReferenceCollector类在界面中的显示与功能 </summary>
//     [CustomEditor(typeof(UIComponent), true)]
//     public class CustomEditorUIComponent : UnityEditor.Editor
//     {
//         private UIComponent _uiComponent;
//         private const string Prefix = "T_";
//
//         private Type selectComponentType;
//
//         private List<Type> componentList = new List<Type>()
//         {
//             typeof(Button),
//             typeof(Text),
//             typeof(Image),
//             typeof(GameObject),
//             typeof(InputField),
//             typeof(LanguageComponent),
//             typeof(CanvasGroup),
//             typeof(CanvasGroupComponent),
//         };
//
//         private void OnEnable() => _uiComponent = (UIComponent)target;
//
//         public override void OnInspectorGUI()
//         {
//             EditorGUILayout.LabelField("改前缀", EditorStyles.boldLabel);
//             if (GUILayout.Button($"改前缀")) UIComponentTool.ShowUIComponentTool();
//
//             EditorGUILayout.LabelField("按钮", EditorStyles.boldLabel);
//             EditorGUILayout.BeginHorizontal();
//             if (GUILayout.Button($"获取{Prefix}物体")) OoGetKeyGos();
//             if (GUILayout.Button("保存")) Save();
//             if (GUILayout.Button("去除空格")) OnDelTrim();
//             if (GUILayout.Button("删除全部")) _uiComponent.dataList.Clear();
//             EditorGUILayout.EndHorizontal();
//
//             EditorGUILayout.LabelField("获取代码", EditorStyles.boldLabel);
//             if (GUILayout.Button("获取脚本必要代码")) OnGetEssentialCode();
//
//             EditorGUILayout.BeginHorizontal();
//
//             if (GUILayout.Button("变量获取")) GetValue();
//             if (GUILayout.Button("组件获取")) GetCode();
//             EditorGUILayout.EndHorizontal();
//
//
//             EditorGUILayout.Space(10);
//             EditorGUILayout.LabelField("内容", EditorStyles.boldLabel);
//             EditorGUILayout.BeginVertical();
//             for (var i = 0; i < _uiComponent.dataList?.Count; i++)
//             {
//                 EditorGUILayout.BeginHorizontal();
//                 UIData tempData = _uiComponent.dataList[i];
//                 tempData.key = EditorGUILayout.TextField(tempData.key, GUILayout.Width(200));
//                 tempData.@object = EditorGUILayout.ObjectField(tempData.@object, typeof(Object), true);
//                 if (GUILayout.Button("删除")) _uiComponent.dataList.Remove(tempData);
//                 EditorGUILayout.EndHorizontal();
//             }
//
//             EditorGUILayout.EndVertical();
//
//
//             EditorGUILayout.Space(10);
//             UnityEngine.EventType eventType = Event.current.type;
//             //在Inspector 窗口上创建区域，向区域拖拽资源对象，获取到拖拽到区域的对象
//             if (eventType == UnityEngine.EventType.DragUpdated || eventType == UnityEngine.EventType.DragPerform)
//             {
//                 DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
//
//                 if (eventType == UnityEngine.EventType.DragPerform)
//                 {
//                     DragAndDrop.AcceptDrag(); //接受拖动操作。
//                     foreach (var o in DragAndDrop.objectReferences)
//                         _uiComponent.dataList.Add(new UIData(o.name, o));
//                 }
//
//                 Event.current.Use();
//             }
//         }
//
//
//         #region 面板方法调用
//
//         /// <summary>
//         /// 获取关键字开头的物体
//         /// </summary>
//         private void OoGetKeyGos()
//         {
//             //获取物体
//             _uiComponent.dataList.Clear();
//             var gameObjects = new List<GameObject>();
//             GetKeyGoList(_uiComponent.gameObject.transform, Prefix, ref gameObjects);
//             foreach (var go in gameObjects)
//                 _uiComponent.dataList.Add(new UIData(go.name, go));
//             Save();
//         }
//
//         /// <summary>
//         /// 保存
//         /// </summary>
//         private void Save()
//         {
//             foreach (var item in _uiComponent.dataList)
//             {
//                 Undo.RecordObject(item.@object, item.key);
//                 EditorUtility.SetDirty(item.@object);
//             }
//         }
//
//         /// <summary>
//         /// 去除空白
//         /// </summary>
//         private void OnDelTrim()
//         {
//             foreach (var uiData in _uiComponent.dataList)
//                 uiData.key = uiData.@object.name = uiData.@object.name.Trim().Replace(" ", "");
//         }
//
//         /// <summary>
//         /// 获取脚本必要代码
//         /// </summary>
//         private void OnGetEssentialCode()
//         {
//             var sb = new StringBuilder();
//             sb.AppendLine($"UIComponent uiComponent = GetComponent<UIComponent>();");
//             sb.ToString().Copy();
//         }
//
//         /// <summary>
//         /// 获取变量
//         /// </summary>
//         private void GetValue()
//         {
//             var menu = new GenericMenu();
//             foreach (var strValue in componentList)
//                 menu.AddItem(new GUIContent(strValue.Name), selectComponentType == strValue, OnComponentSelected,
//                     strValue);
//             menu.ShowAsContext();
//             return;
//
//             void OnComponentSelected(object value)
//             {
//                 selectComponentType = (Type)value;
//                 var sb = new StringBuilder();
//                 foreach (var item in _uiComponent.dataList)
//                 {
//                     Object obj;
//                     if (selectComponentType.IsSubclassOf(typeof(Component)))
//                         obj = ((GameObject)item.@object).GetComponent(selectComponentType);
//                     else
//                         obj = ((GameObject)item.@object);
//                     var key = value.ToString().Replace("UnityEngine", string.Empty)
//                         .Replace(".", string.Empty).Replace("UI", string.Empty);
//                     if (obj == default) continue;
//                     sb.AppendLine($"public {key} {item.key}{key};");
//                 }
//
//                 if (string.IsNullOrEmpty(sb.ToString())) return;
//                 Debug.LogError(sb.ToString());
//                 sb.ToString().Copy();
//             }
//         }
//
//         /// <summary>
//         /// 获取脚本
//         /// </summary>
//         private void GetCode()
//         {
//             var menu = new GenericMenu();
//             foreach (var strValue in componentList)
//                 menu.AddItem(new GUIContent(strValue.Name), selectComponentType == strValue, OnComponentSelected,
//                     strValue);
//             menu.ShowAsContext();
//             return;
//
//             void OnComponentSelected(object value)
//             {
//                 selectComponentType = (Type)value;
//                 var sb = new StringBuilder();
//                 foreach (var item in _uiComponent.dataList)
//                 {
//                     var goName = item.@object.name;
//                     var key = value.ToString().Replace("UnityEngine", string.Empty).Replace("UI", string.Empty)
//                         .Replace(".", string.Empty);
//                     if (selectComponentType.IsSubclassOf(typeof(Component)))
//                     {
//                         Object obj = ((GameObject)item.@object).GetComponent(selectComponentType);
//                         if (!obj) continue;
//                         sb.AppendLine($"{goName}{key} = uiComponent.GetComponent<{key}>(\"{goName}\");");
//                     }
//                     else
//                     {
//                         sb.AppendLine($"{goName}{key} = uiComponent.Get(\"{goName}\");");
//                     }
//                 }
//
//                 if (string.IsNullOrEmpty(sb.ToString())) return;
//                 Debug.LogError(sb.ToString());
//                 sb.ToString().Copy();
//             }
//         }
//
//         #endregion
//
//         /// <summary>
//         /// 查找物体(PS：包含隐藏、关键词开头,Hierarchy面板)
//         /// </summary>
//         /// <param name="transform">物体</param>
//         /// <param name="keyValue">寻找的关键字</param>
//         /// <param name="goList">返回的列表</param>
//         private void GetKeyGoList(Transform transform, string keyValue, ref List<GameObject> goList)
//         {
//             if (transform.name.StartsWith(keyValue))
//                 goList.Add(transform.gameObject);
//             for (var i = 0; i < transform?.childCount; i++)
//                 GetKeyGoList(transform.GetChild(i), keyValue, ref goList);
//         }
//
//         /// <summary>
//         /// 反射命名空间
//         /// </summary>
//         /// <param name="className"></param>
//         /// <param name="namespaceName"></param>
//         /// <returns></returns>
//         private Type ReflectClass(string className, string namespaceName = "UnityEngine.UI")
//         {
//             var assem = Assembly.Load(namespaceName);
//             var type = assem.GetType($"{namespaceName}.{className}");
//             return type;
//         }
//     }
// }