// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
//
// // <summary>
// // @Author: zrh
// // @Date: 2023,02,15,9:31
// // @Description:
// // </summary>
//
// namespace Game.UI
// {
//     public class UIGridByAutoLayout : UIListBaseByAutoLauout
//     {
//         protected override void Awake()
//         {
//             base.Awake();
//             var layout = root.GetComponent<GridLayoutGroup>();
//             if (layout.cellSize.x < .01f || layout.cellSize.y < .01f)
//                 layout.cellSize = cellTemplate.GetComponent<RectTransform>().sizeDelta;
//         }
//
//         public override void SetPosition(float postion)
//         {
//             var tmp = root.anchoredPosition;
//             if (scrollRect.horizontal)
//                 tmp.x = postion;
//             if (scrollRect.vertical)
//                 tmp.y = postion;
//             root.anchoredPosition = tmp;
//         }
//
//         /// <summary>
//         /// 生成指定个数的格子 (覆盖)
//         /// 注 ：这里生成的数据是不进行数据填充处理的
//         /// </summary>
//         /// <param name="count"></param>
//         public void Launch(int count)
//         {
//             var min = cells.Count < count ? cells.Count : count;
//             var temp = new List<UICell>(count);
//             for (var i = 0; i < min; ++i) // update
//             {
//                 var cell = cells[i];
//                 temp.Add(cell);
//             }
//             for (var i = cells.Count; i < count; ++i) // append
//             {
//                 var cell = GetCellTemplate();
//                 cell.rectTransform.SetParent(root);
//                 cell.rectTransform.Normalize();
//                 cell.name = temp.Count.ToString();
//                 temp.Add(cell);
//                 cell.rectTransform.SetAsLastSibling(); // 防止顺序混乱
//             }
//             for (var i = count; i < cells.Count; ++i) // remove
//             {
//                 var cell = cells[i];
//                 if (cache)
//                 {
//                     cell.gameObject.SetActive(false);
//                     cellCaches.Add(cell);
//                 }
//                 else Destroy(cell.gameObject);
//             }
//             cells.Clear();
//             cells.AddRange(temp);
//             SetPosition(0);
//         }
//
//         public override void MoveTo(int index, Direction direction)
//         {
//
//         }
//
//         protected override void OnClear()
//         {
//
//         }
//     }
// }