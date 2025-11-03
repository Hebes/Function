using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// <summary>
// @Author: zrh
// @Date: 2023,01,16,19:36
// @Description:
// </summary>

namespace Game.UI
{
    /// <summary>
    /// 拖拽按钮
    /// </summary>
    public class ButtonDrag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler
    {
        // 用来标记正在拖拽中
        private bool draging;

        /// <summary>
        /// 回调
        /// </summary>
        public readonly zhaorh.UI.Event<Vector2> onDrag = new zhaorh.UI.Event<Vector2>();
        /// <summary>
        /// 单机回调
        /// </summary>
        public readonly zhaorh.UI.Event onClick = new zhaorh.UI.Event();

        // 防止直接禁用按钮(不调用OnEndDrag)时，按钮状态错误问题
        void OnEnable()
        {
            draging = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            //var pos = eventData.position;
            onDrag.Invoke(eventData.delta);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            draging = true;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            draging = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (draging) return; // 判定为拖拽时不做 单机回调
            onClick.Invoke();
        }

        /// <summary>
        /// 单机回调追加
        /// </summary>
        /// <param name="callback"></param>
        public void AppendClick(Action callback)
        {
            onClick.AddListener(callback);
        }

        /// <summary>
        /// 拖拽回调追加
        /// </summary>
        /// <param name="callback"></param>
        public void AppendDrag(Action<Vector2> callback)
        {
            onDrag.AddListener(callback);
        }
    }
}