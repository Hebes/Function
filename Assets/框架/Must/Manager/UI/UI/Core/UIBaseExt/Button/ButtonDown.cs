using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// <summary>
// @Author: zrh
// @Date: 2022,12,22,17:35
// @Description:
// </summary>

namespace Game.UI
{
    public class ButtonDown :  MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        // 单机
        public readonly zhaorh.UI.Event onDown = new zhaorh.UI.Event();
        // 单机
        public readonly zhaorh.UI.Event onUp = new zhaorh.UI.Event();

        [SerializeField]
        private Image targetImage;

        public Image image
        {
            get
            {
                if (targetImage == null)
                {
                    targetImage = GetComponent<Image>();
                }
                return targetImage;
            }
        }

        void Awake()
        {
            if (targetImage == null)
            {
                targetImage = GetComponent<Image>();
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            onDown.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            onUp.Invoke();
        }

        /// <summary>
        /// 追加回调
        /// </summary>
        /// <param name="action"></param>
        public void AppendDownCallback(Action action)
        {
            onDown.AddListener(action);
        }
        /// <summary>
        /// 追加回调
        /// </summary>
        /// <param name="action"></param>
        public void AppendUpCallback<T>(Action<T> action) where T : Component
        {
            var t = GetComponent<T>();
            onUp.AddListener(() => action(t));
        }

        /// <summary>
        /// 追加回调
        /// </summary>
        /// <param name="action"></param>
        public void AppendUpCallback(Action action)
        {
            onUp.AddListener(action);
        }
        /// <summary>
        /// 追加回调
        /// </summary>
        /// <param name="action"></param>
        public void AppendDownCallback<T>(Action<T> action) where T : Component
        {
            var t = GetComponent<T>();
            onDown.AddListener(() => action(t));
        }
        
    }
}