using DG.Tweening;
using UnityEngine;

namespace Game.UI
{
    public class UIFxLoopRotation360 : MonoBehaviour
    {
        public Vector3 from = new Vector3(0, 0, 0);
        public Vector3 to = new Vector3(0, 0, 360);
        public float duration = 0.4f;

        private void Start()
        {
            Play();
        }

        [ContextMenu("Play")]
        public void Play()
        {
            transform.DOPause();

            transform.localPosition = from;
            var tween = transform.DOLocalRotate(to, duration, RotateMode.FastBeyond360);
            tween.SetEase(Ease.Linear);
            tween.SetLoops(-1, LoopType.Restart);
        }

        [ContextMenu("Stop")]
        public void Stop()
        {
            transform.localPosition = from;
            transform.DOPause();
        }
    }
}