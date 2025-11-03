using DG.Tweening;
using UnityEngine;

namespace Game.UI
{
    public class UIFxCanvasGroupLoopFadeInOut : MonoBehaviour
    {
        public float from = 1f;
        public float to = 0.4f;

        public float duration = 1f;
        public Ease easeType = Ease.Linear;

        [ContextMenu("Play")]
        public void Play()
        {
            var canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.DOFade(to, duration)
                .SetEase(easeType)
                .SetUpdate(true)
                .SetLoops(-1, LoopType.Yoyo)
                .ChangeStartValue(from)
                .ChangeEndValue(to);
        }

        [ContextMenu("Stop")]
        public void Stop()
        {
            var canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.DOPause();
        }
    }
}