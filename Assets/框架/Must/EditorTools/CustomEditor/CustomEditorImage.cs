using UnityEditor;
using UnityEngine.UI;

#if UNITY_EDITOR

/// <summary>
/// 图片工具
/// </summary>
[CustomEditor(typeof(ImageComponent), true)]
public class CustomEditorImage : UnityEditor.Editor
{
    private void OnEnable()
    {
        ImageComponent imageComponent = (ImageComponent)target;
        string nameValue = imageComponent.name.Replace("T_", string.Empty);
        imageComponent.key = nameValue;
        imageComponent.image = imageComponent.GetComponent<Image>();
    }
}
#endif