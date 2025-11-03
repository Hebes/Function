using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// <summary>
// @Author: zrh
// @Date: 2023,01,20,10:36
// @Description:
// </summary>

namespace Game.UI
{
    /// <summary>
    /// 解决 UGUI build 延时问题
    /// </summary>
    public class GraphicBuildEvent : MonoBehaviour
    {
        /// <summary>
        /// UI构建文成时调用
        /// </summary>
        Action onBulideGraphic;

        public static GraphicBuildEvent Create(GameObject target, Action callback)
        {  
            var build =  target.AddComponent<GraphicBuildEvent>();
            build.onBulideGraphic = callback;
            return build;
        }

        void LateUpdate()
        {
            onBulideGraphic.Invoke();
            Destroy(this);
        }
    }
}