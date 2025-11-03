using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using zhaorh.UI;

// <summary>
// @Author: zrh
// @Date: 2022,12,06,16:52
// @Description:
// </summary>

namespace zhaorh
{
    public partial class Tools
    {
        
        /// <summary>
        /// 获取UI特效加亮的材质
        /// </summary>
        public static Material UIFxAdditiveMaterial
        {
            get
            {
                var material = Resources.Load<Material>("Material/Effect_ui_additive");
                if (!material)
                {
                    Debug.LogError("[UIFxAdditiveMaterial]获取UI特效加量的材质失败!!请检查资源是否存在!");
                }
                return material;
            }
        }
        private static Material greyMt;
        /// <summary>
        /// 获取一个灰色材质
        /// </summary>
        public static Material GreyMaterial
        {
            get
            {
                if (!greyMt)
                {
                    greyMt = Resources.Load<Material>("Material/UI-Grayscale");
                    if (!greyMt)
                    {
                        Debug.LogError("[GreyMaterial]获取UI特效加量的材质失败!!请检查资源是否存在!");
                    }
                }
                return greyMt;
            }
        }
        
    }
}