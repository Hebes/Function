using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// <summary>
// @Author: zrh
// @Date: 2023,02,09,10:35
// @Description:
// </summary>

namespace Game.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class Zrh_Scrollbar : Scrollbar, IEndDragHandler
    {
        public bool IsDraging { get; private set; }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            base.OnBeginDrag(eventData);

            IsDraging = true;
        }
		
        public virtual void OnEndDrag(PointerEventData eventData)
        {
            IsDraging = false;
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);

            IsDraging = true;
        }
		
        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);

            IsDraging = false;
        }
    }
}