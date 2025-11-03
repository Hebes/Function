using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

// <summary>
// @Author: zrh
// @Date: 2023,02,08,17:00
// @Description:
// </summary>

namespace Game.UI
{
    [AddComponentMenu("UI/Effects/UIScaleEffect")]
    public class UIScaleEffect : MonoBehaviour
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