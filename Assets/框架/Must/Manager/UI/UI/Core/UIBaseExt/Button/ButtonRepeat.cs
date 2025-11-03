using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// <summary>
// @Author: zrh
// @Date: 2023,01,18,10:54
// @Description:
// </summary>

namespace Game.UI
{
    /// <summary>
    /// 按住按钮
    /// </summary>
    [RequireComponent(typeof(CanvasRenderer))]
    public class ButtonRepeat : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
    {
        // 重复准备开始时候调用
        public readonly zhaorh.UI.Event onRepeatStart = new zhaorh.UI.Event();
        // 回调
        public readonly zhaorh.UI.Event onRepeat = new zhaorh.UI.Event();
        // 按住结束回调
        public readonly zhaorh.UI.Event onRepeatEnd = new zhaorh.UI.Event();
        // 单击
        public readonly zhaorh.UI.Event onClick = new zhaorh.UI.Event();
        // 按住判断时间(这个字段是说，单击以后多久就算长按了。。。)
        public float delay = .5f;
        // 重复发送
        public float rate = .2f;

        // 开始 发送
        protected bool run;
        // 判断是否为按住
        protected bool check;
        // 判断是否长按操作
        private bool isPress;
        // 计时器
        protected float timer;
        // 计时器
        protected float delayTimer;

        [ContextMenu("Reset")]
        public void Reset()
        {
            run = false;
            check = false;
            isPress = false;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            check = true;
            run = false;
            isPress = false;
            delayTimer = Time.unscaledTime;
            onRepeatStart.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            check = false;
            if (run) // ch
            {
                run = !run;
                isPress = true;
                onRepeatEnd.Invoke();
            }

        }

        /// <summary>
        /// 判断是否单击
        /// 注：按住后，若移动光标、手指超出点击范围不会执行此回调
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerClick(PointerEventData eventData)
        {
            if (isPress) // 如果是执行按住操作后不执行单机事件
                return;
            onClick.Invoke();
        }

        /// <summary>
        /// 取消（中止）本次任何事件响应
        /// </summary>
        public void CancelAnyEvent()
        {
            check = false;
            run = false;
            isPress = true; // 阻止 单机回调
        }

        /// <summary>
        /// 追加单击回调
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void AppendClick<T>(Action<T> action) where T : Component
        {
            var t = GetComponent<T>();
            onClick.AddListener(() =>
            {
                action(t);
            });
        }

        /// <summary>
        /// 追加单击回调
        /// </summary>
        public void AppendRepeatStart(Action action)
        {
            onRepeatStart.AddListener(action);
        }

        /// <summary>
        /// 追加单击回调
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void AppendRepeatStart<T>(Action<T> action) where T : Component
        {
            var t = GetComponent<T>();
            onRepeatStart.AddListener(() =>
            {
                action(t);
            });
        }

        /// <summary>
        /// 追加单击回调
        /// </summary>
        public void AppendClick(Action action)
        {
            onClick.AddListener(action);
        }

        /// <summary>
        /// 追加单击回调
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void AppendRepeat<T>(Action<T> action) where T : Component
        {
            var t = GetComponent<T>();
            onRepeat.AddListener(() =>
            {
                action(t);
            });
        }

        /// <summary>
        /// 追加单击回调
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void AppendRepeatEnd<T>(Action<T> action) where T : Component
        {
            var t = GetComponent<T>();
            onRepeatEnd.AddListener(() =>
            {
                action(t);
            });
        }

        /// <summary>
        /// 追加单击回调
        /// </summary>
        public void AppendRepeat(Action action)
        {
            onRepeat.AddListener(action);
        }

        /// <summary>
        /// 追加单击回调
        /// </summary>
        public void AppendRepeatEnd(Action action)
        {
            onRepeatEnd.AddListener(action);
        }

        // 捕捉
        void Update()
        {
            if (check)
            {
                if (Time.unscaledTime - delayTimer > delay) // 超过延时时 开始做消息更新
                {
                    check = false;
                    run = !check;
                    timer = Time.unscaledTime;
                    return;
                }
            }
            if (run)
            {
                if (Time.unscaledTime - timer > rate) // 回调
                {
                    timer = Time.unscaledTime;
                    onRepeat.Invoke();
                }
            }
        }
    }
}