﻿using UnityEngine;
using UnityEngine.UI;

namespace Item10W
{
    /// <summary>
    /// 格子类对象  他是放在背包里的一个一个的道具格子
    /// 主要用来显示 单组道具信息的
    /// </summary>
    public class BagItem : MonoBehaviour, IItemBase<Item>, IPool
    {
        public Text text;

        private void Awake()
        {
            text = transform.Find("txtNum").GetComponent<Text>();
        }
        /// <summary>
        /// 这个方法 是用于初始化 道具格子信息
        /// </summary>
        /// <param name="info"></param>
        public void InitInfo(Item info)
        {
            //先读取道具表 
            //根据表中数据 来更新信息
            //更新图标
            //更新名字
            text.text = info.num.ToString();//更新道具数量
        }

        public void Get()
        {
            gameObject.SetActive(true);
            Debug.Log($"获取{text}");
        }

        public void Push()
        {
            gameObject.SetActive(false);
            Debug.Log($"推入{text}");
        }
    }
}