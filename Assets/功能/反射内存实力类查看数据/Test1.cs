using System;
using UnityEngine;

namespace 反射内存实力类查看数据
{    
    [ClassReflectiveMemoryShow]
    public class Test1 : MonoBehaviour
    {
        public Test2 temp;
        private void Awake()
        {
           temp = new Test2();
        }
    }
}