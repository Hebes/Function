using System;
using System.Collections.Generic;
using UnityEngine;

namespace zhaorh.UI
{
    /// <summary>
    /// 无参数毁掉
    /// </summary>
    public class Event
    {
        private readonly List<Action> list = new List<Action>();

        /// <summary>
        /// 追加一个毁掉
        /// </summary>
        /// <param name="callback"></param>
        public void AddListener(Action callback)
        {
            list.Add(callback);
        }

        /// <summary>
        /// 追加一组毁掉
        /// </summary>
        /// <param name="callbacks"></param>
        public void AddListener(params Action[] callbacks)
        {
            for (var i = 0; i < callbacks.Length; ++i)
            {
                list.Add(callbacks[i]);
            }
        }

        /// <summary>
        /// 移除对应的回调
        /// </summary>
        /// <param name="callback"></param>
        public void RemoveListener(Action callback)
        {
            var index = list.FindIndex(target => callback == target);
            if (index == -1)
            {
                Debug.LogWarning("{0} be not found");
                return;
            }
            list.RemoveAt(index);
        }

        /// <summary>
        /// 移除所有回调
        /// </summary>
        public void RemoveAllListeners()
        {
            list.Clear();
        }

        /// <summary>
        /// 调用
        /// </summary>
        public void Invoke()
        {
            list.ForEach(callback => callback());
        }

        public bool Exists(Action callback)
        {
            return list.Exists(obj => callback == obj);
        }
    }

    /// <summary>
    /// 一个参数毁掉
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Event<T>
    {
        private readonly List<Action<T>> list = new List<Action<T>>();

        /// <summary>
        /// 追加一个回调
        /// </summary>
        /// <param name="callback"></param>
        public void AddListener(Action<T> callback)
        {
            list.Add(callback);
        }

        /// <summary>
        /// 追加一组回调
        /// </summary>
        /// <param name="callbacks"></param>
        public void AddListener(params Action<T>[] callbacks)
        {
            for (var i = 0; i < callbacks.Length; ++i)
            {
                list.Add(callbacks[i]);
            }
        }

        /// <summary>
        /// 移除对应的回调
        /// </summary>
        /// <param name="callback"></param>
        public void RemoveListener(Action<T> callback)
        {
            var index = list.FindIndex(target => callback == target);
            list.RemoveAt(index);
        }

        /// <summary>
        /// 移除所有回调
        /// </summary>
        public void RemoveAllListener()
        {
            list.Clear();
        }

        /// <summary>
        /// 调用
        /// </summary>
        public void Invoke(T t)
        {
            list.ForEach(callback => callback(t));
        }

        public int GetActionCount()
        {
            return list.Count;
        }
    }

    /// <summary>
    /// 两个参数毁掉
    /// </summary>
    /// <typeparam name="T0"></typeparam>
    /// <typeparam name="T1"></typeparam>
    public class Event<T0, T1>
    {
        private readonly List<Action<T0, T1>> list = new List<Action<T0, T1>>();

        /// <summary>
        /// 追加一个回调
        /// </summary>
        /// <param name="callback"></param>
        public void AddListener(Action<T0, T1> callback)
        {
            list.Add(callback);
        }

        /// <summary>
        /// 追加一组回调
        /// </summary>
        /// <param name="callbacks"></param>
        public void AddListener(params Action<T0, T1>[] callbacks)
        {
            for (var i = 0; i < callbacks.Length; ++i)
            {
                list.Add(callbacks[i]);
            }
        }

        /// <summary>
        /// 移除对应的回调
        /// </summary>
        /// <param name="callback"></param>
        public void RemoveListener(Action<T0, T1> callback)
        {
            var index = list.FindIndex(target => callback == target);
            list.RemoveAt(index);
        }

        /// <summary>
        /// 移除所有回调
        /// </summary>
        public void RemoveAllListener()
        {
            list.Clear();
        }

        /// <summary>
        /// 调用
        /// </summary>
        public void Invoke(T0 t0, T1 t1)
        {
            list.ForEach(callback => callback(t0, t1));
        }
    }
}
