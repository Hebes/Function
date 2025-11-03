using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ExpansionUnity
{
    public static class ExpansionToggle
    {
        public static void AddToggleListener(this Transform tf, UnityAction<bool> listenedAction,
            ToggleGroup group = null)
        {
            //添加或获取组件
            var trigger = tf.GetComponent<Toggle>();
            trigger = trigger ? trigger : tf.gameObject.AddComponent<Toggle>();
            trigger.onValueChanged.AddListener(listenedAction);
            trigger.group = group;
        }
    }
}