using UnityEngine;

namespace Framework.Core
{
    public static class ExpansionInterface
    {
        /// <summary>
        /// 检查接口是否存在
        /// </summary>
        /// <typeparam name="T">原先的接口</typeparam>
        /// <typeparam name="K">需要转换的接口</typeparam>
        /// <param name="t">原先的接口实例</param>
        /// <returns>返回转换的接口</returns>
        public static K ChackInherit<T, K>(this T t)
        {
            if (t is K k)
                return k;
            Debug.LogError($"{t.GetType().FullName}请继承{typeof(K).FullName}");
            return default;
        }

        public static bool ChackInterFace<T, K>(this T t)
        {
            if (t is K k)
                return true;
            Debug.LogError($"{t.GetType().FullName}请继承{typeof(K).FullName}");
            return false;
        }
    }
}
