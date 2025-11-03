using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// <summary>
// @Author: zrh
// @Date: 2023,01,24,9:20
// @Description:
// </summary>

namespace Game.UI
{
    /// <summary>
    /// 动画
    /// </summary>
    public class Tween : MonoBehaviour
    {
        /// <summary>
        ///  参数是一个变化的值
        /// </summary>
        private readonly zhaorh.UI.Event<float> onUpdate = new zhaorh.UI.Event<float>();
        // 完成回调
        private readonly zhaorh.UI.Event onComplete = new zhaorh.UI.Event();
        // 曲线
        public AnimationCurve curve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
        // 周期
        private float dur;
        // 计时器
        private float timer;
        // 比例系数
        private float k;
        // 时间缩放系数
        private float tk;
        private float mStart;

        void Awake()
        {
            enabled = false;
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="start">起始值</param>
        /// <param name="end">结束值</param>
        /// <param name="duration">周期</param>
        public Tween Launch(float start, float end, float duration)
        {
            var kv = curve.keys[curve.length - 1];
            if (!Mathf.Approximately(kv.value, 1f) || !Mathf.Approximately(kv.time, 1f))
            {
                Debug.LogError("need keep the line max = [1,1]");
                return this;
            }
            mStart = start;
			timer = Time.unscaledTime;
            k = end - start;
            dur = duration;
            tk = 1 / dur;
            enabled = true;
            return this;
        }

        /// <summary>
        /// 回调参数是 变化的中间值
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public Tween OnUpdate(Action<float> action)
        {
            onUpdate.RemoveAllListener();
            onUpdate.AddListener(action);
            return this;
        }

        public Tween OnComplete(Action action)
        {
            onComplete.RemoveAllListeners();
            onComplete.AddListener(action);
            return this;
        }

        public bool IsPlay
        {
            get { return enabled; }
        }

        void Update()
        {
			var t = Time.unscaledTime - timer;
            if (t > dur)
            {
                Stop();
                return;
            }
            onUpdate.Invoke(mStart + k * curve.Evaluate(t * tk));
        }

        public void Stop()
        {
            enabled = false;
            onComplete.Invoke();
        }
    }
}