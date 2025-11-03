using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

// <summary>
// @Author: zrh
// @Date: 2023,01,20,12:18
// @Description:
// </summary>

namespace Game.UI
{
    public class LongPressListener : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {
        public float triggerDuration = 0.3f;

        public UnityEvent onLongPress = new UnityEvent();

        private float pressDownTime = 0f;

        private bool isPressing = false;
        private bool IsPressing
        {
            get { return isPressing; }
            set
            {
                isPressing = value;
                pressDownTime = Time.time;
            }
        }

        private void Update()
        {
            CheckLongPress();
        }

        private void CheckLongPress()
        {
            if (IsPressing)
            {
                //Debug.Log("time: " + (Time.time - pressDownTime));
                if (Time.time - pressDownTime >= triggerDuration)
                {
                    onLongPress.Invoke();

                    IsPressing = false;
                }
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            // Debug.LogError(string.Format("LongPressListener|OnPointerDown|eventData.pointerEnter:{0}, this.gameObject:{1}", eventData.pointerEnter, this.gameObject));

            IsPressing = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            // Debug.LogError(string.Format("LongPressListener|OnPointerUp|eventData.pointerEnter:{0}, this.gameObject:{1}", eventData.pointerEnter, this.gameObject));

            IsPressing = false;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            // Debug.LogError(string.Format("LongPressListener|OnPointerExit|eventData.pointerEnter:{0}, this.gameObject:{1}", eventData.pointerEnter, this.gameObject));

            IsPressing = false;
        }
    }
}