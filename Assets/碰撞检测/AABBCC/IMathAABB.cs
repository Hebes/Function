using System.Numerics;

namespace 碰撞检测.AABBCC
{
    /// <summary>
    /// 接口
    /// </summary>
    public interface IMathAABB
    {
        Vector3 MinVector { get; }
 
        Vector3 MaxVector { get; }
 
        Vector3 Center { get; }
 
        Vector3[] Corners { get; }
 
        /// <summary>
        /// Gets the center point of the bounding box.
        /// </summary>
        /// <returns>获取中心点</returns>
        Vector3 GetCenter();
 
        /// <summary>
        ///  Near face, specified counter-clockwise looking towards the origin from the positive z-axis.
        ///  verts[0] : left top front
        ///  verts[1] : left bottom front
        ///  verts[2] : right bottom front
        ///  verts[3] : right top front
        ///  Far face, specified counter-clockwise looking towards the origin from the negative z-axis.
        ///  verts[4] : right top back
        ///  verts[5] : right bottom back
        ///  verts[6] : left bottom back
        ///  verts[7] : left top back
        /// </summary>
        /// <returns>获取包围盒八个顶点信息</returns>
        void GetCorners();
 
        /// <summary>
        /// Tests whether this bounding box intersects the specified bounding object.
        /// </summary>
        /// <returns>判断两个包围盒是否碰撞</returns>
        bool Intersects(IMathAABB aabb);
 
        /// <summary>
        /// check whether the point is in.
        /// </summary>
        /// <returns>返回这个点是否在包围盒中</returns>
        bool ContainPoint(Vector3 point);
 
        /// <summary>
        /// Sets this bounding box to the smallest bounding box
        /// that contains both this bounding object and the specified bounding box.
        /// </summary>
        /// <returns>生成一个新的包围盒 同时容纳两个包围盒，新的包围盒: min各轴要是其他两个最小的那个，max各轴要是其他两个最大的那个</returns>
        void Merge(IMathAABB box);
 
        /// <summary>
        /// Sets this bounding box to the specified values.
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns>设置</returns>
        void SetMinMax(Vector3 min, Vector3 max);
 
        /// <summary>
        /// reset min and max value.
        /// </summary>
        /// <returns>重置</returns>
        void ResetMinMax();
 
        bool IsEmpty();
    }
}