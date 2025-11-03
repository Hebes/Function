using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 多语言组件
/// </summary>
[DisallowMultipleComponent]
[RequireComponent(typeof(Text))]
public class LanguageComponent : MonoBehaviour, ILanguage
{
    public uint key;

    public uint Key => key;
    public LanguageData LanguageData { get; set; }
    public Text text;

    private void Awake() => ILanguage.Register(key, this);
    public void SetText() => text.text = ILanguage.GetLanguageData(key).GetValue();
    private void OnDestroy() => ILanguage.UnRegister(key);
}