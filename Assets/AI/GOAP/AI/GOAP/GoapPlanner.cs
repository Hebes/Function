using System;
using System.Collections.Generic;
using UnityEngine;

/**
 * Goap规划计划什么行动可以完成，以实现目标状态。
 * Plans what actions can be completed in order to fulfill a goal state.
 */
public class GoapPlanner
{
    /**
     * Plan what sequence of actions can fulfill the goal.
     * Returns null if a plan could not be found, or a list of the actions
     * that must be performed, in order, to fulfill the goal.
     * 计划实现目标的行动顺序。
     * 如果找不到计划，返回null，或者返回一个操作列表
     * 为了实现目标，必须执行这些任务。
     */
    public Queue<GoapAction> plan(GameObject agent,
        HashSet<GoapAction> availableActions, //角色的所有可以的行动
        HashSet<KeyValuePair<string, object>> worldState, //世界的状态
        HashSet<KeyValuePair<string, object>> goal) //自己的目标
    {
        //重置操作，以便我们可以重新开始
        foreach (GoapAction a in availableActions)
            a.doReset();

        //使用checkProceduralPrecondition检查哪些操作可以运行
        HashSet<GoapAction> usableActions = new HashSet<GoapAction>();
        foreach (GoapAction a in availableActions)
        {
            if (a.checkProceduralPrecondition(agent))
                usableActions.Add(a);
        }

        //我们现在有了所有可以运行的操作，存储在usableActions中

        // 构建树并记录为目标提供解决方案的叶节点。
        List<Node> leaves = new List<Node>();
        Node start = new Node(null, 0, worldState, null);
        bool success = buildGraph(start, leaves, usableActions, goal);

        if (!success)
        {
            Debug.Log("没有获取到一个计划");
            return null;
        }

        // get the cheapest leaf 获得最便宜的叶子
        Node cheapest = null;
        foreach (Node leaf in leaves)
        {
            if (cheapest == null)
                cheapest = leaf;
            else
            {
                if (leaf.runningCost < cheapest.runningCost)
                    cheapest = leaf;
            }
        }

        // get its node and work back through the parents 获取它的节点并通过父节点返回
        List<GoapAction> result = new List<GoapAction>();
        Node n = cheapest;
        while (n != null)
        {
            if (n.action != null)
            {
                result.Insert(0, n.action); // insert the action in the front 在前面插入动作
            }

            n = n.parent;
        }

        // we now have this action list in correct order 我们现在有了正确的操作列表
        Queue<GoapAction> queue = new Queue<GoapAction>();
        foreach (GoapAction a in result)
            queue.Enqueue(a);

        // hooray we have a plan! 万岁，我们有计划了!
        return queue;
    }

    /**
     * Returns true if at least one solution was found.
     * The possible paths are stored in the leaves list. Each leaf has a
     * 'runningCost' value where the lowest cost will be the best action
     * sequence.
     * 如果至少找到了一个解决方案，返回true。所有可能的路径都存储在叶子列表中。每片叶子都有一个
     * 
     *  'runningCost'值，其中最低的成本将是最佳的操作
     * 序列。
     */
    private bool buildGraph(Node parent, //节点的父节点
        ICollection<Node> leaves, //所有节点的储存list
        HashSet<GoapAction> usableActions, //可以运行的行动
        HashSet<KeyValuePair<string, object>> goal) //自己的目标,比如收集木材
    {
        //是否找到一个
        var foundOne = false;

        //遍历这个节点上的每个操作，看看是否可以在这里使用它
        foreach (var action in usableActions)
        {
            //如果父状态具有这个动作的前提条件，我们可以在这里使用它
            if (inState(action.Preconditions, parent.state))
            {
                //将操作的效果应用到父状态
                var currentState = populateState(parent.state, action.Effects);
                //Debug.Log(GoapAgent.prettyPrint(currentState));
                var node = new Node(parent, parent.runningCost + action.cost, currentState, action);

                if (inState(goal, currentState))
                {
                    // we found a solution! 我们找到了解决方案!
                    leaves.Add(node);
                    foundOne = true;
                }
                else
                {
                    // not at a solution yet, so test all the remaining actions and branch out the tree
                    //这还不是一个解决方案，所以测试所有剩余的操作并扩展树
                    var subset = actionSubset(usableActions, action);
                    var found = buildGraph(node, leaves, subset, goal);
                    if (found)
                        foundOne = true;
                }
            }
        }

        return foundOne;
    }

    /**
     * 创建除removeMe操作之外的操作子集。创建一个新的集合。
     * Create a subset of the actions excluding the removeMe one. Creates a new set.
     */
    private HashSet<GoapAction> actionSubset(HashSet<GoapAction> actions, GoapAction removeMe)
    {
        HashSet<GoapAction> subset = new HashSet<GoapAction>();
        foreach (GoapAction a in actions)
        {
            if (!a.Equals(removeMe))
                subset.Add(a);
        }

        return subset;
    }

    /**
     * 检查执行行动的条件(实例GoapAction)和自己所拥有的东西(Labourer的getWorldState)是否想匹配(只要一个不匹配，这个行动就不能执行)
     * Check that all items in 'test' are in 'state'. If just one does not match or is not there
     * then this returns false.
     */
    private bool inState(HashSet<KeyValuePair<string, object>> test, HashSet<KeyValuePair<string, object>> state)
    {
        var allMatch = true;
        foreach (var t in test)
        {
            var match = false; //是否相匹配
            foreach (var s in state)
            {
                if (!s.Equals(t)) continue;
                match = true;
                break;
            }

            if (!match)
                allMatch = false;
        }

        return allMatch;
    }

    /// <summary>
    /// 将stateChange应用到currentState将stateChange应用到currentState
    /// Apply the stateChange to the currentState 将stateChange应用到currentState
    /// </summary>
    /// <param name="currentState">自身的世界数据</param>
    /// <param name="stateChange">影响</param>
    /// <returns></returns>
    private HashSet<KeyValuePair<string, object>> populateState(HashSet<KeyValuePair<string, object>> currentState, HashSet<KeyValuePair<string, object>> stateChange)
    {
        HashSet<KeyValuePair<string, object>> state = new HashSet<KeyValuePair<string, object>>();
        // copy the KVPs over as new objects 将kvp复制为新对象
        foreach (var s in currentState)
            state.Add(new KeyValuePair<string, object>(s.Key, s.Value));

        foreach (var change in stateChange)
        {
            // if the key exists in the current state, update the Value 如果该键在当前状态中存在，则更新其值
            var exists = false;

            foreach (var s in state)
            {
                if (!s.Equals(change)) continue;
                exists = true;
                break;
            }

            if (exists)
            {
                state.RemoveWhere((KeyValuePair<string, object> kvp) => kvp.Key.Equals(change.Key));
                var updated = new KeyValuePair<string, object>(change.Key, change.Value);
                state.Add(updated);
            }
            // if it does not exist in the current state, add it 如果当前状态不存在，请添加
            else
            {
                state.Add(new KeyValuePair<string, object>(change.Key, change.Value));
            }
        }

        return state;
    }

    /**
     * 用于构建图形并保存操作的运行成本。
     * Used for building up the graph and holding the running costs of actions.
     */
    private class Node
    {
        /// <summary>
        /// 父节点
        /// </summary>
        public Node parent;

        /// <summary>
        /// 运行计划的花费
        /// </summary>
        public float runningCost;

        /// <summary>
        /// 状态->getWorldState
        /// </summary>
        public HashSet<KeyValuePair<string, object>> state;

        /// <summary>
        /// 计划
        /// </summary>
        public GoapAction action;

        public Node(Node parent, float runningCost, HashSet<KeyValuePair<string, object>> state, GoapAction action)
        {
            this.parent = parent;
            this.runningCost = runningCost;
            this.state = state;
            this.action = action;
        }
    }
}