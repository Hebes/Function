using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// <summary>
// @Author: zrh
// @Date: 2023,01,19,19:10
// @Description:
// </summary>

namespace Game.UI
{
    /// <summary>
    /// Tab 抽象类
    /// </summary>
    /// <typeparam name="T">子类型</typeparam>
    public abstract class UITabBase : MonoBehaviour
    {
        // 关闭回调
        public Action onClose;

        // 关闭回调
        public Action onOpen;

        // 是否打开
        public bool IsOpen
        {
            get { return gameObject.activeSelf; }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public virtual void Init()
        {
        }

        public abstract void SetContent(object obj);

        protected abstract void Awake();
        public abstract void Open();
        public abstract void Close();
        public abstract void Release();
    }
}