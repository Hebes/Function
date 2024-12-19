using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace 多边形UI
{
    /// <summary>
    /// https://mp.weixin.qq.com/s/S__mItSIRDah_p0b67GWPw
    /// </summary>
    [RequireComponent(typeof(CanvasRenderer))]
    public class PentagonImage : Image, IPointerClickHandler
    {
        private Vector2[] vertices;

        public void Test()
        {
            
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();

            Vector2 size = rectTransform.rect.size;

            // 计算五边形的顶点位置
            vertices = new Vector2[5];
            float radius = Mathf.Min(size.x, size.y) / 2;
            for (int i = 0; i < 5; i++)
            {
                float angle = (72 * i - 18) * Mathf.Deg2Rad;
                vertices[i] = new Vector2(radius * Mathf.Cos(angle), radius * Mathf.Sin(angle));
            }

            // 添加顶点和UV坐标到VertexHelper
            for (int i = 0; i < 5; i++)
            {
                // 计算UV坐标
                float uvX = (vertices[i].x + size.x / 2) / size.x;
                float uvY = (vertices[i].y + size.y / 2) / size.y;
                vh.AddVert(vertices[i], color, new Vector2(uvX, uvY));
            }

            // 添加三角形到VertexHelper
            vh.AddTriangle(0, 1, 2);
            vh.AddTriangle(0, 2, 3);
            vh.AddTriangle(0, 3, 4);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!RectTransformUtility.RectangleContainsScreenPoint(rectTransform, eventData.position, eventData.enterEventCamera))
                return;

            // 获取点击点相对于五边形坐标系的位置
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out localPoint);

            // 判断点击点是否在五边形内
            if (IsPointInPentagon(localPoint))
            {
                Debug.Log("Pentagon Clicked!");
                // 在这里执行点击事件的逻辑
            }
        }

        // 判断点是否在五边形内
        private bool IsPointInPentagon(Vector2 point)
        {
            int j = 4;
            bool inside = false;

            for (int i = 0; i < 5; i++)
            {
                if (vertices[i].y < point.y && vertices[j].y >= point.y || vertices[j].y < point.y && vertices[i].y >= point.y)
                {
                    if (vertices[i].x + (point.y - vertices[i].y) / (vertices[j].y - vertices[i].y) * (vertices[j].x - vertices[i].x) < point.x)
                    {
                        inside = !inside;
                    }
                }
                j = i;
            }

            return inside;
        }
    }
}