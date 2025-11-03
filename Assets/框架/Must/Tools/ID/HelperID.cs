using System;

namespace Framework.Core
{
    /// <summary>
    /// 生成唯一ID
    /// https://blog.csdn.net/weixin_42042886/article/details/102542419
    /// https://blog.csdn.net/qq_29406323/article/details/86182706
    /// https://www.cainiaojc.com/csharp/csharp-datetime.html
    /// https://www.cnblogs.com/qingheshiguang/p/15806915.html
    /// </summary>
    public class HelperID
    {
        private static int m_id = 0;

        /// <summary>
        /// 生成ID,唯一
        /// </summary>
        public static int GenerateID()
        {
            return m_id++;
        }

        /// <summary>
        /// 由连字符分隔的32位数字
        /// </summary>
        /// <returns></returns>
        public static string GetGuid()
        {
            System.Guid guid = new Guid();
            guid = Guid.NewGuid();
            return guid.ToString();
        }

        /// <summary>
        /// 根据GUID获取19位的唯一数字序列 
        /// </summary>
        /// <returns></returns>
        public static long CreateRandSeed()
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();
            return BitConverter.ToInt64(buffer, 0);
        }

        /// <summary>
        /// 创建一个唯一值
        /// ticks这个属性值是指从0001年1月1日12：00:00开始到此时的以ticks为单位的时间，就是以ticks表示的时间的间隔数。
        /// 使用DateTime.Now.Ticks返回的是一个long型的数值
        /// </summary>
        /// <returns></returns>
        public static Int64 CreatRandID()
        {
            return DateTime.Now.Ticks;
        }


        /// <summary>
        /// 获取1970-01-01至dateTime的毫秒数
        /// https://blog.csdn.net/ultramand/article/details/134505309
        /// </summary>
        public static long GetTimestamp(DateTime dateTime)
        {
            DateTime dt1970 = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return (dateTime.Ticks - dt1970.Ticks) / 10000;
        }

        public static long GetTimestamp()
        {
            DateTime dtNow = DateTime.Now;
            DateTime dt1970 = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return (dtNow.Ticks - dt1970.Ticks) / 10000;
        }

        /// <summary>
        /// 根据时间戳timestamp（单位毫秒）计算日期
        /// https://blog.csdn.net/ultramand/article/details/134505309
        /// </summary>
        public static DateTime NewDate(long timestamp)
        {
            DateTime dt1970 = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            long t = dt1970.Ticks + timestamp * 10000;
            return new DateTime(t);
        }

        /// <summary>
        /// 时间戳方法
        /// </summary>
        /// <returns></returns>
        public static string CreateTimeId()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        //        var uuid = Guid.NewGuid().ToString();
        //        var uuidN = Guid.NewGuid().ToString("N");
        //        var uuidD = Guid.NewGuid().ToString("D");
        //        var uuidB = Guid.NewGuid().ToString("B");
        //        var uuidP = Guid.NewGuid().ToString("P");
        //        var uuidX = Guid.NewGuid().ToString("X");
        //return Json(new { uuid, uuidB, uuidD, uuidN, uuidP, uuidX
        //    }, JsonRequestBehavior.AllowGet);
        //输出结果
        //{
        //    "uuid":"5bfeb442-9a2f-420b-9d12-262ee9168c3d",
        //    "uuidB":"{72dadb5e-ee46-4f29-9696-0a1e1e265cbf}",
        //    "uuidD":"c85663e4-8329-4261-b147-06b5cca170ed",
        //    "uuidN":"7227ac945cb449b78278c6a45125914c",
        //    "uuidP":"(c38c3db8-e7a8-4edd-8f6c-b908a045a5b5)",
        //    "uuidX":"{0x737e82bd,0x5a68,0x41a6,{0x87,0x0a,0xc8,0x7d,0xcd,0xcf,0x80,0xdf}}"




        /// <summary>
        /// 生成订单编码,20位，日期部分在二次随机数
        /// </summary>
        /// <returns></returns>
        //public static string CreateOrderNo()
        //{
        //    var strNo = DateTime.Now.ToString("yyMMddHHmmssms");
        //    var iSeed = CreateRandSeed();
        //    var iSeed2 = NextRandom(1000, 2);
        //    iSeed = iSeed - 500 + iSeed2;
        //    var rnd = new Random(iSeed);
        //    var strExt = rnd.Next(1000, 10000);
        //    strNo += strExt;

        //    //再拼接2位
        //    rnd = new Random(iSeed - 1);
        //    strExt = rnd.Next(10, 99);
        //    strNo += strExt;
        //    return strNo;
        //}
        /// <summary>  
        /// 参考：msdn上的RNGCryptoServiceProvider例子  
        /// </summary>  
        /// <param name="numSeeds"></param>  
        /// <param name="length"></param>  
        /// <returns></returns>  
        //public static int NextRandom(int numSeeds, int length)
        //{
        //    // Create a byte array to hold the random value.    
        //    byte[] randomNumber = new byte[length];
        //    // Create a new instance of the RNGCryptoServiceProvider.    
        //    System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
        //    // Fill the array with a random value.    
        //    rng.GetBytes(randomNumber);
        //    // Convert the byte to an uint value to make the modulus operation easier.    
        //    uint randomResult = 0x0;
        //    for (int i = 0; i < length; i++)
        //    {
        //        randomResult |= ((uint)randomNumber[i] << ((length - 1 - i) * 8));
        //    }
        //    return (int)(randomResult % numSeeds) + 1;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        //public static int CreateRandSeed()
        //{
        //    var iSeed = 0;
        //    var guid = Guid.NewGuid().ToString();
        //    iSeed = guid.GetHashCode();
        //    return iSeed;
        //}

    }
}
