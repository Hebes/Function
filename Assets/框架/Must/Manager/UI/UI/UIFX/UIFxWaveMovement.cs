using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

// <summary>
// @Author: zrh
// @Date: 2023,01,16,16:57
// @Description:
// </summary>

namespace Game.UI
{
    public class UIFxWaveMovement : MonoBehaviour
    {
        public float amplitude = 1f;
        public float duration = 0.5f;

        private Vector3 originalPosition;

        private void Awake()
        {
            originalPosition = transform.localPosition;
        }

        private void OnEnable()
        {
            ClearContent();
        }

        private void OnDisable()
        {
            transform.DOPause();
        }

        private void ClearContent()
        {
            transform.localPosition = originalPosition;
        }

        public void PlayAnimation()
        {
            transform.DOLocalMoveY(transform.localPosition.y + amplitude, duration)
                .ChangeStartValue(transform.localPosition.y - amplitude)
                .SetUpdate(true)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine)
                .Goto(duration / 2, true);
        }
    }
}