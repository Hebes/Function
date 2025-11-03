using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// <summary>
// @Author: zrh
// @Date: 2023,02,07,17:18
// @Description:
// </summary>

namespace Game.UI
{
    [AddComponentMenu("Tween/AlphaTween")]
    public class AlphaTween : MonoBehaviour
    {
        public AnimationCurve cure = new AnimationCurve(new Keyframe(0, 1f), new Keyframe(1, 0f));
        // 一个曲线循环的周期
        public float duration = 1f;
        // 是否作用于子节点
        public bool attachChild = false;
        // 是否循环
        public bool loop = false;
        private AnimationCurve runCure;
        // 计时器
        private float timer;
        // 目标
        private Graphic[] graphics;
        // init
        void Awake()
        {
            Duration = duration;
            graphics = attachChild ? GetComponentsInChildren<Graphic>() : new[] { GetComponent<Graphic>() };
            enabled = false;
        }

        public float Duration
        {
            set
            {
                var frames = cure.keys;
                runCure = new AnimationCurve();
                for (var i = 0; i < frames.Length; ++i)
                {
                    var frame = frames[i];
                    runCure.AddKey(new Keyframe(frame.time * value, frame.value));
                }
            }
        }

        /// <summary>
        /// 设置目标alpha值
        /// </summary>
        /// <param name="alpha"></param>
        void SetAlpha(float alpha)
        {
            for (var i = 0; i < graphics.Length; ++i)
            {
                var temp = graphics[i].color;
                temp.a = alpha;
                graphics[i].color = temp;
            }
        }

        /// <summary>
        /// 播放
        /// </summary>
        public void Play()
        {
            timer = Time.unscaledTime;
            enabled = true;
        }

        public void Stop()
        {
            enabled = false;
            SetAlpha(1f);
        }

        void Update()
        {
            var t = Time.unscaledTime - timer;
            if (t < duration)
                SetAlpha(runCure.Evaluate(t));
            else if (loop) timer = Time.unscaledTime;
            else Stop();
        }
#if UNITY_EDITOR
        [ContextMenu("Play")]
        void Test()
        {
            Play();
        }
#endif
    }
}