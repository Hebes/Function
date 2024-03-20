// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
//
// public class Main : MonoBehaviour
// {
//     public static Main Instance { get; private set; }
//     public BagPanel bagPanel;
//     public BagItem bagItem;
//     public List<Item> Items { get; private set; }
//
//     void Start()
//     {
//         Instance = this;
//         Items = new List<Item>();   
//         //这个方法 是我们模拟获取数据的方法,在实际开发中 数据应该是从服务器 或者 是本地文件中读取出来的
//         for (int i = 0; i < 100000; ++i)
//             Items.Add(new Item() { id = i, num = i, });
//     }
// }
//
// /// <summary>
// /// 道具信息
// /// </summary>
// public class Item
// {
//     public int id;
//     public int num;
// }