using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "多语言", menuName = "对话系统/多语言", order = 0)]
public class LanguageSO : ScriptableObject
{
    public List<LanguageData> languageList = new List<LanguageData>();
}

/// <summary>
/// 语言类型
/// </summary>
public enum LanguageType
{
    SimplifiedChinese, // 简体中
    TraditionalChinese, // 繁体中
    English, // 英
    Japanese, // 日本
    Spanish, // 西班牙
    Portuguese, // 葡萄牙
    German, // 德
    French, // 法
    Italian, // 意大利
    Korean, // 韩
    Russian, // 俄
    Polish, // 波兰
    Turkish, // 土耳其
    Arabic, // 阿拉伯
    Thai, // 泰
    Indonesian, // 印尼
    Dutch, // 荷兰
    Hindi, // 印地
    Vietnamese, // 越南
}

/// <summary>
/// 多语言接口
/// </summary>
public interface ILanguage
{
    public uint Key { get; }
    public void SetText(); // 设置多语言

    public static void Register(uint key, ILanguage language) => LanguageManager.Register(key, language);
    public static void UnRegister(uint key) => LanguageManager.UnRegister(key);
    public static LanguageData GetLanguageData(uint key) => LanguageManager.GetData(key);
}

/// <summary>
/// 多语言系统
/// </summary>
public class LanguageManager
{
    private static LanguageManager _i;
    public static LanguageManager I => _i ??= new LanguageManager();


    public static LanguageType LanguageType = LanguageType.SimplifiedChinese;
    private readonly Dictionary<uint, ILanguage> _componentDic = new(); //语言组件
    private readonly Dictionary<uint, LanguageData> _languageDic = new(); //语言数据
    private LanguageSO LanguageSo { get; set; }

    /// <summary>
    /// 设置多语言数据
    /// </summary>
    /// <returns></returns>
    public void SetData(LanguageSO languageSo)
    {
        LanguageSo = languageSo;
        foreach (LanguageData languageData in LanguageSo.languageList)
            _languageDic.Add(languageData.key, languageData);
    }

    /// <summary>
    /// 获取多语言数据
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static LanguageData GetData(uint key)
    {
        _i._languageDic.TryGetValue(key, out LanguageData value);
        return value ?? RecordUnknownData(key);
    }

    /// <summary>
    /// 注册组件
    /// </summary>
    /// <param name="key"></param>
    /// <param name="language"></param>
    public static void Register(uint key, ILanguage language)
    {
        _i._componentDic.TryAdd(key, language);
    }

    /// <summary>
    /// 移除注册组件
    /// </summary>
    /// <param name="key"></param>
    public static void UnRegister(uint key)
    {
        Dictionary<uint, ILanguage> temp = _i._componentDic;
        if (temp.ContainsKey(key))
            temp.Remove(key);
    }

    /// <summary>
    /// 切换语言类型
    /// </summary>
    /// <param name="languageMode"></param>
    public static void ChangeLanguageType(LanguageType languageMode)
    {
        LanguageType = languageMode;
        foreach (ILanguage languageComponent in _i._componentDic.Values)
            languageComponent.SetText();
    }

    /// <summary>
    /// 注册未知语言类型,记录未知数据
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    private static LanguageData RecordUnknownData(uint key)
    {
        LanguageData languageDataTemp = new LanguageData { key = key };
        _i.LanguageSo.languageList.Add(languageDataTemp);
        _i._languageDic.Add(key, languageDataTemp);
        $"未配置多语言:{key},但是添加到了SO中，请配置".LogWarning();
        return languageDataTemp;
    }
}

/// <summary>
/// 多语言数据
/// </summary>
[Serializable]
public class LanguageData
{
    public uint key;
    public string chinese = string.Empty;
    public string english = string.Empty;

    //TODO 后面自己填写
    public string GetValue()
    {
        return LanguageManager.LanguageType switch
        {
            LanguageType.SimplifiedChinese => chinese,
            LanguageType.English => english,
            _ => throw new Exception("未知语言类型")
        };
    }
}