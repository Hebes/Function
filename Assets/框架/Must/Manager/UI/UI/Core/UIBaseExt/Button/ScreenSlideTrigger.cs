using UnityEngine;
using UnityEngine.EventSystems;
using zhaorh.UI;

namespace Game.UI
{
    public class ScreenSlideTrigger : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        private RectTransform selfRect;
        public float SlideStrength = 0.2f;
        private Vector2 size;
        
        void Start()
        {
            selfRect = GetComponent<RectTransform>();

            size = GetComponent<RectTransform>().sizeDelta;
        }
        
        private Vector2 startPosition;
        private Vector2 curPosition;
        private Vector2 slideVector;

        public readonly Event<Vector2> OnStartDrag = new Event<Vector2>();
        public readonly Event<Vector2> OnDraging = new Event<Vector2>();
        public readonly Event<Vector2> OnStopDrag = new Event<Vector2>();
        public readonly Event<Vector2> OnSlide = new Event<Vector2>();

        private bool slided = true;

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (OnStartDrag != null)
            {
                OnStartDrag.Invoke(eventData.position);
            }

            slided = false;

            CalculateLocalPosition(eventData, out startPosition);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (OnStopDrag != null)
            {
                OnStopDrag.Invoke(eventData.position);
            }

            CalculateLocalPosition(eventData, out curPosition);

            CheckDelta();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (OnDraging != null)
            {
                OnDraging.Invoke(eventData.delta);
            }

            CalculateLocalPosition(eventData, out curPosition);

            CheckDelta();
        }

        private void CheckDelta()
        {
            if (!slided)
            {
                slideVector = curPosition - startPosition;

                if (Mathf.Abs(slideVector.x) >= size.x * SlideStrength)
                {
                    slided = true;
                    if (OnSlide != null)
                    {
                        OnSlide.Invoke(slideVector);
                    }
                }
            }
        }

        private void CalculateLocalPosition(PointerEventData data, out Vector2 localPos)
        {
            Vector2 tmp;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(selfRect, data.position, data.pressEventCamera, out tmp))
            {
                localPos = tmp;
            }
            else
            {
                localPos = curPosition;
            }
        }
    }
}