using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// <summary>
// @Author: zrh
// @Date: 2022,12,23,11:38
// @Description:
// </summary>

namespace Game.UI
{
    /// <summary>
    /// 简单按钮
    /// </summary>
    public class ButtonSimple : Button
    {
        /// <summary>
        /// 追加回调
        /// </summary>
        /// <param name="action"></param>
        public void AppendClick<T>(Action<T> action) where T : Component
        {
            onClick.AddListener(delegate
            {
                var t = GetComponent<T>();
                action(t);
            });
        }
    }
}