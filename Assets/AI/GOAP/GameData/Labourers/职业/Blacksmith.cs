using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 铁匠
/// </summary>
public class Blacksmith : Labourer
{
	/**
	 * Our only goal will ever be to make tools.
	 * The ForgeTooldAction will be able to fulfill this goal.
	 * 我们唯一的目标就是制作工具。
	 * ForgeTooldAction将能够实现这个目标。
	 */
	public override HashSet<KeyValuePair<string,object>> createGoalState () {
		HashSet<KeyValuePair<string,object>> goal = new HashSet<KeyValuePair<string,object>> ();
		
		goal.Add(new KeyValuePair<string, object>("collectTools", true ));//制造工具
		return goal;
	}
}

