using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 矿工
/// </summary>
public class Miner : Labourer
{
	/**
	 * Our only goal will ever be to mine ore.
	 * The MineOreAction will be able to fulfill this goal.
	 * 我们唯一的目标就是开采矿石。
	 * mineroreaction将能够实现这一目标。
	 */
	public override HashSet<KeyValuePair<string,object>> createGoalState () {
		HashSet<KeyValuePair<string,object>> goal = new HashSet<KeyValuePair<string,object>> ();
		
		goal.Add(new KeyValuePair<string, object>("collectOre", true ));//收集矿石
		return goal;
	}

}

