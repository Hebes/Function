using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
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
        #region MD5编码
        /// <summary>
        /// 16位
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string To16Md5(string str)
        {
            //使用加密服务提供程序
            var md5 = new MD5CryptoServiceProvider();
            //将指定的字节子数组的每个元素的数值转换为它的等效十六进制字符串表示形式。
            var md5Pwd = BitConverter.ToString(md5.ComputeHash(Encoding.Default.GetBytes(str)), 4, 8);
            md5Pwd = md5Pwd.Replace("-", "");
            return md5Pwd;
        }

        /// <summary>
        /// 32 位
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string To32Md5(string str)
        {
            string pwd = string.Empty;

            //实例化一个md5对像
            MD5 md5 = MD5.Create();

            // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(str));

            // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
            for (int i = 0; i < s.Length; i++)
            {
                // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符 
                pwd = pwd + s[i].ToString("X");
            }
            return pwd;
        }
        #endregion
    }
}