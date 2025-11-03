using DG.Tweening;
using UnityEngine;

namespace Game.UI
{
    [AddComponentMenu("UI/Effects/UiScaleEffect")]
    public class UiScaleEffect : MonoBehaviour
    {
        [SerializeField]
        float T = 1f;

        [SerializeField]
        Ease ease = Ease.Linear;

        float _time = 0f;

        Vector3 oriScale;
        
        RectTransform uiBaseTrans = null;
        
        void Awake()
        {
            uiBaseTrans = transform.GetComponent<RectTransform>();
            oriScale = uiBaseTrans.localScale;
            _time = 0f;
        }
        void OnEnable()
        {
            StartScale();
        }

        public void StartScale()
        {
            uiBaseTrans.localScale = Vector3.zero;

            uiBaseTrans.DOScale(oriScale, T).SetEase(ease);
        }
    }
}