using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

// <summary>
// @Author: zrh
// @Date: 2022,12,24,11:25
// @Description:
// </summary>

namespace Game.UI
{
    [RequireComponent(typeof(Button)), DisallowMultipleComponent]
    public class LongPressButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {
        private bool pressing = false;
        [SerializeField]
        public float longPressClickInterval = 0.4f;
        private float lastSendClickEventTime = 0f;
        private float eventStartTime = 0f;//第一次按下的时间
        [FormerlySerializedAs("onClick")]
        [SerializeField]
        private Button.ButtonClickedEvent m_OnClick = new Button.ButtonClickedEvent();

        public Button.ButtonClickedEvent onClick
        {
            get
            {
                return m_OnClick;
            }
            set
            {
                m_OnClick = value;
            }
        }

        /// <summary>
        /// 普通点击，和长按区分
        /// </summary>
        private Button.ButtonClickedEvent m_OnNormalClick = new Button.ButtonClickedEvent();
        public Button.ButtonClickedEvent onNormalClick
        {
            get { return m_OnNormalClick; }
            set {  m_OnNormalClick = value; }
        }

        /// <summary>
        ///  onClickDown扩展
        /// </summary>
        [SerializeField]
        private Button.ButtonClickedEvent m_OnClickDown = new Button.ButtonClickedEvent();

        public Button.ButtonClickedEvent onClickDown
        {
            get
            {
                return m_OnClickDown;
            }
            set
            {
                m_OnClickDown = value;
            }
        }

        /// <summary>
        ///onClickUp扩展
        /// </summary>
        [SerializeField]
        private Button.ButtonClickedEvent m_OnClickUp = new Button.ButtonClickedEvent();

        public Button.ButtonClickedEvent onClickUp
        {
            get
            {
                return m_OnClickUp;
            }
            set
            {
                m_OnClickUp = value;
            }
        }

        void OnDisable()
        {
            if (pressing)
            {
                pressing = false;

                if (onClickUp != null)
                {
                    onClickUp.Invoke();
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (onClick == null)
            {
                return;
            }
            if (longPressClickInterval < 0.4f)
            {
                longPressClickInterval = 0.4f;
            }
            if (pressing && Time.realtimeSinceStartup - lastSendClickEventTime > longPressClickInterval)
            {
                Debug.Log(this + "|Update|onClick|长按触发at|" + Time.frameCount + "|" + lastSendClickEventTime + "<" + Time.realtimeSinceStartup + "|longPressClickInterval=" + longPressClickInterval);
                lastSendClickEventTime = Time.realtimeSinceStartup;
                onClick.Invoke();
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            lastSendClickEventTime = Time.realtimeSinceStartup;
            eventStartTime = lastSendClickEventTime;
            pressing = true;
            onClickDown.Invoke();
        }


        public void OnPointerUp(PointerEventData eventData)
        {
            if(pressing &&(Time.realtimeSinceStartup - eventStartTime < longPressClickInterval)  && eventData.pointerEnter == gameObject)
            {
                if(m_OnNormalClick != null)
                {
                    m_OnNormalClick.Invoke();
                }
            }
            lastSendClickEventTime = Time.realtimeSinceStartup;

            pressing = false;
            onClickUp.Invoke();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (pressing)
            {
                pressing = false;

                lastSendClickEventTime = Time.realtimeSinceStartup;

                if (onClickUp != null)
                {
                    onClickUp.Invoke();
                }
            }
        }
    }
}