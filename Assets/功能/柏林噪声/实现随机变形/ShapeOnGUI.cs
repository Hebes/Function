using System.Collections;
using UnityEngine;
using UnityEngine.UI;


namespace 柏林噪声.实现随机变形
{
    public class ShapeOnGUI : MaskableGraphic
    {
        public int Detail = 60;
        public float Radius = 200;
        public float Diff = 100;
        public float DataOffset = 100;

        private Vector2 _offset;
        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
            vh.AddVert(Vector3.zero, color, Vector4.zero);
        
            var deltaAngle = Mathf.PI * 2.0f / Detail;
            var len = Detail - 1;
            for (var i = 0; i < Detail; ++i)
            {
                var angle = deltaAngle * i;
                var r = Radius + Mathf.PerlinNoise(_offset.x + DataOffset + Mathf.Cos(angle), _offset.y + DataOffset + Mathf.Sin(angle)) * Diff;
                vh.AddVert(new Vector3(r * Mathf.Cos(angle), r * Mathf.Sin(angle), 0), color, Vector4.zero );
                if( i == len )
                    vh.AddTriangle( 0, i + 1, 1 );
                else
                    vh.AddTriangle(0, i + 1, i + 2);
            }
        }

        protected override void Start()
        {
            base.Start();
            StartCoroutine(OnUpdate());
        }

        protected override void OnDisable()
        {
            StopAllCoroutines();
            base.OnDisable();
        }

        private IEnumerator OnUpdate()
        {
            while (gameObject.activeSelf)
            {
                _offset += Vector2.right * 0.01f;
                //SetAllDirty();
                SetVerticesDirty();
                yield return null;
            }
        }
    }


}