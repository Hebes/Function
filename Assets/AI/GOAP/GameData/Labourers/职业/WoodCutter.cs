using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 伐木者
/// </summary>
public class WoodCutter : Labourer
{
	/**
	 * Our only goal will ever be to chop logs.
	 * The ChopFirewoodAction will be able to fulfill this goal.
	 * 我们唯一的目标就是砍木头。
	 * ChopFirewoodAction将能够实现这个目标。
	 */
	public override HashSet<KeyValuePair<string,object>> createGoalState () {
		HashSet<KeyValuePair<string,object>> goal = new HashSet<KeyValuePair<string,object>> ();
		
		goal.Add(new KeyValuePair<string, object>("collectFirewood", true ));//收集柴火
		return goal;
	}
}

