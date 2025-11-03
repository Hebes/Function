using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// <summary>
// @Author: zrh
// @Date: 2023,01,18,15:37
// @Description:
// </summary>

namespace Game.UI
{
    /// <summary>
    /// 自动设置大小
    /// </summary>
    // [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    public class AutoSetSize : MonoBehaviour
    {
        public RectTransform target;

        public float maxHeight = -1f;

        private RectTransform thisRectTransform;

        private void Awake()
        {
            thisRectTransform = GetComponent<RectTransform>();
        }

        private void Update()
        {
            if (Mathf.Abs(thisRectTransform.sizeDelta.x - target.sizeDelta.x) > 0.1f || Mathf.Abs(thisRectTransform.sizeDelta.y - target.sizeDelta.y) > 0.1f)
            {
                thisRectTransform.sizeDelta = new Vector2(target.sizeDelta.x, maxHeight < 0f ? target.sizeDelta.y : Mathf.Min(target.sizeDelta.y, maxHeight));
            }
        }
    }
}