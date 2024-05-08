using System;
using UnityEngine;

namespace DateTimett
{
    /// <summary>
    /// https://blog.csdn.net/weixin_40948750/article/details/108293874
    /// https://wenku.csdn.net/answer/5w49dqoq0y
    /// </summary>
    public class DateTimeMono : MonoBehaviour
    {
        private void Awake()
        {
           Debug.Log($"今天{DateTime.Now.Date.ToShortDateString()}");
        }
    }
}