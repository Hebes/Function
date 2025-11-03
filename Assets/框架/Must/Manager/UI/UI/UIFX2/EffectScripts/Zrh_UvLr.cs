using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// <summary>
// @Author: zrh
// @Date: 2023,01,20,19:49
// @Description:
// </summary>

namespace zhaorh
{
    /// <summary>
    ///  uv lr.替代js脚本
    /// </summary>
    public class Zrh_UvLr : MonoBehaviour
    {

        public float scrollSpeed;
        private Renderer r = null;

        void Start ()
        {
            r = GetComponent<Renderer> ();
        }
	
        // Update is called once per frame
        void Update ()
        {
            if (r != null) {
                var offset = Time.time * scrollSpeed;
                r.material.SetTextureOffset ("_MainTex", new Vector2 (-offset, 0));
            }
        }
    }
}