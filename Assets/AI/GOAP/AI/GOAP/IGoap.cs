using UnityEngine;
using System.Collections;

/**
 * Collect the world data for this Agent that will be
 * used for GOAP planning.
 */
using System.Collections.Generic;


/**
 * Any agent that wants to use GOAP must implement
 * this interface. It provides information to the GOAP
 * planner so it can plan what actions to use.
 *
 * It also provides an interface for the planner to give
 * feedback to the Agent and report success/failure.
 * 任何想要使用GOAP的代理都必须实现
 * 此接口。它向GOAP提供信息
 * 计划，所以它可以计划什么行动使用。
 * 它还为规划人员提供了一个界面
 * 反馈给座席并报告成功/失败。
 */
public interface IGoap
{
    /**
     * The starting state of the Agent and the world.
     * Supply what states are needed for actions to run.
     * Agent和世界的起始状态。
     * 提供操作运行所需的状态。
     */
    HashSet<KeyValuePair<string,object>> getWorldState ();

    /**
     * Give the planner a new goal so it can figure out
     * the actions needed to fulfill it.
     * 给计划者一个新目标，这样他们就能弄清楚
     * 实现目标所需的行动。
     */
    HashSet<KeyValuePair<string,object>> createGoalState ();

    /**
     * No sequence of actions could be found for the supplied goal.
     * You will need to try another goal
     * 无法找到提供目标的动作序列。
     * 你需要尝试另一个目标
     */
    void planFailed (HashSet<KeyValuePair<string,object>> failedGoal);

    /**
     * A plan was found for the supplied goal.
     * These are the actions the Agent will perform, in order.
     * 为提供的目标找到了一个计划。
     * 这些是代理将按顺序执行的操作。
     */
    void planFound (HashSet<KeyValuePair<string,object>> goal, Queue<GoapAction> actions);

    /**
     * All actions are complete and the goal was reached. Hooray!
     * 所有的行动都完成了，目标达到了。万岁!
     */
    void actionsFinished ();

    /**
     * One of the actions caused the plan to abort.
     * That action is returned.
     * 其中一个行动导致计划中止。
     * 该操作被返回。
     */
    void planAborted (GoapAction aborter);

    /**
     * Called during Update. Move the agent towards the target in order
     * for the next action to be able to perform.
     * Return true if the Agent is at the target and the next action can perform.
     * False if it is not there yet.
     * 在更新期间调用。将代理按顺序移动到目标
     * 以便能够执行下一个动作。
     * 如果Agent在目标位置并且可以执行下一个动作，则返回true。
     * 如果还没有，则为False。
     */
    bool moveAgent(GoapAction nextAction);
}