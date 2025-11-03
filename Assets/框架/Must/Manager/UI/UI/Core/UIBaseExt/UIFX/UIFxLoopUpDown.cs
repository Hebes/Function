using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

// <summary>
// @Author: zrh
// @Date: 2023,01,23,19:06
// @Description:
// </summary>

namespace Game.UI
{
    public class UIFxLoopUpDown : MonoBehaviour
    {
        public Vector3 from = new Vector3(0, 43, 0);
        public Vector3 to = new Vector3(0, 33, 0);
        public float duration = 0.4f;

        public bool autoPlay = true;

        public Ease easeType = Ease.Linear;

        private void OnEnable()
        {
            if (autoPlay)
            {
                Play();
            }
        }

        private void OnDisable()
        {
            transform.DOPause();
        }

        [ContextMenu("Play")]
        public void Play()
        {
            transform.localPosition = from;
            transform.DOLocalMove(to, duration)
                .SetEase(easeType)
                .SetUpdate(true)
                .SetLoops(-1, LoopType.Yoyo)
                .ChangeStartValue(from)
                .ChangeEndValue(to);
            transform.DOPlay();
        }

        [ContextMenu("Stop")]
        public void Stop()
        {
            transform.DOPause();
        }
    }
}