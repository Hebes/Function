using System;
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
        public static DateTime Jan1st1970 = new DateTime (1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private static long timeDiff = 0;
        /// <summary>
        ///     获取当前的系统时间，毫秒值.
        /// </summary>
        /// <returns>The time millis.</returns>
        public static long currentTimeMillis ()
        {
            return (long)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
        }
        /// <summary>
        ///     校准服务器时间
        /// </summary>
        /// <param name="timeMillisFromServer">Time millis from server.</param>
        public static void adjustServerTime (long timeMillisFromServer)
        {
            if(timeMillisFromServer <=0){//0不需要校准
                return;
            }
            long localTime = currentTimeMillis ();
            timeDiff = timeMillisFromServer - localTime;
        }
        /// <summary>
        ///     获取校准过之后的服务器时间，避免手机本地时间设置不正确引起问题,timeDiff需要在登录之后进校准
        ///     Java举例1970年的毫秒值
        /// </summary>
        /// <value>The server time.</value>
        public static long serverTime {
            get { return (currentTimeMillis () + timeDiff); }
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
        
        /// <summary>
        ///     针对需要按周期时间生长的体力，技能点等属性，根据当前属性值以及最近一次结算时间来计算玩家此刻应该拥有的属性点
        /// </summary>
        /// <returns>返回玩家当前的可获得属性值数量.</returns>
        /// <param name="lastCalcPoint">玩家最近一次结算属性时的属性值.</param>
        /// <param name="lastCalcTime">玩家最近一次结算属性时的时间戳，单位是毫秒.</param>
        /// <param name="period">每一点属性值的生长周期,单位是毫秒，比如120000是两分钟</param>
        /// <param name="maxPointCount">属性值的生长上限，30比如是表示可获得的最大体力是30.</param>
        public static long GetPointCount (long lastCalcPoint, long lastCalcTimeLong, long period, int maxPointCount)
        {
            if (lastCalcPoint >= maxPointCount) {
                //玩家花钱已经获得的属性超过了上限，则直接是当前值
                return lastCalcPoint;
            }
            long lastCalcTime = getTimeMillisFromTimestamp4Long (lastCalcTimeLong);
            var p = (int)(lastCalcPoint + (serverTime - lastCalcTime) / period);
            if (p >= maxPointCount) {
                p = maxPointCount;
            }
            if (p < 0) {
                p = 0;
            }
            return p;
        }
        /// <summary>
        /// sql格式的精确到秒的日期转化成毫秒值
        /// </summary>
        /// <returns>The time millis from timestamp4 long.</returns>
        /// <param name="timestamp4Long">Timestamp4 long.</param>
        public static long getTimeMillisFromTimestamp4Long (long timestamp4Long)
        {
            try {
                if(timestamp4Long<19700101000001L){
                    timestamp4Long = 19700101000001L;
                }
                var dt = DateTime.ParseExact ("" + timestamp4Long + " +8", "yyyyMMddHHmmss z", null).ToUniversalTime ();
                return (long)(dt - Jan1st1970).TotalMilliseconds; //+ (1000 * 60 * 60 * 8);	
            } catch (Exception ex) {
                Debug.LogError ("getTimeMillisFromTimestamp4Long|err|" + timestamp4Long + "|isnot|yyyyMMddHHmmss");
                Debug.LogException (ex);
            }
            return 0L;
        }
        
        /// <summary>
        ///     计算在玩家属性生长未到达上限之前距离获得下一点属性值的剩余时间.
        /// </summary>
        /// <returns>返回属性生长剩余时间，如果属性值已经超过上限，剩余时间则为0.</returns>
        /// <param name="lastCalcPoint">玩家最近一次结算属性时的属性值.</param>
        /// <param name="lastCalcTimeLong">玩家最近一次结算属性时的时间戳，单位是秒.</param>
        /// <param name="period">每一点属性值的生长周期,单位是毫秒，比如120000是两分钟</param>
        /// <param name="maxPointCount">属性值的生长上限，30比如是表示可获得的最大体力是30.</param>
        public static int GetNextPointTime (long lastCalcPoint, long lastCalcTimeLong, long period, int maxPointCount)
        {
            if (lastCalcPoint >= maxPointCount) {
                //玩家花钱已经获得的属性超过了上限，则直接是不需要倒计时了
                //Debug.Log ("技能点本来就大于上限，肯定花钱买了|lastCalcPoint=" + lastCalcPoint + "|lastCalcTimeLong=" + lastCalcTimeLong + "|period=" + period + "|maxPointCount=" + maxPointCount);
                return 0;
            }
            long lastCalcTimeMillis = getTimeMillisFromTimestamp4Long (lastCalcTimeLong);
            long serverTimeMillis = serverTime;
            var p = (int)(lastCalcPoint + (serverTimeMillis - lastCalcTimeMillis) / period);
            if (p >= maxPointCount) {
                //Debug.Log ("累计时间折算达到点数上限了，GetNextPointTime|lastCalcPoint=" + lastCalcPoint + "|lastCalcTimeLong=" + lastCalcTimeLong + "|period=" + period + "|maxPointCount=" + maxPointCount);
                return 0;
            }
            long growedTime = (serverTimeMillis - lastCalcTimeMillis) % period;
            int leftTime = (int)((period - growedTime) / 1000);
            if (leftTime < 0) {
                //Debug.Log ("倒计时负数了，GetNextPointTime|lastCalcPoint=" + lastCalcPoint + "|lastCalcTimeLong=" + lastCalcTimeLong + "|period=" + period + "|maxPointCount=" + maxPointCount);
                leftTime = 0;
            }
            //Debug.Log ("GetNextPointTime|lastCalcPoint=" + lastCalcPoint + "|lastCalcTimeLong=" + lastCalcTimeLong + "|period=" + period + "|maxPointCount=" + maxPointCount
            //+ "|serverSqlTimestamp4Long=" + getTimestamp4LongFromTimeMillis (serverTimeMillis) + "|leftTime=" + leftTime);
            return leftTime;
        }
        public static long getTimestamp4LongFromTimeMillis (long timeMillis)
        {
            DateTime dt = ConvertJavaMillisecondsToDateTime (timeMillis);
            long val = 0;
            long.TryParse(dt.ToString("yyyyMMddHHmmss"), out val);
            return val;
        }
        /// <summary>
        /// 计算倒计时，单位是毫秒
        /// </summary>
        /// <returns>比如还剩1小时3分5秒，则格式化成 01:03:05</returns>
        /// <param name="timeMills">剩余时间的毫秒值.,时间到了的情况下，返回00:00:00</param>
        public static String GetCountDownTime (long timeMills)
        {
            if (timeMills <= 0) {
                return "00:00";
            }
            System.Text.StringBuilder sb = new System.Text.StringBuilder ();
            long day = timeMills / (1000L * 60 * 60 * 24);
            long hour = (timeMills % (1000L * 60 * 60 * 24)) / (1000L * 60 * 60);
            long minute = (timeMills % (1000L * 60 * 60)) / (1000L * 60);
            long second = (timeMills % (1000L * 60)) / (1000L);
            if (day != 0) {
                sb.Append (day).Append ("天");
            }
            if (hour != 0) {
                if (hour < 10) {
                    sb.Append ("0");
                }
                sb.Append (hour).Append (":");
            }
            if (minute < 10) {
                sb.Append ("0");
            }
            sb.Append (minute).Append (":");
            if (second < 10) {
                sb.Append ("0");
            }
            sb.Append (second);
            return sb.ToString ();

        }
        /// <summary>
        /// 根据结束时间计算当前的倒计时时间，并格式化显示出来
        /// </summary>
        /// <returns>返回格式化的时间，比如当然时间离结束时间还剩1小时3分5秒，则格式化成 01:03:05.</returns>
        /// <param name="javaTimeMillsForEnd">结束时间的java毫秒值.</param>
        /// <param name="isEnd">true是倒计时结束了.</param>
        public static String GetCountDownTimeToEndtime (long javaTimeMillsForEnd, out bool isEnd)
        {
            long serverTimeTmp = serverTime;
            isEnd = (serverTimeTmp - javaTimeMillsForEnd >= 0);
            return GetCountDownTime (javaTimeMillsForEnd - serverTime);
        }
        /// <summary>
        ///  万分比长整型时间转浮点数时间
        /// </summary>
        /// <returns>The long2 time.</returns>
        /// <param name="t">T.</param>
        public static float WanfenbiLong2Time(long t){
            float f = t / 10000.0F;
            return f;
        }
        /// <summary>
        /// 浮点数时间转万分比长整型时间
        /// </summary>
        /// <returns>The long2 time.</returns>
        /// <param name="f">F.</param>
        public static long WanfenbiTime2Long(float f){
            long t = (long)(f*10000);
            return t;
        }
    }
}