using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

namespace Item10W
{
    /// <summary>
    /// 道具信息
    /// </summary>
    public class Item
    {
        public int id;
        public int num;
    }

    /// <summary>
    /// 背包面板 主要是用来更新背包逻辑
    /// </summary>
    public class BagPanel : MonoBehaviour
    {
        public RectTransform content;
        private CustomSV<Item, BagItem> sv;
        public BagItem bagItem;//物品资源
        public List<Item> itemList { get; private set; }// 临时填充数据
        public Button btn;

        public int iTemp;

        void Start()
        {
            btn.onClick.AddListener(ActionClick);
            itemList = new List<Item>();
            //这个方法 是我们模拟获取数据的方法,在实际开发中 数据应该是从服务器 或者 是本地文件中读取出来的
            for (int i = 0; i < 14; ++i)
            {
                itemList.Add(new Item() { id = i, num = i, });
                iTemp = i;
            }

            sv = new CustomSV<Item, BagItem>(bagItem);
            //初始化格子间隔大小 以及 一行几列
            sv.InitItemSizeAndCol(117, 117, 4);
            //初始化COntent父对象以及可视范围
            sv.InitContentAndSVH(content, 117 * 3);//
            //初始化数据来源
            sv.InitInfos(itemList);

        }

        private void ActionClick()
        {
            int max = iTemp + 10;
            for (int i = iTemp; i < max; ++i)
            {
                iTemp = i;
                itemList.Add(new Item() { id = iTemp, num = iTemp, });
            }
            sv.InitInfos(itemList);
        }

        void Update()
        {
            sv.CheckShowOrHide();
        }
    }
}