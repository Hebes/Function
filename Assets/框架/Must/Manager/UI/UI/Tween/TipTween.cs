using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// <summary>
// @Author: zrh
// @Date: 2023,01,20,10:36
// @Description:
// </summary>

namespace Game.UI
{
    /// <summary>
    /// tip 上下移动动画
    /// </summary>
    public class TipTween : MonoBehaviour
    {
        // 曲线
        public AnimationCurve cure = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
        // 周期
        public float duration = .5f;
        // 移动距离
        public float distance = 2f;
        // 动画模式
        public WrapMode mode = WrapMode.PingPong;
        // trans
        private RectTransform rect;
        // 原始值
        private float origin;
        // 计时器
        private float timer;
        // 实际曲线
#if UNITY_EDITOR
        public
#else
        private 
#endif
            AnimationCurve runCure;
        //inti
        void Awake()
        {
            rect = GetComponent<RectTransform>();
            enabled = false;
            GraphicBuildEvent.Create(gameObject, () => { origin = rect.anchoredPosition.y; });
        }

        // play
        public void Play()
        {
            var kfs = new Keyframe[cure.length];
            for (var i = 0; i < cure.length; ++i)
            {
                var kf = cure[i];
                kfs[i] = new Keyframe(kf.time * duration, kf.value * distance);
            }
            runCure = new AnimationCurve(kfs)
            {
                preWrapMode = mode,
                postWrapMode = mode
            };
            timer = Time.unscaledTime;
            enabled = true;
        }

        // stop
        public void Stop()
        {
            enabled = false;
        }

        void Update()
        {
            float val;
            if (mode == WrapMode.Once)
            {
                var t = Time.unscaledTime - timer;
                if (t > duration)
                    timer = Time.unscaledTime;
                val = runCure.Evaluate(t);

            }
            else val = runCure.Evaluate(Time.unscaledTime);
            var temp = rect.anchoredPosition;
            temp.y = origin + val;
            rect.anchoredPosition = temp;
        }
    }
}