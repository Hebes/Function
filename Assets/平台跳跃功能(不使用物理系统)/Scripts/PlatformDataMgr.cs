using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace 平台跳跃功能
{
    /// <summary>
    /// 平台数据管理器 主要用于一次性获取数据 避免之后再多次单独获取
    /// </summary>
    public class PlatformDataMgr
    {
        private static PlatformDataMgr instance;

        public static PlatformDataMgr Instance => instance ??= new PlatformDataMgr();

        public List<Platform> platformList = new List<Platform>();

        /// <summary>
        /// 动态添加平台数据
        /// </summary>
        /// <param name="data"></param>
        public void AddPlatform(Platform data) => platformList.Add(data);

        /// <summary>
        /// 动态移除平台数据
        /// </summary>
        /// <param name="data"></param>
        public void RemovePlatform(Platform data)
        {
            if (platformList.Contains(data))
                platformList.Remove(data);
        }

        /// <summary>
        /// 清空平台数据
        /// </summary>
        public void ClearData() => platformList.Clear();
    }
}