// using System;
// using System.Collections;
// using System.Collections.Generic;
// using System.Resources;
// using UnityEngine;
// using UnityEngine.UI;
// using zhaorh.UI;
//
// // <summary>
// // @Author: zrh
// // @Date: 2022,12,24,11:19
// // @Description:
// // </summary>
//
// namespace Game.UI
// {
//     /// <summary>
//     ///     网格
//     ///     author:sy
//     /// </summary>
//     public class UIGrid : MonoBehaviour
//     {
//         // 使用缓存
//         internal bool cache = false;
//         // 四周间隔
//         public Vector2 border = new Vector2(5, 5);
//         // 格子
//         protected UICell[,] cells;
//         // 缓存的格子
//         protected readonly List<UICell> cellCaches = new List<UICell>();
//         // viewport 遮罩层
//         protected RectTransform mask;
//         // 列
//         protected int mColumn;
//         // 格子数量
//         protected int mCount;
//         // 格子根节点
//         protected RectTransform root;
//         // 滑动组件
//         protected ScrollRect mScrollRect;
//         // 格子间隔
//         public Vector2 space = new Vector2(2f, 2f);
//         // 格子模版
//         protected UICell template;
//
//
//         public string cellPrefabName;
//
//         // 格子大小
//         protected RectTransform templateRect;
//         /// <summary>
//         ///     获取格子
//         /// </summary>
//         public UICell this[int row, int column]
//         {
//             get { return cells[row, column]; }
//         }
//         // 滚动组件
//         public ScrollRect scrollRect
//         {
//             get { return mScrollRect; }
//         }
//
//         // init field
//         protected virtual void Awake()
//         {
//             mask = transform.GetChildComponent<RectTransform>("Mask");
//             root = mask.GetChildComponent<RectTransform>(0);
//             template = transform.GetChildComponent<UICell>("Cell");
//             
//             if (template == null)
//             {
//                 //TODO 需要修改
//                 //template = ResMgr.Instance.ResLoad<UICell>(cellPrefabName);
//             }
//
//             templateRect = template.GetComponent<RectTransform>();
//             template.gameObject.SetActive(false);
//             mScrollRect = transform.GetComponent<ScrollRect>();
//         }
//
//         public void Clear()
//         {
//             if (root.childCount != 0)
//             {
//                 if (cache)
//                 {
//                     for (var i = root.childCount - 1; i >= 0; --i) // 这里必须是反向删除
//                     {
//                         var c = root.GetChildComponent<UICell>(i);
//                         c.gameObject.SetActive(false);
//                         cellCaches.Add(c);
//                     }
//                 }
//                 else
//                 {
//                     for (var i = root.childCount - 1; i >= 0; --i) // 这里必须是反向删除
//                         Destroy(root.GetChild(i).gameObject);
//                 }
//             }
//         }
//
//         /// <summary>
//         ///     生成网格
//         /// </summary>
//         /// <param name="column">列</param>
//         /// <param name="count">数量</param>
//         /// <param name="data">数据</param>
//         public void Launch(int column, int count, Array data)
//         {
//             mCount = count;
//             mColumn = column;
//             if (root.childCount != 0)
//             {
//                 if (cache)
//                 {
//                     for (var i = root.childCount - 1; i >= 0; --i) // 这里必须是反向删除
//                     {
//                         var c = root.GetChildComponent<UICell>(i);
//                         c.gameObject.SetActive(false);
//                         cellCaches.Add(c);
//                     }
//                 }
//                 else
//                 {
//                     for (var i = root.childCount - 1; i >= 0; --i) // 这里必须是反向删除
//                     {
//                         Destroy(root.GetChild(i).gameObject);
//                     }
//                 }
//             }
//
//             if (mCount == 0 || mColumn == 0)
//             {
//                 Debug.Log("UIGrid : count == 0 || column == 0");
//                 return;
//             }
//
//             //mScrollRect.inertia = false;
//             var remainder = mCount % mColumn;
//             var row = mCount / mColumn;
//             row += remainder == 0 ? 0 : 1;
//             cells = new UICell[row, mColumn];
//             var y = border.y + (1 - templateRect.pivot.y) * templateRect.sizeDelta.y;
//             var counter = 0; // 计数器
//             for (var i = 0; i < row; ++i)
//             {
//                 var max = i == row - 1 && remainder != 0 ? remainder : mColumn; // 当前行的列数
//                 for (var j = 0; j < max; ++j)
//                 {
//                     var cell = GetCellTemplate();
//                     cell.transform.SetParent(root);
//                     cell.transform.localScale = Vector3.one;
//                     cell.gameObject.SetActive(true);
//                     cell.SetContent(data.GetValue(counter));
//                     ++counter;
//                     cell.name = string.Format("{0}x{1}", i, j);
//                     var rect = cell.GetComponent<RectTransform>();
//                     rect.anchoredPosition = new Vector2(border.x + j * (cell.Size.x + space.x) + rect.pivot.x * cell.Size.x, -y);
//                     cells[i, j] = cell;
//                    
//                 }
//                 y += cells[0, 0].Size.y + space.y;
//             }
//             y += border.y - space.y; // root 高度
//             var width = border.x * 2 + mColumn * (cells[0, 0].Size.x + space.x) - space.x; // root 宽度
//             root.sizeDelta = new Vector2(width, y);
//             mScrollRect.horizontal = width > mask.sizeDelta.x;
//             mScrollRect.vertical = y > mask.sizeDelta.y;
//         }
//
//         /// <summary>
//         ///     生成网格
//         /// </summary>
//         /// <param name="column">列</param>
//         /// <param name="count">数量</param>
//         public void Launch(int column, int count)
//         {
//             mCount = count;
//             mColumn = column;
// #if UNITY_EDITOR
//             if (mCount == 0 || mColumn == 0)
//             {
//                 Debug.LogWarning("参数传入有误 : count == 0 || column == 0");
//                 return;
//             }
// #endif
//             if (root.childCount != 0)
//             {
//                 if (cache)
//                 {
//                     for (var i = root.childCount - 1; i >= 0; --i) // 这里必须是反向删除
//                     {
//                         var c = root.GetChildComponent<UICell>(i);
//                         c.gameObject.SetActive(false);
//                         cellCaches.Add(c);
//                     }
//                 }
//                 else
//                 {
//                     for (var i = root.childCount - 1; i >= 0; --i) // 这里必须是反向删除
//                         Destroy(root.GetChild(i).gameObject);
//                 }
//             }
//             //mScrollRect.inertia = false;
//             var remainder = mCount % mColumn;
//             var row = mCount / mColumn;
//             row += remainder == 0 ? 0 : 1;
//             cells = new UICell[row, mColumn];
//             var y = border.y + (1 - templateRect.pivot.y) * templateRect.sizeDelta.y;
//             for (var i = 0; i < row; ++i)
//             {
//                 var max = i == row - 1 && remainder != 0 ? remainder : mColumn; // 当前行的列数
//                 for (var j = 0; j < max; ++j)
//                 {
//                     var cell = GetCellTemplate();
//                     cell.transform.SetParent(root);
//                     cell.transform.localScale = Vector3.one;
//                     cell.gameObject.SetActive(true);
//                     cell.name = string.Format("{0}x{1}", i, j);
//                     var rect = cell.GetComponent<RectTransform>();
//                     rect.anchoredPosition = new Vector2(border.x + j * (cell.Size.x + space.x) + rect.pivot.x * cell.Size.x, -y);
//                     cells[i, j] = cell;
//                     //SetPosition(Vector2.zero);
//                 }
//                 y += cells[0, 0].Size.y + space.y;
//             }
//             y += border.y - space.y; // root 高度
//             var width = border.x * 2 + mColumn * (cells[0, 0].Size.x + space.x) - space.x; // root 宽度
//             root.sizeDelta = new Vector2(width, y);
//             mScrollRect.horizontal = width > mask.sizeDelta.x;
//             mScrollRect.vertical = y > mask.sizeDelta.y;
//         }
//
//         /// <summary>
//         /// 获取一个模版
//         /// </summary>
//         /// <returns></returns>
//         protected UICell GetCellTemplate()
//         {
//             UICell cell;
//             if (cellCaches.Count > 0)
//             {
//                 cell = cellCaches[0];
//                 cellCaches.RemoveAt(0);
//                 cell.gameObject.SetActive(true);
//             }
//             else
//             {
//                 cell = Instantiate(template);
//                 cell.gameObject.SetActive(true);
//                 cell.transform.SetParent(root);
//                 cell.transform.localScale = Vector3.one;
//             }
//             return cell;
//         }
//
//         /// <summary>
//         ///     遍历
//         /// </summary>
//         /// <param name="callback"></param>
//         public void Foreach<T>(System.Action<T> callback) where T : UICell
//         {
//             if (mColumn == 0 || mCount == 0) return;
//             var remainder = mCount % mColumn;
//             var row = mCount / mColumn;
//             row += remainder == 0 ? 0 : 1;
//             for (var i = 0; i < row; ++i)
//             {
//                 var max = i == row - 1 && remainder != 0 ? remainder : mColumn; // 当前行的列数
//                 for (var j = 0; j < max; ++j)
//                     callback((T)cells[i, j]);
//             }
//         }
//
//         /// <summary>
//         ///     遍历
//         /// </summary>
//         /// <param name="callback"></param>
//         /// <param name="breakCondition"></param>
//         public void Foreach<T>(System.Action<T> callback, Predicate<T> breakCondition) where T : UICell
//         {
//             var remainder = mCount % mColumn;
//             var row = mCount / mColumn;
//             row += remainder == 0 ? 0 : 1;
//             for (var i = 0; i < row; ++i)
//             {
//                 var max = i == row - 1 && remainder != 0 ? remainder : mColumn; // 当前行的列数
//                 for (var j = 0; j < max; ++j)
//                 {
//                     var cell = (T)cells[i, j];
//                     if (breakCondition(cell)) // 遍历终止
//                         return;
//                     callback(cell);
//                 }
//             }
//         }
//
//         /// <summary>
//         ///     遍历
//         /// </summary>
//         /// <param name="callback"></param>
//         public void Foreach(System.Action<UICell> callback)
//         {
//             if (mCount == 0 || mColumn == 0)
//             {
//                 Debug.LogWarning("UIGrid| mCount =0 || mColumn == 0");
//                 return;
//             }
//             var remainder = mCount % mColumn;
//             var row = mCount / mColumn;
//             row += remainder == 0 ? 0 : 1;
//             for (var i = 0; i < row; ++i)
//             {
//                 var max = i == row - 1 && remainder != 0 ? remainder : mColumn; // 当前行的列数
//                 for (var j = 0; j < max; ++j)
//                     callback(cells[i, j]);
//             }
//         }
//
//         /// <summary>
//         ///     条件寻找
//         /// </summary>
//         /// <param name="callback"></param>
//         public UICell Find(Predicate<UICell> callback)
//         {
//             var remainder = mCount % mColumn;
//             var row = mCount / mColumn;
//             row += remainder == 0 ? 0 : 1;
//             for (var i = 0; i < row; ++i)
//             {
//                 var max = i == row - 1 && remainder != 0 ? remainder : mColumn; // 当前行的列数
//                 for (var j = 0; j < max; ++j)
//                 {
//                     var cell = cells[i, j];
//                     if (callback(cell))
//                         return cell;
//                 }
//             }
//             return null;
//         }
//
//         /// <summary>
//         ///     条件寻找
//         /// </summary>
//         /// <param name="callback"></param>
//         public T Find<T>(Predicate<T> callback) where T : UICell
//         {
//             var remainder = mCount % mColumn;
//             var row = mCount / mColumn;
//             row += remainder == 0 ? 0 : 1;
//             for (var i = 0; i < row; ++i)
//             {
//                 var max = i == row - 1 && remainder != 0 ? remainder : mColumn; // 当前行的列数
//                 for (var j = 0; j < max; ++j)
//                 {
//                     var cell = (T)cells[i, j];
//                     if (callback(cell))
//                         return cell;
//                 }
//             }
//             return null;
//         }
//         /// <summary>
//         ///  获取一组cell
//         /// </summary>
//         /// <typeparam name="T"></typeparam>
//         /// <param name="condition"></param>
//         /// <param name="count"></param>
//         /// <returns></returns>
//         public T[] FindRange<T>(Predicate<T> condition, int count) where T : UICell
//         {
// #if UNITY_EDITOR
//             if (count > mCount)
//             {
//                 Debug.LogError($"需要的格子数量count = {count} 大于 格子总数量 total = {mCount}");
//                 return null;
//             }
// #endif
//             var res = new T[count];
//             if (count == 0)
//                 return res;
//             var index = 0;
//             var remainder = mCount % mColumn;
//             var row = mCount / mColumn;
//             row += remainder == 0 ? 0 : 1;
//             for (var i = 0; i < row; ++i)
//             {
//                 var max = i == row - 1 && remainder != 0 ? remainder : mColumn; // 当前行的列数
//                 for (var j = 0; j < max; ++j)
//                 {
//                     var cell = (T)cells[i, j];
//                     if (condition(cell))
//                     {
//                         res[index] = cell;
//                         if (++index == count)
//                             return res;
//                     }
//                 }
//             }
//             return res;
//         }
//
//         /// <summary>
//         ///  获取一组cell 无条件
//         /// </summary>
//         /// <typeparam name="T"></typeparam>
//         /// <param name="count"></param>
//         /// <returns></returns>
//         public T[] FindRange<T>(int count) where T : UICell
//         {
// #if UNITY_EDITOR
//             if (count > mCount)
//             {
//                 Debug.LogError($"需要的格子数量count = {count} 大于 格子总数量 total = {mCount}");
//                 return null;
//             }
// #endif
//             var res = new T[count];
//             if (count == 0)
//                 return res;
//             var index = 0;
//             var remainder = mCount % mColumn;
//             var row = mCount / mColumn;
//             row += remainder == 0 ? 0 : 1;
//             for (var i = 0; i < row; ++i)
//             {
//                 var max = i == row - 1 && remainder != 0 ? remainder : mColumn; // 当前行的列数
//                 for (var j = 0; j < max; ++j)
//                 {
//                     var cell = (T)cells[i, j];
//                     res[index] = cell;
//                     if (++index == count)
//                         return res;
//                 }
//             }
//             return res;
//         }
//
//         /// <summary>
//         /// 像生成格子的添加网格追加单元格
//         /// </summary>
//         /// <param name="arg"></param>
//         public void Append(object arg)
//         {
//             if (mCount == 0 || mColumn == 0)
//                 throw new Exception("Please first cast Launch() function");
//             var remainder = mCount % mColumn;
//             var row = mCount / mColumn;
//             row += remainder == 0 ? 0 : 1;
//             var regular = remainder == 0;
//             var last = cells[row - 1, regular ? mColumn : remainder];
//             var lastPos = last.GetComponent<RectTransform>().anchoredPosition;
//             var cell = GetCellTemplate();
//             cell.SetContent(arg);
//             var rect = cell.GetComponent<RectTransform>();
//             var temp = rect.anchoredPosition;
//             temp.x = regular ? border.x + rect.pivot.x * cell.Size.x : lastPos.x + cell.Size.x + space.x;
//             temp.y = regular ? lastPos.y + cell.Size.y + space.y : lastPos.y;
//             rect.anchoredPosition = temp;
//             
//             ++mCount;
//             if (!regular)
//             {
//                 cells[row - 1, remainder + 1] = cell;
//                 return;
//             }
//             // new 多一行
//             var cellsCopy = new UICell[row + 1, mColumn];
//             for (var i = 0; i < row; ++i)
//             {
//                 var max = i == row - 1 && remainder != 0 ? remainder : mColumn; // 当前行的列数
//                 for (var j = 0; j < max; ++j)
//                     cellsCopy[i, j] = cells[i, j];
//             }
//             cellsCopy[row, 0] = cell;
//             cells = cellsCopy;
//         }
//
//         /// <summary>
//         ///     位置
//         /// </summary>
//         /// <param name="position"></param>
//         public void SetPosition(Vector2 position)
//         {
//             root.anchoredPosition = position;
//         }
//
//         /// <summary>
//         /// 设置单元格中心点
//         /// </summary>
//         /// <param name="pivot"></param>
//         public void SetPivot(Vector2 pivot)
//         {
//             Foreach(c =>
//             {
//                 var rect = c.GetComponent<RectTransform>();
//                 var x0 = rect.anchoredPosition.x - rect.pivot.x * rect.sizeDelta.x;
//                 var y0 = rect.anchoredPosition.y - rect.pivot.y * rect.sizeDelta.y;
//                 rect.anchoredPosition = new Vector2(x0 + pivot.x * rect.sizeDelta.x, y0 + pivot.y * rect.sizeDelta.y);
//                 rect.pivot = pivot;
//             });
//         }
//
//     }
// }