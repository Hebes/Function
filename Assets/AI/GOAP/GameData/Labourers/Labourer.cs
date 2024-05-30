using UnityEngine;
using System.Collections.Generic;


/**
 * A general labourer class.
 * You should subclass this for specific Labourer classes and implement
 * the createGoalState() method that will populate the goal for the GOAP
 * planner.
 *
 * 一般工人阶级。
 * 你应该子类化这个特定的劳动者类和实现
 * createGoalState()方法，它将为GOAP填充目标
 * 计划。
 *
 * 劳工 职业
 */
public abstract class Labourer : MonoBehaviour, IGoap
{
    /// <summary>
    /// 资源点
    /// </summary>
    public BackpackComponent backpack;
    public float moveSpeed = 1;


    void Start()
    {
        backpack ??= gameObject.AddComponent<BackpackComponent>();
        if (backpack.tool != null) return;
        var prefab = Resources.Load<GameObject>(backpack.toolType);
        var tool = Instantiate(prefab, transform.position, transform.rotation);
        backpack.tool = tool;
        tool.transform.parent = transform; // attach the tool 连接工具
    }

    /**
     * Key-Value data that will feed the GOAP actions and system while planning.
     * 在计划时将提供给GOAP操作和系统的键值数据。
     */
    public HashSet<KeyValuePair<string, object>> getWorldState()
    {
        var worldData = new HashSet<KeyValuePair<string, object>>
        {
            new KeyValuePair<string, object>("hasOre", (backpack.numOre > 0)), //有矿
            new KeyValuePair<string, object>("hasLogs", (backpack.numLogs > 0)), //有原木
            new KeyValuePair<string, object>("hasFirewood", (backpack.numFirewood > 0)), //有柴火
            new KeyValuePair<string, object>("hasTool", (backpack.tool != null)) //有工具
        };

        return worldData;
    }

    /**
     * Implement in subclasses
     * 在子类中实现
     * 创建状态
     */
    public abstract HashSet<KeyValuePair<string, object>> createGoalState();


    /// <summary>
    /// 计划失败了
    /// </summary>
    /// <param name="failedGoal"></param>
    public void planFailed(HashSet<KeyValuePair<string, object>> failedGoal)
    {
        // Not handling this here since we are making sure our goals will always succeed.
        // But normally you want to make sure the world state has changed before running
        // the same goal again, or else it will just fail.

        //这里没有处理这个问题，因为我们要确保我们的目标总是成功。
        //但通常情况下，你希望在运行之前确保全局状态已更改
        //再次实现相同的目标，否则将会失败
    }

    /// <summary>
    /// 找寻计划
    /// </summary>
    /// <param name="goal"></param>
    /// <param name="actions"></param>
    public void planFound(HashSet<KeyValuePair<string, object>> goal, Queue<GoapAction> actions)
    {
        // Yay we found a plan for our goal 我们为我们的目标找到了一个计划
        Debug.Log("<color=green>Plan found</color> " + GoapAgent.prettyPrint(actions));
    }

    /// <summary>
    /// 行动完成
    /// </summary>
    public void actionsFinished()
    {
        // Everything is done, we completed our actions for this gool. Hooray! 一切都完成了，我们完成了这个goool的行动。万岁!
        Debug.Log("<color=blue>Actions completed 操作完成</color>");
    }

    /// <summary>
    /// 计划退出
    /// </summary>
    /// <param name="aborter"></param>
    public void planAborted(GoapAction aborter)
    {
        // An action bailed out of the plan. State has been reset to plan again.
        // Take note of what happened and make sure if you run the same goal again
        // that it can succeed.

        //一个action退出了计划。状态已重置为再次计划。
        //记下发生了什么，并确保你是否再次运行相同的目标
        //它可以成功。
        Debug.Log("<color=red>Plan Aborted</color> " + GoapAgent.prettyPrint(aborter));
    }

    /// <summary>
    /// 移动
    /// </summary>
    /// <param name="nextAction"></param>
    /// <returns></returns>
    public bool moveAgent(GoapAction nextAction)
    {
        // move towards the NextAction's target
        //朝着NextAction的目标前进
        var step = moveSpeed * Time.deltaTime;
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, nextAction.target.transform.position, step);
        if (!gameObject.transform.position.Equals(nextAction.target.transform.position)) return false;
        // we are at the target location, we are done
        //我们在目标位置，我们完成了
        nextAction.setInRange(true);
        return true;
    }
}