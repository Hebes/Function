using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// <summary>
// @Author: zrh
// @Date: 2023,02,06,15:58
// @Description:
// </summary>

namespace Game.UI
{
    public class WentRotate : MonoBehaviour
    {
        #region property
        public int frameRate = 30;
        public float rotateSpeed = 1f;
        public Vector3 rotateAxial = Vector3.up;

        public float distance = 1f;
        public float moveSpeed = 1f;
        public Vector3 moveAxial = Vector3.up;
        private float delta = 0f;
        private int sign = 1;


        #endregion

        void Awake()
        {
            delta = 0f;
        }
        /// <summary>
        /// Update
        /// </summary>
        private void Update()
        {
            float deltaTime = 1f / frameRate;

            //rotate
            transform.Rotate(rotateAxial * rotateSpeed * 100 * deltaTime, Space.Self);
            //move
            transform.position += moveAxial.normalized * moveSpeed * deltaTime * sign;
            delta += sign * moveSpeed * deltaTime;
            if (Mathf.Abs(delta) >= distance)
            {
                sign = -sign;
            }
        }
    }
}