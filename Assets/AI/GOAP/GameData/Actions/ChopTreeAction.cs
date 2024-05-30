
using System;
using UnityEngine;

/// <summary>
/// 砍树动作
/// </summary>
public class ChopTreeAction : GoapAction
{
	/// <summary>
	/// 砍
	/// </summary>
	private bool chopped = false;
	
	/// <summary>
	/// 目标树
	/// where we get the logs from
	/// </summary>
	private TreeComponent targetTree; 
	
	private float startTime = 0;
	
	/// <summary>
	/// 工作时间/秒
	/// </summary>
	public float workDuration = 2; // seconds
	
	public ChopTreeAction () {
		addPrecondition ("hasTool", true); // we need a tool to do this 我们需要一个工具来做到这一点
		addPrecondition ("hasLogs", false); // if we have logs we don't want more 如果我们有原木，我们不想要更多
		addEffect ("hasLogs", true);//影响-> 获取原木
	}
	
	
	public override void reset ()
	{
		chopped = false;
		targetTree = null;
		startTime = 0;
	}
	
	public override bool isDone ()
	{
		return chopped;
	}
	
	public override bool requiresInRange ()
	{
		//是的，我们需要靠近一棵树
		// yes we need to be near a tree
		return true; 
	}
	
	public override bool checkProceduralPrecondition (GameObject agent)
	{
		// find the nearest tree that we can chop
		TreeComponent[] trees = (TreeComponent[]) UnityEngine.GameObject.FindObjectsOfType ( typeof(TreeComponent) );
		TreeComponent closest = null;
		float closestDist = 0;
		
		foreach (TreeComponent tree in trees) {
			if (closest == null) {
				// first one, so choose it for now
				//第一个，所以现在选择它
				closest = tree;
				closestDist = (tree.gameObject.transform.position - agent.transform.position).magnitude;
			} else {
				// is this one closer than the last?
				//这次比上次近吗?
				float dist = (tree.gameObject.transform.position - agent.transform.position).magnitude;
				if (dist < closestDist) {
					// we found a closer one, use it
					//我们找到了一个近一点的，用它
					closest = tree;
					closestDist = dist;
				}
			}
		}
		if (closest == null)
			return false;

		targetTree = closest;
		target = targetTree.gameObject;
		
		return closest != null;
	}
	
	public override bool perform (GameObject agent)
	{
		if (startTime == 0)
			startTime = Time.time;
		
		if (Time.time - startTime > workDuration) {
			// finished chopping
			BackpackComponent backpack = (BackpackComponent)agent.GetComponent(typeof(BackpackComponent));
			backpack.numLogs += 1;
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