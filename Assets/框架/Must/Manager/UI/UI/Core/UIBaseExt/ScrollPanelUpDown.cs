using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// <summary>
// @Author: zrh
// @Date: 2022,12,23,17:26
// @Description:
// </summary>

namespace Game.UI
{
    public class ScrollPanelUpDown : MonoBehaviour {

        private List<GameObject> items = new List<GameObject>();
        private int curItemNum = 0;

        Transform contentGo = null;

        public string contentXPath = string.Empty;

        public string itemPrefabPath = string.Empty;

        public int cols = 1;

        public float nextCol = 0f;
        public float nextRow = 0f;

        public Vector2 firstItemPos, firstItemAnchorMin, firstItemAnchorMax;
        public Vector2 firstSizeDelta;

        public float horizontalBarHeight;

        void Awake()
        {
            contentGo = gameObject.transform.Find (contentXPath);
        }

        public bool AddItem(GameObject go)
        {
            if (string.IsNullOrEmpty (itemPrefabPath)) {
                Debug.LogError (this + "|没有设置item|");
                return false;
            }

            go.transform.SetParent(contentGo, true);

            // 显示在相应的位置
            int colNum = curItemNum % cols;
            int rowNum = curItemNum / cols;

            Vector2 pos = firstItemPos;
            pos.x += (colNum * nextCol);
            pos.y += (rowNum * nextRow);

            RectTransform rectTr = go.GetComponent<RectTransform> ();
            rectTr.anchorMin = firstItemAnchorMin;
            rectTr.anchorMax = firstItemAnchorMax;
            rectTr.anchoredPosition = pos;
            rectTr.sizeDelta = firstSizeDelta;

            // 设置滚动区域的size
            contentGo.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Mathf.Abs(pos.y - firstItemPos.y) + firstSizeDelta.y + horizontalBarHeight);

            items.Add (go);

            ++curItemNum;

            return true;
        }

    }
}