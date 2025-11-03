using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// <summary>
// @Author: zrh
// @Date: 2022,12,28,19:25
// @Description:
// </summary>

namespace Game.UI
{
    [AddComponentMenu("UI/Effects/UiPuddingEffect")]
    public class UiPuddingEffect : MonoBehaviour
    {
        [SerializeField]
        public Type pivotType;

        [SerializeField]
        public float attenCoefficient = 0.2f;

        [SerializeField]
        public float R = 0.3f;

        [SerializeField]
        public float puddingT = 0.5f;

        [SerializeField]
        public float T = 1f;

        [SerializeField]
        public float cutOffR = 0.1f;

        Vector2 oriPivot;

        Vector3 oriScale;
        
        RectTransform uiBaseTrans = null;

        float r = 1f;

        float _time = 0f;

        float _cycleTime = 0f;

        float k = 1f;

        float b = 0f;

        void Start()
        {
            uiBaseTrans = transform.GetComponent<RectTransform>();
            oriPivot = uiBaseTrans.pivot;
            oriScale = uiBaseTrans.localScale;
            _time = 0f;
            _cycleTime = 0f;
            k = 2 * Mathf.PI / puddingT;
            r = R;

            b = Random.Range(0f, 2f);
        }

        void Update()
        {
            _time += Time.deltaTime;
            _cycleTime += Time.deltaTime;

            if(_cycleTime > puddingT)
            {
                r -= attenCoefficient;
            }

            if (_time > T)
            {
                _time = 0f;

                _cycleTime = 0f;

                r = R;
            }

            if (r < 0f)
            {
                return;
            }

            float st = _cycleTime;

            _cycleTime = _cycleTime % puddingT;
            
            if (float.IsNaN(_cycleTime))
            {
                Debug.LogError("puddingT:" + puddingT + "|st:" + st);
            }

            k = 2 * Mathf.PI / puddingT;
            float scale = Mathf.Sin(k * _time + b) * r;

            switch(pivotType)
            {
                case Type.Left:
                    uiBaseTrans.pivot = new Vector2(1, oriPivot.y);
                    uiBaseTrans.localScale = new Vector3(oriScale.x + scale, oriScale.y, oriScale.z);
                    break;
                case Type.Right:
                    uiBaseTrans.pivot = new Vector2(0, oriPivot.y);
                    uiBaseTrans.localScale = new Vector3(oriScale.x + scale, oriScale.y, oriScale.z);
                    break;
                case Type.Top:
                    uiBaseTrans.pivot = new Vector2(oriPivot.x, 0);
                    uiBaseTrans.localScale = new Vector3(oriScale.x, oriScale.y + scale, oriScale.z);
                    break;
                case Type.Down:
                    uiBaseTrans.pivot = new Vector2(oriPivot.x, 1);
                    uiBaseTrans.localScale = new Vector3(oriScale.x, oriScale.y + scale, oriScale.z);
                    break;
                default:
                    Debug.LogError("不可能走到这里");
                    break;
            };
        }
        
        public enum Type
        {
            Top,
            Down,
            Left,
            Right
        }
    }
}