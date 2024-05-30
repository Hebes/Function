using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace 为枚举提供自定义字符串
{
    public class Test : MonoBehaviour
    {
        public string str;
        public TimeUnitType timeUnitType;

        /// <summary>
        /// 当前时间类型
        /// </summary>
        public TimeUnitType TimeUnitType
        {
            get
            {
                return timeUnitType;
            }
            private set
            {
                timeUnitType = value;
                str = GetDescriptions<TimeUnitType>()[(int)value];
            }
            
        }
        
        public TimeUnitType timeUnitType1;//这个时测试用的

        private void Update()
        {
            TimeUnitType = timeUnitType1;
        }

        /// <summary>
        /// 获取枚举的自定义字符串,需要结合EnumStrAttribute特性使用
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <returns>字符串集合</returns>
        private static List<string> GetDescriptions<T>() where T : System.Enum
        {
            //var methods = GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            Type attrType = typeof(DescriptionAttribute);
            Type type = typeof(T);
            var fields = type.GetFields();
            List<string> strArray = new List<string>();
            foreach (var item in fields) 
            {
                if (item.FieldType != type)
                {
                    continue;
                }

                Attribute attribute = Attribute.GetCustomAttribute(item, attrType);
                if (attribute != null)
                {
                    strArray.Add(((DescriptionAttribute)attribute).str);
                }
                else
                {
                    Debug.LogError($"{type.Name} 的 {item.Name} has no attribute which type is EnumStrAttribute");
                }
            }

            return strArray;
        }
    }
}