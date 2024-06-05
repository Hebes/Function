using System;
using System.Collections.Generic;
using UnityEngine;

namespace 功能.二分查找._1
{
    [Serializable]
    public class Data
    {
        public int id;
    }

    public class Test : MonoBehaviour
    {
        public List<Data> dataList = new List<Data>();

        private void Awake()
        {
           
        }
    }
}