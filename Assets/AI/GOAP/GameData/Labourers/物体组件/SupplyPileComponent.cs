using UnityEngine;

/// <summary>
/// 供应桩组件
/// </summary>
public class SupplyPileComponent : MonoBehaviour
{
    /// <summary>
    /// 挖矿和砍木头
    /// for mining ore and chopping logs
    /// 工具数量
    /// </summary>
    [Header("工具数量")] public int numTools;

    /// <summary>
    /// 让柴火
    /// makes firewood
    /// 原木数量
    /// </summary>
    [Header("原木数量")]public int numLogs;

    /// <summary>
    /// 我们想做什么
    /// what we want to make
    /// 柴火数量
    /// </summary>
    [Header("柴火数量")] public int numFirewood;

    /// <summary>
    /// 制作工具
    /// makes tools
    /// 矿石数量
    /// </summary>
    [Header("矿石数量")] public int numOre;
}