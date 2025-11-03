using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace ExpansionUnity
{
    public static class ExpansionEventTrigger
    {
        /// <summary>
        /// 为EventTrigger的事件类型绑定Action方法
        /// </summary>
        /// <param name="trigger">EventTrigger组件对象</param>
        /// <param name="eventType">事件类型</param>
        /// <param name="listenedAction">要执行的方法</param>
        private static void AddEventTrigger(this EventTrigger trigger, EventTriggerType eventType,
            Action<PointerEventData> listenedAction)
        {
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = eventType;
            entry.callback.AddListener(data => listenedAction.Invoke((PointerEventData)data));
            trigger.triggers.Add(entry);
        }

        /// <summary>
        /// UI物体移除EventTrigger
        /// </summary>
        /// <param name="trigger">要移除事件的ui物体</param>
        /// <param name="eventType">事件类型</param>
        /// <param name="action">调用的方法</param>
        public static void RemoveEventTrigger(this EventTrigger trigger, EventTriggerType eventType,
            UnityAction<BaseEventData> action)
        {
            var entry = new EventTrigger.Entry();
            entry.eventID = eventType;
            entry.callback.AddListener(action);
            trigger.triggers.Remove(entry);
        }

        /// <summary>
        /// 添加EventTrigger组件
        /// </summary>
        /// <param name="tf"></param>
        /// <param name="eventType"></param>
        /// <param name="action"></param>
        public static void AddEventTrigger(this Transform tf, EventTriggerType eventType,
            Action<PointerEventData> action) =>
            tf.gameObject.AddEventTrigger(eventType, action);

        /// <summary>
        /// 添加EventTrigger组件
        /// </summary>
        /// <param name="goValue"></param>
        /// <param name="eventType"></param>
        /// <param name="action"></param>
        public static void AddEventTrigger(this GameObject goValue, EventTriggerType eventType,
            Action<PointerEventData> action)
        {
            var eventTrigger = goValue.GetComponent<EventTrigger>();
            eventTrigger = eventTrigger ? eventTrigger : goValue.AddComponent<EventTrigger>();
            eventTrigger.AddEventTrigger(eventType, action);
        }
    }
}