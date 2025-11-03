using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// <summary>
// @Author: zrh
// @Date: 2022,12,06,16:52
// @Description:
// </summary>

namespace zhaorh
{
    public partial class Tools
    {
        /// <summary>
        /// 安全的按索引取数组里的值，策划有约定如果索引大于数组长度，则取数组最后一个值
        /// </summary>
        /// <returns>The get by index.</returns>
        /// <param name="arr">Arr.</param>
        /// <param name="index">Index.</param>
        public static long SafeGetByIndex (long[] arr, int index)
        {
            if (index >= 0 && index < arr.Length) {
                return arr [index];
            }
            return arr [arr.Length - 1];
        }
    }
}