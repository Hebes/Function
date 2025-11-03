// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using zhaorh.UI;
//
// // <summary>
// // @Author: zrh
// // @Date: 2023,01,06,10:12
// // @Description: 自定义布局
// // </summary>
//
// namespace Game.UI
// {
//     /// <summary>
//     /// 布局锚点
//     /// </summary>
//     public enum LayoutAnchor
//     {
//         Up,
//         // 水平
//         MiddleHorizontal,
//         Down,
//         Right,
//         Left,
//         // 垂直
//         MiddleVertical
//     }
//
//     /// <summary>
//     /// 垂直布局
//     /// </summary>
//     public class Layout : MonoBehaviour
//     {
//         // 间距
//         public float space = 2;
//         // 锚点
//         public LayoutAnchor anchor;
//
//         private RectTransform _rect;
//
//         // 矩形
//         private RectTransform rect
//         {
//             get
//             {
//                 if (_rect == null)
//                 {
//                     _rect = GetComponent<RectTransform>();
//                 }
//
//                 return _rect;
//             }
//         }
//
//         void Awake()
//         {
//             _rect = GetComponent<RectTransform>();
//         }
//
//         /// <summary>
//         /// 显示某个节点
//         /// </summary>
//         /// <param name="arg"></param>
//         public void Show(GameObject arg)
//         {
//             arg.gameObject.SetActive(true);
//             Sort();
//         }
//         // 隐藏某个节点
//         public void Hide(GameObject arg)
//         {
//             arg.gameObject.SetActive(false);
//             Sort();
//         }
//
//         /// <summary>
//         ///  排序
//         /// </summary>
//         public void Sort()
//         {
//             switch (anchor)
//             {
//                 case LayoutAnchor.Up:
//                     UpSort();
//                     break;
//                 case LayoutAnchor.MiddleHorizontal:
//                     MiddleSort();
//                     break;
//                 case LayoutAnchor.Down:
//                     DownSort();
//                     break;
//                 case LayoutAnchor.Right:
//                     RightSort();
//                     break;
//                 case LayoutAnchor.MiddleVertical:
//                     MiddleVerticalSort();
//                     break;
//                 default: // left
//                     LeftSort();
//                     break;
//             }
//         }
//
//         /// <summary>
//         /// 从上往下
//         /// </summary>
//         void UpSort()
//         {
//             var y = 0f;
//             for (var i = 0; i < rect.childCount; ++i)
//             {
//                 var child = rect.GetChildComponent<RectTransform>(i);
//                 if (!child.gameObject.activeSelf) continue;
//                 child.anchoredPosition = new Vector2(0, -y);
//                 y += child.sizeDelta.y + space;
//             }
//         }
//
//         /// <summary>
//         /// 从中间开始 
//         /// </summary>
//         void MiddleSort()
//         {
//             var activeObjects = new List<RectTransform>();
//             for (var i = 0; i < rect.childCount; ++i)
//             {
//                 var child = rect.GetChildComponent<RectTransform>(i);
//                 if (child.gameObject.activeSelf)
//                     activeObjects.Add(child);
//             }
//             bool isOdd = activeObjects.Count % 2 != 0; //奇数
//             var harf = activeObjects.Count / 2;
//
//             for (var i = harf; i < activeObjects.Count; ++i) // 下半部分
//             {
//                 var child = activeObjects[i];
//                 var x = isOdd ? (i - harf) * (child.sizeDelta.x + space) : (space + child.sizeDelta.x) / 2 + (i - harf) * (child.sizeDelta.x + space);
//                 child.anchoredPosition = new Vector2(x, 0);
//             }
//             for (var i = 0; i < harf; ++i) // 上半部分
//             {
//                 var child = activeObjects[i];
//                 var x = activeObjects[(activeObjects.Count - 1 - i)].anchoredPosition.x;
//                 child.anchoredPosition = new Vector2(-x, 0);
//             }
//         }
//
//         /// <summary>
//         /// 从中间开始 (垂直)
//         /// </summary>
//         void MiddleVerticalSort()
//         {
//             var activeObjects = new List<RectTransform>();
//             for (var i = 0; i < rect.childCount; ++i)
//             {
//                 var child = rect.GetChildComponent<RectTransform>(i);
//                 if (child.gameObject.activeSelf)
//                     activeObjects.Add(child);
//             }
//             bool isOdd = activeObjects.Count % 2 != 0; //奇数
//             var harf = activeObjects.Count / 2;
//
//             for (var i = 0; i < harf; ++i) // 上半部分
//             {
//                 var child = activeObjects[i];
//                 var y = activeObjects[(activeObjects.Count - 1 - i)].anchoredPosition.y;
//                 child.anchoredPosition = new Vector2(0, -y);
//                
//             }
//
//             for (var i = harf; i < activeObjects.Count; ++i) // 下半部分
//             {
//                 var child = activeObjects[i];
//                 var y = isOdd ? (i - harf) * (child.sizeDelta.y + space) : (space + child.sizeDelta.y) / 2 + (i - harf) * (child.sizeDelta.y + space);
//                 child.anchoredPosition = new Vector2(0, y);
//             }
//         }
//
//         /// <summary>
//         ///  从下往上排序
//         /// </summary>
//         void DownSort()
//         {
//             var y = 0f;
//             for (var i = 0; i < rect.childCount; ++i)
//             {
//                 var child = rect.GetChildComponent<RectTransform>(i);
//                 if (!child.gameObject.activeSelf) continue;
//                 child.anchoredPosition = new Vector2(0, y);
//                 y += child.sizeDelta.y + space;
//             }
//         }
//
//         /// <summary>
//         /// 往右开始排序
//         /// </summary>
//         void RightSort()
//         {
//             var x = 0f;
//             for (var i = 0; i < rect.childCount; ++i)
//             {
//                 var child = rect.GetChildComponent<RectTransform>(i);
//                 if (!child.gameObject.activeSelf) continue;
//                 child.anchoredPosition = new Vector2(x, 0);
//                 x += child.sizeDelta.x + space;
//             }
//         }
//
//         /// <summary>
//         /// 往左开始排序
//         /// </summary>
//         void LeftSort()
//         {
//             var x = 0f;
//             for (var i = 0; i < rect.childCount; ++i)
//             {
//                 var child = rect.GetChildComponent<RectTransform>(i);
//                 if (!child.gameObject.activeSelf) continue;
//                 child.anchoredPosition = new Vector2(-x, 0);
//                 x += child.sizeDelta.x + space;
//             }
//         }
//
// #if UNITY_EDITOR
//         // 效果预览
//         public void Preview()
//         {
//             Awake();
//             Sort();
//         }
// #endif
//     }
// }