using System;
using UnityEngine;
using UnityEngine.UI;

namespace 功能.动态图片
{
    [RequireComponent(typeof(Image))]
    public class ImageComponent : MonoBehaviour
    {
        public Image image;
        public string key;

        private void Awake()
        {
            ImageManager.Instance.Add(this);
            Change();
        }

        public void Change() => image.sprite = ImageManager.Instance.GetSprite(key) ?? image.sprite;
    }
}