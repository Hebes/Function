using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// <summary>
// @Author: zrh
// @Date: 2022,12,06,15:50
// @Description:
// </summary>

namespace zhaorh.UI
{
    public static partial class Extension
    {
        public static string TimeToHM(this long time)
        {
            string str = string.Empty;

            System.DateTime dt = ConvertJavaMillisecondsToDateTime(time);
			
            str = string.Format("{0}", dt.ToString("HH:mm"));

            return str;
        }
        /// <summary>
        /// 服务器java的毫秒转本地时间
        /// </summary>
        /// <returns>The java milliseconds to date time.</returns>
        /// <param name="javaMS">Java M.</param>
        public static DateTime ConvertJavaMillisecondsToDateTime (long javaMS)
        {  
            DateTime UTCBaseTime = new DateTime (1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);  
            DateTime dt = UTCBaseTime.Add (new TimeSpan (javaMS * TimeSpan.TicksPerMillisecond)).ToLocalTime ();  
            return dt;  
        }
    }
}