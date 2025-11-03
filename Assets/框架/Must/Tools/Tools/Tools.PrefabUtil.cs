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
        /// 加载指定的预制对象到指定的节点
        /// </summary>
        /// <returns>The prefab to game object.</returns>
        /// <param name="effectPrefabPath">Effect prefab path.</param>
        /// <param name="parentGo">Parent go.</param>
        /// <param name="needSetParent">If set to <c>true</c> need set parent.</param>
        public static GameObject loadPrefabToGameObject (string effectPrefabPath, Transform parentGo, bool needSetParent = true)
        {
            // if (effectPrefabPath != null && effectPrefabPath.Contains ("效果")) {
            //     return null;
            // }
            // GameObject go = ResMgr.Instance.ResLoad<GameObject> (effectPrefabPath);
            return null;

            //return loadPrefabToGameObject (go, parentGo, needSetParent);
        }
        
        /// <summary>
        /// 加载指定的对象到指定的节点
        /// </summary>
        /// <returns>The prefab to game object.</returns>
        /// <param name="go">Game Object.</param>
        /// <param name="parentGo">Parent go.</param>
        /// <param name="needSetParent">If set to <c>true</c> need set parent.</param>
        public static GameObject loadPrefabToGameObject (GameObject go, Transform parentGo, bool needSetParent = true)
        {
            if (go == null) {
                Debug.Log ("PrefabUtils.loadPrefabToGameObject|for|isnull");
                return null;
            }
            GameObject effectGo = (GameObject)GameObject.Instantiate (go, parentGo.position,
                Quaternion.identity);

            if (parentGo != null && needSetParent) {
                effectGo.transform.SetParent (parentGo);
                effectGo.transform.localPosition = go.transform.localPosition;
                effectGo.transform.localRotation = go.transform.localRotation;
                effectGo.transform.localScale = go.transform.localScale;
                RectTransform rectTrans = effectGo.GetComponent<RectTransform> ();
                if (rectTrans != null) {
                    RectTransform rectTransOrg = go.GetComponent<RectTransform> ();
                    rectTrans.anchoredPosition = rectTransOrg.anchoredPosition;
                }
                effectGo.name = go.name;
            } else {
                effectGo.transform.position = parentGo.position;
                Debug.Log ("PrefabUtils.loadPrefabToGameObject|parentGo.position" + parentGo.position);
            }
            return effectGo;
        }
    }
}