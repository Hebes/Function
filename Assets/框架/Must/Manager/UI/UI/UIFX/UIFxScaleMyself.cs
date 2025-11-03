using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

// <summary>
// @Author: zrh
// @Date: 2023,01,29,19:29
// @Description:
// </summary>

namespace Game.UI
{
    public class UIFxScaleMyself : MonoBehaviour
    {
        public Vector3 from = new Vector3(3, 3, 3);
        public Vector3 to = new Vector3(1, 1, 1);
        public float duration = 1f;
        public Ease ease = Ease.InOutElastic;

        public bool autoPlay = true;


        private void OnEnable()
        {
            if (autoPlay)
            {
                Play();
            }
        }

        private void Disable()
        {
            Stop();
        }

        [ContextMenu("Play")]
        public void Play()
        {
            transform.localScale = from;
            var tween = transform.DOScale(to, duration);
            tween.SetEase(ease);
            tween.Restart();
            tween.Play();
        }

        [ContextMenu("Stop")]
        public void Stop()
        {
            transform.DOPause();
            transform.localScale = to;
        }
    }
}