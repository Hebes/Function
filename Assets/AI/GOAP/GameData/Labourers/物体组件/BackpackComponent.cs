using UnityEngine;
using System.Collections;

/**
 * Holds resources for the Agent.
 * 为代理保存资源。
 */
public class BackpackComponent : MonoBehaviour
{
	[Header("工具")]public GameObject tool;
	[Header("原木数量")]public int numLogs;
	[Header("木柴数量")]public int numFirewood;
	[Header("矿石数量")]public int numOre;
	[Header("工具类型")]public string toolType = "ToolAxe";
	
}

