using DG.Tweening;
using UnityEngine;

namespace Game.UI
{
    public class UIFxLoopScale : MonoBehaviour
    {
        public Vector3 from = new Vector3(1, 1, 1);
        public Vector3 to = new Vector3(0, 0, 0);
        public float duration = 0.4f;

        public bool autoPlay = true;

        [ContextMenu("Init")]
        private void Start()
        {
            transform.localScale = from;
            var tween = transform.DOScale(to, duration);
            tween.SetLoops(-1, LoopType.Yoyo);
        }

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
            transform.DOPlay();
        }

        [ContextMenu("Stop")]
        public void Stop()
        {
            transform.DOPause();
        }
    }
}