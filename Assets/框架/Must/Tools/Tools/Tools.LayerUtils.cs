using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        /// 循环设置对象的layer，只对Defaut的执行替换
        /// </summary>
        /// <param name="tran"></param>
        /// <param name="layerName"></param>
        public static void SetLayer(Transform tran, string layerName, bool force = false)
        {
            int layerDefault = LayerMask.NameToLayer("Default");

            int layer = LayerMask.NameToLayer(layerName);

            foreach (Transform t in tran.transform)
            {
                if (force || t.gameObject.layer == layerDefault)
                {
                    t.gameObject.layer = layer;
                }
				
                SetLayer(t, layerName, force);
            }
        }
        /// <summary>
        /// 设置layer, 包括子节点
        /// </summary>
        /// <param name="layerName"></param>
        public static void SetLayer(GameObject gameObject, string layerName)
        {
            int layer = LayerMask.NameToLayer(layerName);

            gameObject.layer = layer;
            Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].gameObject.layer = layer;
            }
        }
    }
}