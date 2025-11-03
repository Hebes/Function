using UnityEngine;

namespace ExpansionUnity
{
    public static class ExpansionChild
    {
        /// <summary>
        /// 获取物体基类方法
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="childName"></param>
        /// <returns></returns>
        public static Transform GetChild(this Transform transform, string childName)
        {
            Transform childTF = transform.Find(childName);
            if (childTF != null) return childTF;
            for (int i = 0; i < transform?.childCount; i++)
            {
                childTF = GetChild(transform.GetChild(i), childName); // 2.将任务交给子物体
                if (childTF != null)
                    return childTF;
            }

            return null;
        }

        /// <summary>
        /// 获取物体
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="childName"></param>
        /// <returns></returns>
        public static GameObject GetChild(this GameObject gameObject, string childName)
        {
            return GetChild(gameObject.transform, childName).gameObject;
        }

        /// <summary>
        /// 获取物体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="gameObject"></param>
        /// <param name="childName"></param>
        /// <returns></returns>
        public static T GetChild<T>(this GameObject gameObject, string childName) where T : Component
        {
            return GetChild(gameObject.transform, childName).GetComponent<T>();
        }

        public static T GetChildComponent<T>(this Transform transform, string childName) where T : UnityEngine.Object
        {
            return GetChild(transform, childName)?.GetComponent<T>();
        }

        public static T GetChildComponent<T>(this Component component, string childName) where T : UnityEngine.Object
        {
            return GetChild(component.transform, childName)?.GetComponent<T>();
        }

        public static T GetChildComponent<T>(this GameObject gameObject, string childName) where T : UnityEngine.Object
        {
            return GetChild(gameObject.transform, childName)?.GetComponent<T>();
        }

        public static T GetChildComponent<T>(this GameObject gameObject, int i)
        {
            return gameObject.transform.GetChild(i).GetComponent<T>();
        }


        /// <summary>
        /// 清除子物体
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="Number">跳过的编号</param>
        public static void ClearChild(this Transform transform, params int[] Number)
        {
            if (transform.childCount <= 0) return;
            for (int i = 0; i < transform.childCount; i++)
            {
                for (int j = 0; j < Number.Length; j++)
                {
                    if (i == Number[j])
                        break;
                    else
                        GameObject.Destroy(transform.GetChild(i).gameObject);
                }
            }
        }

        public static void ClreatChildToPool()
        {
        }


        //通过全路径获取组件
        public static T GetChildAllPath<T>(this Transform transform, string childPath) where T : UnityEngine.Object
        {
            return transform.Find(childPath)?.GetComponent<T>();
        }

        public static T GetChildAllPath<T>(this GameObject gameObject, string childPath) where T : UnityEngine.Object
        {
            return gameObject.transform.Find(childPath)?.GetComponent<T>();
        }


        //给子节点添加父对象
        public static void AddChildNodeToParentNode(Transform parents, Transform child)
        {
            child.SetParent(parents, false);
            child.localPosition = Vector3.zero;
            child.localScale = Vector3.one;
            child.localEulerAngles = Vector3.zero;
        }
    }
}