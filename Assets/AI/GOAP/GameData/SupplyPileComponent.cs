using UnityEngine;
using System.Collections;

public class SupplyPileComponent : MonoBehaviour
{
	/// <summary>
	/// 挖矿和砍木头
	/// for mining ore and chopping logs
	/// 工具数量
	/// </summary>
	public int numTools; 
	
	/// <summary>
	/// 让柴火
	/// makes firewood
	/// 原木数量
	/// </summary>
	public int numLogs;  
	
	/// <summary>
	/// 我们想做什么
	/// what we want to make
	/// 柴火数量
	/// </summary>
	public int numFirewood;  
	
	/// <summary>
	/// 制作工具
	/// makes tools
	/// 矿石数量
	/// </summary>
	public int numOre; 
}

