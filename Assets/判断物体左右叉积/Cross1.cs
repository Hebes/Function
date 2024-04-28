using System;
using UnityEngine;

namespace 判断物体左右叉积
{
    /// <summary>
    /// https://blog.csdn.net/ainklg/article/details/129760154
    /// </summary>
    public class Cross1: MonoBehaviour
    {
        private void Awake()
        {
            // Cross (Vector3 lhs, Vector3 rhs)
            // 定义：c = a x b，其中a b c均为向量
            //     叉积就是返回一个和传入的两个向量都垂直的向量。
            
            //叉积判断左右，返回1表示在右面，-1表示在左面。
            
            // Vector3 A = transform.right;
            // Vector3 B = go1.transform.position - transform.position;
            // float red = Vector3.Cross(A, B);
        }
    }
}