using System;
using System.Collections.Generic;

/// <summary>
/// 检查管理器
/// </summary>
public class CheckManager
{
    private static CheckManager _i;
    public static CheckManager I => _i ??= new CheckManager();

    private readonly Dictionary<Enum, bool> _checkDic = new();
    public bool Check(Enum arg1, bool arg2 = false) => _checkDic.TryAdd(@arg1, arg2) ? arg2 : _checkDic[@arg1];
}