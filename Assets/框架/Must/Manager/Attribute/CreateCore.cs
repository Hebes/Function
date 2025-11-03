// using System;
// using System.Collections;
// using System.Collections.Generic;
// using System.Reflection;
// using UnityEngine;
//
// public IEnumerator FrameworkCoreRun()
// {
//     var instanceList = new List<CreateCore>();
//     var assembly = Assembly.GetExecutingAssembly();
//     var types = assembly.GetTypes();
//     // 遍历所有类型
//     foreach (var type in types)
//     {
//         if (!Attribute.IsDefined(type, typeof(CreateCore))) continue;
//         var attribute = Attribute.GetCustomAttribute(type, typeof(CreateCore));
//         instanceList.Add((CreateCore)attribute);
//     }
//
//     //排序
//     instanceList.Sort();
//     //执行
//     foreach (var instanceValue in instanceList)
//     {
//         if (!typeof(ICore).IsAssignableFrom(instanceValue.Type))
//             throw new Exception($"{instanceValue.Type.Name}请继承ICore接口");
//         var instance = Activator.CreateInstance(instanceValue.Type);
//         Debug.Log($"{instanceValue.Type.Name}初始化,序列号为{instanceValue.NumberValue}");
//         var type = instance.GetType();
//         //普通方法
//         var init = type.GetMethod("Init");
//         init?.Invoke(instance, new object[] { });
//         //协程方法
//         var asyncInit = type.GetMethod("AsyncInit");
//         yield return asyncInit?.Invoke(instance, new object[] { });
//     }
// }
//
// /// <summary>
// /// 创建注解实例
// /// </summary>
// public class CreateCore : Attribute, IComparable<CreateCore>
// {
//     /// <summary>
//     /// 执行顺序
//     /// </summary>
//     public readonly int NumberValue;
//
//     /// <summary>
//     /// 需要实例化的类型
//     /// </summary>
//     public readonly Type Type;
//
//     /// <summary>
//     /// 创建注释
//     /// </summary>
//     /// <param name="type">需要创建的类型</param>
//     /// <param name="numberValue">创建的顺序</param>
//     public CreateCore(Type type, int numberValue)
//     {
//         Type = type;
//         NumberValue = numberValue;
//     }
//
//     /// <summary>
//     /// 排序
//     /// </summary>
//     /// <param name="other"></param>
//     /// <returns></returns>
//     public int CompareTo(CreateCore other)
//     {
//         return NumberValue.CompareTo(other.NumberValue);
//     }
// }
//
// public interface ICore
// {
//     /// <summary>
//     /// 初始化
//     /// </summary>
//     /// <returns></returns>
//     public void Init();
//
//     /// <summary>
//     /// 异步初始化
//     /// </summary>
//     /// <returns></returns>
//     public IEnumerator AsyncEnter();
//
//     /// <summary>
//     /// 退出
//     /// </summary>
//     public IEnumerator Exit();
// }