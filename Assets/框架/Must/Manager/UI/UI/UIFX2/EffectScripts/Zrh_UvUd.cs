using UnityEngine;

namespace zhaorh
{
    /// <summary>
    ///  uv ud. 替代掉js脚本
    /// </summary>
    public class Zrh_UvUd : MonoBehaviour
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
                var offset = Time.realtimeSinceStartup * scrollSpeed;
                r.material.SetTextureOffset ("_MainTex", new Vector2 (0, -offset));
            }
        }
    }
}