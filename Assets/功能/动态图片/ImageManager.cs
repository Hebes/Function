using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace 功能.动态图片
{
    public class ImageManager
    {
        public static ImageManager Instance;

        public ImageManager()
        {
            Instance = this;
        }

        private readonly Dictionary<string, ImageComponent> imageComponentDic = new();
        private readonly Dictionary<string, Sprite> spriteDic = new();

        public void Add(ImageComponent imageComponent) => imageComponentDic.TryAdd(imageComponent.name, imageComponent);

        public void UnAdd(ImageComponent imageComponent) => imageComponentDic.Remove(imageComponent.name);

        public void ChangeAllImage()
        {
            foreach (var value in imageComponentDic)
                value.Value.Change();
        }

        public Sprite GetSprite(string value)
        {
            Sprite sprite = null;

            return sprite;
        }

        //其中一种加载
        private IEnumerator LoadSpriteAsync(string spriteName)
        {
            ResourceRequest request = Resources.LoadAsync<Sprite>(spriteName);
            yield return request; // 等待异步加载完成

            Sprite sprite = request.asset as Sprite; // 获取加载的Sprite资源
            if (sprite != null)
            {
                // 使用加载的Sprite资源
                Debug.Log("Sprite loaded: " + sprite.name);
            }
            else
            {
                Debug.Log("Failed to load Sprite.");
            }
        }
    }
}