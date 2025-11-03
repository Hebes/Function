using System.Collections.Generic;
using UnityEngine;


public class ImageManager
{
    private static ImageManager _i;
    public static ImageManager I => _i ??= new ImageManager();
    
    
    private static Dictionary<uint, ImageComponent> ImageDic { get; set; }
    private static Dictionary<uint, Sprite> SpriteDic { get; set; }
    
    
    public static void Add(ImageComponent imageComponent) => ImageDic.TryAdd(imageComponent.id, imageComponent);
    public static void Refresh(uint key) => ImageDic[key].Refresh();
    
    
    public static void AddSprite(uint key,Sprite sprite) => SpriteDic.TryAdd(key, sprite);
    public static Sprite GetSprite(uint key) => SpriteDic.GetValueOrDefault(key);
}