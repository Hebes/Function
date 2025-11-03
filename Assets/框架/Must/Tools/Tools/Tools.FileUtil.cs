using System.Collections;
using System.Collections.Generic;
using System.IO;

// <summary>
// @Author: zrh
// @Date: 2022,12,08,16:53
// @Description:
// </summary>

namespace zhaorh
{
    public partial class Tools
    {
        public static byte[] ReadAllBytes (string path)
        {
            byte[] b = null;
#if UNITY_WEBPLAYER
            Debug.LogError("UNITY_WEBPLAYER 模式下 无法使用!");
#else
            b = File.ReadAllBytes (path);
#endif
            return b;
        }
        public static bool Exists (string path)
        {
			
#if UNITY_WEBPLAYER
if(Debug2.isLogErrorEnabled){
			Debug2.LogError("UNITY_WEBPLAYER not support File.Exists");
}
			return false;
#else
            return File.Exists (path);
#endif
        }
    }
}