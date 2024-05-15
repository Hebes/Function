using System;
using UnityEngine;
    
namespace 判断物体前后点积
{
    /// <summary>
    /// https://blog.csdn.net/ainklg/article/details/129760154
    /// </summary>
    public class Dot1 : MonoBehaviour
    {
        private void Awake()
        {
            //Dot (Vector3 lhs, Vector3 rhs)
            
            // 定义：a·b=|a|·|b|cos<a,b>
            //     点积就是两个向量相乘再乘以两个向量之间的余切函数。
            // 对于 normalized 向量，如果它们指向完全相同的方向，Dot 返回 1； 如果它们指向完全相反的方向，返回 -1；如果向量彼此垂直，则 Dot 返回 0。
           
            //使用场景：可以使用Dot求另一个物体是否在当前物体前方
            
            // Vector3 A = transform.forward;
            // Vector3 B = go1.transform.position - transform.position;
            // float red = Vector3.Dot(A, B);
            //
            // if (red>0) {
            //     Debug.Log("这个物体在物体前方");
            // }else if (red == 0) {
            //     Debug.Log("这两个物体方向垂直");
            // }else {
            //     Debug.Log("这个个物体不在玩家前方");
            // }
            
            //点积判断前后，返回1表示在前面，-1表示在背面，0表示正好垂直。
        }
    }
}