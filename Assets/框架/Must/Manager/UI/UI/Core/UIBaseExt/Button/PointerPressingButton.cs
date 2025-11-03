using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// <summary>
// @Author: zrh
// @Date: 2023,01,18,9:50
// @Description:
// </summary>

namespace Game.UI
{
    [RequireComponent(typeof(Button)), DisallowMultipleComponent]
    public class PointerPressingButton : EventTrigger
    {
        [System.Serializable]
        public class OnPressDownHandler : UnityEvent
        {

        }

        [System.Serializable]
        public class OnPressingHandler : UnityEvent<Vector2>
        {

        }

        [System.Serializable]
        public class OnPressUpHandler : UnityEvent
        {

        }

        [SerializeField]
        public OnPressDownHandler OnPressDown = new OnPressDownHandler();
        [SerializeField]
        public OnPressingHandler OnPressing = new OnPressingHandler();
        [SerializeField]
        public OnPressUpHandler OnPressUp = new OnPressUpHandler();

        public bool activated { get; private set; }

        void Start()
        {

            RectTransform rect = GetComponent<RectTransform>();
            rect.localScale = Vector3.one;
            rect.anchoredPosition = Vector3.zero;
        }

        /// <summary>
        /// Mouse/Touch 按下事件回调
        /// </summary>
        /// <param name="eventData">Event data.</param>
        public override void OnPointerDown(PointerEventData eventData)
        {
            activated = true;

            OnPressDown.Invoke();
        }

        /// <summary>
        /// 摇杆拖曳事件回调
        /// </summary>
        /// <param name="eventData">Event data.</param>
        public override void OnDrag(PointerEventData eventData)
        {
            Vector3 deltaPos = GetScreenPoint(eventData.delta);
            OnPressing.Invoke(new Vector2(deltaPos.x, deltaPos.y));
        }

        public void Stop()
        {
            OnPointerUp(null);
        }


        /// <summary>
        /// Mouse/Touch 离开事件回调
        /// </summary>
        /// <param name="eventData">Event data.</param>
        public override void OnPointerUp(PointerEventData eventData)
        {
            activated = false;
            OnPressUp.Invoke();
        }

        Vector2 GetScreenPoint(Vector2 point)
        {
            //默认是宽对齐
            return point * (1136f / Screen.width);
        }
    }
}