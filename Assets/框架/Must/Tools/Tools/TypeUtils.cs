using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

// <summary>
// @Author: zrh
// @Date: 2022,12,15,16:28
// @Description:
// </summary>

namespace Game
{
    public static class TypeUtils
    {
        private static readonly string[] RuntimeAssemblyNames =
        {
            "Assembly-CSharp",
            "Game",
            "Common",
        };
        /// <summary>
        /// 在运行时程序集中获取指定基类的所有子类的名称。
        /// </summary>
        /// <param name="typeBase">基类类型。</param>
        /// <returns>指定基类的所有子类的名称。</returns>
        public static string[] GetRuntimeTypeNames(System.Type typeBase)
        {
            return GetTypeNames(typeBase, RuntimeAssemblyNames);
        }
        /// <summary>
        /// 在运行时程序集中获取指定基类的所有子类的名称。
        /// </summary>
        /// <param name="typeBase">基类类型。</param>
        /// <returns>指定基类的所有子类的名称。</returns>
        public static System.Type GetRuntimeType(string typeName)
        {
            return GetType(typeName, RuntimeAssemblyNames);
        }
        private static string[] GetTypeNames(System.Type typeBase, string[] assemblyNames)
        {
            List<string> typeNames = new List<string>();
            foreach (string assemblyName in assemblyNames)
            {
                Assembly assembly = null;
                try
                {
                    assembly = Assembly.Load(assemblyName);
                }
                catch
                {
                    continue;
                }

                if (assembly == null)
                {
                    continue;
                }

                System.Type[] types = assembly.GetTypes();
                foreach (System.Type type in types)
                {
                    if (type.IsClass && !type.IsAbstract && typeBase.IsAssignableFrom(type))
                    {
                        typeNames.Add(type.FullName);
                    }
                }
            }

            typeNames.Sort();
            return typeNames.ToArray();
        }
        
        private static System.Type GetType(string TypeName, string[] assemblyNames)
        {
            System.Type rType = null;
            List<string> typeNames = new List<string>();
            foreach (string assemblyName in assemblyNames)
            {
                Assembly assembly = null;
                try
                {
                    assembly = Assembly.Load(assemblyName);
                }
                catch
                {
                    continue;
                }

                if (assembly == null)
                {
                    continue;
                }

                System.Type[] types = assembly.GetTypes();
                foreach (System.Type type in types)
                {
                    if (type.IsClass && !type.IsAbstract && type.Name == TypeName)
                    {
                        rType = type;
                        break;
                    }
                }

                if (rType!=null)
                {
                    break;
                }
            }

            return rType;
        }
    }
}