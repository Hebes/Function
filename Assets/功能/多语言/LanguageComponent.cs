﻿using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.多语言
{
    /// <summary>
    /// 多语言组件
    /// </summary>
    [RequireComponent(typeof(Text))]
    public class LanguageComponent : MonoBehaviour
    {
        public Text text; //组件
        public string key; //关键字
        private LanguageManager languageManager;

        private void Awake()
        {
            languageManager = LanguageManager.Instance;
            languageManager.AddLanguageComponent(this);
            ChangeImage();
            //ChangeFont();
        }
        public void ChangeImage() => text.text = languageManager.Get(key);
        public void ChangeFont()=> text.font = languageManager.Font;

        //private void OnMouseOver() => text.text = key;
    }
}