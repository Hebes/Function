using System.Collections.Generic;

namespace Framework.Core
{
    public static class ExpansionList
    {
        /// <summary>
        /// 给list集合添加一个元素，如果不包含的话
        /// </summary>
        /// <param name="list"></param>
        /// <typeparam name="T"></typeparam>
        /// <return >是否添加成功</return>
        public static bool AddNotContainElement<T>(this List<T> list, T t)
        {
            if (list.Contains(t)) return false;
            list.Add(t);
            return true;
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static void AddNotContainElement<T>(this List<T> list, params T[] t)
        {
            for (int i = 0; i < t.Length; i++)
            {
                if (!list.Contains(t[i]))
                    list.Add(t[i]);
            }
        }

        /// <summary>
        /// 获取指定元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="K"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static K GetContainElement<T, K>(this List<T> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] is K k)
                    return k;
            }
            return default;
        }

        /// <summary>
        /// 从list集合移除一个元素，如果包含的话
        /// </summary>
        /// <param name="list"></param>
        /// <typeparam name="T"></typeparam>
        /// <return >是否移除成功</return>
        public static bool RemoveContainElement<T>(this List<T> list, T t)
        {
            if (!list.Contains(t)) return false;
            list.Remove(t);
            return true;
        }


        /// <summary>
        /// 获取集合中的一个随机元素
        /// </summary>
        /// <param name="list"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetRandom<T>(this List<T> list)
        {
            return list.Count < 1 ? default : list[UnityEngine.Random.Range(0, list.Count)];
        }
    }
}
