using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 伐木工
/// </summary>
public class Logger : Labourer
{
	/**
	 * Our only goal will ever be to chop trees.
	 * The ChopTreeAction will be able to fulfill this goal.
	 * 我们唯一的目标就是砍树。
	 * ChopTreeAction将能够实现这一目标。
	 */
	public override HashSet<KeyValuePair<string,object>> createGoalState () {
		HashSet<KeyValuePair<string,object>> goal = new HashSet<KeyValuePair<string,object>> ();
		
		goal.Add(new KeyValuePair<string, object>("collectLogs", true ));//收集原木
		return goal;
	}

}

