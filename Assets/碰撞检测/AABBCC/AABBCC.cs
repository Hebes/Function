using UnityEngine;
using Vector3 = System.Numerics.Vector3;

namespace 碰撞检测.AABBCC
{
    public class AABBCC : MonoBehaviour,IMathAABB
    {
        [SerializeField]
        public Vector3 MinVector { get; }
        public Vector3 MaxVector { get; }
        public Vector3 Center { get; }
        public Vector3[] Corners { get; }
        public Vector3 GetCenter()
        {
            throw new System.NotImplementedException();
        }

        public void GetCorners()
        {
            throw new System.NotImplementedException();
        }

        public bool Intersects(IMathAABB aabb)
        {
            throw new System.NotImplementedException();
        }

        public bool ContainPoint(Vector3 point)
        {
            throw new System.NotImplementedException();
        }

        public void Merge(IMathAABB box)
        {
            throw new System.NotImplementedException();
        }

        public void SetMinMax(Vector3 min, Vector3 max)
        {
            throw new System.NotImplementedException();
        }

        public void ResetMinMax()
        {
            throw new System.NotImplementedException();
        }

        public bool IsEmpty()
        {
            throw new System.NotImplementedException();
        }
    }
}