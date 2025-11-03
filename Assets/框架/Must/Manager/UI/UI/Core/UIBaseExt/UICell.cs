using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using zhaorh.UI;

// <summary>
// @Author: zrh
// @Date: 2022,12,22,17:33
// @Description:
// </summary>

namespace Game.UI
{
    /// <summary>
    /// 单元格
    /// </summary>
    public abstract class UICell : MonoBehaviour
    {
        public RectTransform rectTransform { get; private set; }
        // 单元格高度 或者宽度
        protected Vector2 size;

        // init fields
        protected virtual void Awake()
        {
            InitBtnAudio();
            rectTransform = GetComponent<RectTransform>();
            size = rectTransform.sizeDelta;
            Init();
        }

        protected virtual void InitBtnAudio()
        {
            
        }
        // 初始化
        protected virtual void Init()
        {
        }

        /// <summary>
        /// 设置类容
        /// </summary>
        public abstract void SetContent(params object[] args);

        /// <summary>
        /// 高度或者宽度
        /// </summary>
        public Vector2 Size
        {
            get { return size; }
        }
        
    }
}