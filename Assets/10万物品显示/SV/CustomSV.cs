using System.Collections.Generic;
using UnityEngine;

namespace Item10W
{
    /// <summary>
    /// 对象池接口
    /// </summary>
    public interface IPool
    {
        /// <summary>
        /// 从对象池出来之后需要做的事
        /// </summary>
        public void Get();

        /// <summary>
        /// 进对象池之前需要做的事
        /// </summary>
        public void Push();
    }

    /// <summary>
    /// 该接口 作为 格子对象 必须继承的类 它用于实现初始化格子的方法
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IItemBase<T>
    {
        void InitInfo(T info);
    }

    /// <summary>
    /// 自定义sv类 用于节约性能 通过缓存池创建复用对象
    /// </summary>
    /// <typeparam name="T">代表的 数据来源类</typeparam>
    /// <typeparam name="K">代表的 格子类</typeparam>
    public class CustomSV<T, K> where K : Component, IPool, IItemBase<T>
    {
        #region 微型对象池
        private List<K> itemData = new List<K>();
        public K Get()
        {
            K kTemp;
            if (itemData.Count > 0)
            {
                kTemp = itemData[0];
                kTemp.Get();
                itemData.RemoveAt(0);
            }
            else
            {
                kTemp = Object.Instantiate(kitemRes);
                kTemp.Get();
            }
            return kTemp;
        }
        public void Push(K k)
        {
            itemData.Add(k);
            k.Push();
        }
        #endregion

        //履带对象  需要通过他得到可视范围的位置  还要把动态创建的格子设置为他的子对象
        private RectTransform content;

        //可使范围高
        private int viewPortH;

        //当前现实着的格子对象
        private Dictionary<int, K> nowShowItems = new Dictionary<int, K>();

        //数据来源
        private List<T> items;

        //记录上一次显示的索引范围
        private int oldMinIndex = -1;
        private int oldMaxIndex = -1;

        //格子的间隔宽高
        private int itemW;
        private int itemH;

        /// <summary>
        /// 格子的列数
        /// </summary>
        private int col;

        /// <summary>
        /// 需要加载的资源
        /// </summary>
        private K kitemRes;


        #region 初始化
        public CustomSV(K k)
        {
            itemData = new List<K>();
            kitemRes = k;
        }

        /// <summary>
        /// 初始化Content父对象 以及 我们可视范围的高
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="h"></param>
        public void InitContentAndSVH(RectTransform trans, int h)
        {
            this.content = trans;
            this.viewPortH = h;
        }

        /// <summary>
        /// 初始化数据来源 并且把content的高初始化
        /// </summary>
        /// <param name="items"></param>
        public void InitInfos(List<T> items)
        {
            this.items = items;
            content.sizeDelta = Vector3.zero;
            //应该要初始化履带的长度content的高
            content.sizeDelta = new Vector2(0, Mathf.CeilToInt(items.Count / (float)col) * itemH);
            Debug.Log($"当前格子数量{items.Count},每个行{col}," +
                $"计算得出{Mathf.CeilToInt(items.Count / (float)col)}列");

            //Debug.LogError(Math.Ceiling(items.Count / (float)col));//3
        }

        /// <summary>
        /// 初始化格子间隔大小 以及 一行几列
        /// </summary>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <param name="col"></param>
        public void InitItemSizeAndCol(int w, int h, int col)
        {
            this.itemW = w;
            this.itemH = h;
            this.col = col;
        }
        #endregion


        /// <summary>
        /// 更新格子显示的方法
        /// </summary>
        public void CheckShowOrHide()
        {
            //检测哪些格子应该显示出来
            int minIndex = (int)(content.anchoredPosition.y / itemH) * col;
            int maxIndex = (int)((content.anchoredPosition.y + viewPortH) / itemH) * col + col - 1;

            //最小值判断
            if (minIndex < 0)
                minIndex = 0;

            //超出道具最大数量
            if (maxIndex >= items.Count)
                maxIndex = items.Count - 1;

            if (minIndex != oldMinIndex || maxIndex != oldMaxIndex)
            {
                //在记录当前索引之前 要做一些事儿
                //根据上一次索引和这一次新算出来的索引 用来判断 哪些该移除
                //删除上一节溢出
                for (int i = oldMinIndex; i < minIndex; ++i)
                {
                    if (nowShowItems.ContainsKey(i))
                    {
                        if (nowShowItems[i] != null)
                            Push(nowShowItems[i]);
                        nowShowItems.Remove(i);
                    }
                }

                //删除下一节溢出
                for (int i = maxIndex + 1; i <= oldMaxIndex; ++i)
                {
                    if (nowShowItems.ContainsKey(i))
                    {
                        if (nowShowItems[i] != null)
                            Push(nowShowItems[i]);
                        nowShowItems.Remove(i);
                    }
                }
            }

            oldMinIndex = minIndex;
            oldMaxIndex = maxIndex;

            //创建指定索引范围内的格子
            for (int i = minIndex; i <= maxIndex; ++i)
            {
                if (nowShowItems.ContainsKey(i))
                    continue;

                //根据这个关键索引 用来设置位置 初始化道具信息
                int index = i;
                nowShowItems.Add(index, null);

                K bagItem1 = Get();
                bagItem1.transform.SetParent(content);
                bagItem1.transform.localScale = Vector3.one;
                bagItem1.transform.localPosition = new Vector3((index % col) * itemW, -index / col * itemH, 0);
                bagItem1.InitInfo(items[index]);
                if (nowShowItems.ContainsKey(index))
                    nowShowItems[index] = bagItem1;
                else
                    Push(bagItem1);
            }
        }
    }


}