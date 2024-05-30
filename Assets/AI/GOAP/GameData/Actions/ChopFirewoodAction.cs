
using System;
using UnityEngine;

/// <summary>
/// 劈柴行动
/// </summary>
public class ChopFirewoodAction : GoapAction
{
	/// <summary>
	/// 砍
	/// </summary>
	private bool chopped = false;
	
	/// <summary>
	///  where we chop the firewood
	/// 我们在哪里砍柴
	/// </summary>
	private ChoppingBlockComponent targetChoppingBlock; 
	
	/// <summary>
	/// 开始时间
	/// </summary>
	private float startTime = 0;
	
	/// <summary>
	/// 工作持续时间
	/// </summary>
	public float workDuration = 2; // seconds
	
	public ChopFirewoodAction () {
		addPrecondition ("hasTool", true); // we need a tool to do this 我们需要一个工具来做这件事
		addPrecondition ("hasFirewood", false); // if we have firewood we don't want more 如果我们有柴火，我们不需要更多
		addEffect ("hasFirewood", true);//有柴火
	}
	
	
	public override void reset ()
	{
		chopped = false;
		targetChoppingBlock = null;
		startTime = 0;
	}
	
	public override bool isDone ()
	{
		return chopped;
	}
	
	public override bool requiresInRange ()
	{
		// yes we need to be near a chopping block
		//是的，我们需要靠近砧板
		return true; 
	}
	
	/// <summary>
	/// 检查程序前提
	/// </summary>
	/// <param name="agent"></param>
	/// <returns></returns>
	public override bool checkProceduralPrecondition (GameObject agent)
	{
		//找个离我们最近的砧板来劈木头
		// find the nearest chopping block that we can chop our wood at
		ChoppingBlockComponent[] blocks = (ChoppingBlockComponent[]) UnityEngine.GameObject.FindObjectsOfType ( typeof(ChoppingBlockComponent) );
		ChoppingBlockComponent closest = null;
		float closestDist = 0;
		
		foreach (ChoppingBlockComponent block in blocks) {
			if (closest == null) {
				// first one, so choose it for now 第一个，所以现在选择它
				closest = block;
				closestDist = (block.gameObject.transform.position - agent.transform.position).magnitude;
			} else {
				// is this one closer than the last? 这次比上次近吗?
				float dist = (block.gameObject.transform.position - agent.transform.position).magnitude;
				if (dist < closestDist) {
					// we found a closer one, use it  我们找到了一个近一点的，用它
					closest = block;
					closestDist = dist;
				}
			}
		}
		if (closest == null)
			return false;

		targetChoppingBlock = closest;
		target = targetChoppingBlock.gameObject;
		
		return closest != null;
	}
	
	public override bool perform (GameObject agent)
	{
		if (startTime == 0)
			startTime = Time.time;
		
		if (Time.time - startTime > workDuration) {
			// finished chopping
			BackpackComponent backpack = (BackpackComponent)agent.GetComponent(typeof(BackpackComponent));
			backpack.numFirewood += 5;
			chopped = true;
			ToolComponent tool = backpack.tool.GetComponent(typeof(ToolComponent)) as ToolComponent;
			tool.use(0.34f);
			if (tool.destroyed()) {
				Destroy(backpack.tool);
				backpack.tool = null;
			}
		}
		return true;
	}
	
}

