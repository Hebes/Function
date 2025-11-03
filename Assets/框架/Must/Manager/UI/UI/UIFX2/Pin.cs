using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// <summary>
// @Author: zrh
// @Date: 2023,02,09,10:53
// @Description:
// </summary>

namespace Game.UI
{
    public class Pin : MonoBehaviour
    {
        public Transform target;

        private Transform cachedTransform;
		
        private void Awake()
        {
            cachedTransform = transform;
        }

        private void LateUpdate()
        {
            if (Vector3.Distance(cachedTransform.position, target.position) > 0.001f)
            {
                cachedTransform.position = target.position;
            }
        }
    }
}