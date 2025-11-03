using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ImageComponent : MonoBehaviour
{
    public uint id;
    public Image image;
    public string key;

    private void Awake() => ImageManager.Add(this);
    public void Refresh() => image.sprite = ImageManager.GetSprite(id);
}