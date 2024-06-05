using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// Goap代理人->可以看成角色
/// </summary>
public sealed class GoapAgent : MonoBehaviour
{
    /// <summary>
    /// 状态机
    /// </summary>
    private FSM stateMachine;

    private FSM.FSMState idleState; // 找点事情做：寻找一些活动或任务来打发时间或消磨无聊的时光。
    private FSM.FSMState moveToState; // 移动到目标
    private FSM.FSMState performActionState; // 执行一个动作：指进行某项任务或活动，采取必要的步骤以完成特定的行动。

    /// <summary>
    /// 可用操作：指在特定情境下可以执行的可选操作或行动。
    /// </summary>
    private HashSet<GoapAction> availableActions;

    /// <summary>
    /// 当前的行为
    /// </summary>
    private Queue<GoapAction> currentActions;

    /// <summary>
    /// 这是提供世界数据并听取计划反馈的实现类
    /// this is the implementing class that provides our world data and listens to feedback on planning
    /// </summary>
    private IGoap dataProvider;

    /// <summary>
    /// 计划者
    /// </summary>
    private GoapPlanner planner;

    void Start()
    {
        stateMachine = new FSM();
        availableActions = new HashSet<GoapAction>();
        currentActions = new Queue<GoapAction>();
        planner = new GoapPlanner(); //创建规划
        findDataProvider(); //查找职业
        createIdleState(); //创建等待状态
        createMoveToState(); //创建移动状态
        createPerformActionState(); //创建执行一个动作状态
        stateMachine.pushState(idleState); //推入状态
        loadActions(); //加载行动
    }

    void Update() => stateMachine.Update(this.gameObject);

    public GoapAction getAction(Type action)
    {
        foreach (GoapAction g in availableActions)
        {
            if (g.GetType() != action) continue;
            return g;
        }

        return null;
    }

    public void RemoveAction(GoapAction action) => availableActions.Remove(action);
    public void AddAction(GoapAction action) => availableActions.Add(action);

    private bool hasActionPlan()
    {
        return currentActions.Count > 0;
    }

    /// <summary>
    /// 创建Idle状态
    /// </summary>
    private void createIdleState()
    {
        idleState = (fsm, gameObj) =>
        {
            // GOAP planning GOAP规划

            // get the world state and the goal we want to plan for
            //获得我们想要计划的世界状态和目标
            HashSet<KeyValuePair<string, object>> worldState = dataProvider.getWorldState();
            HashSet<KeyValuePair<string, object>> goal = dataProvider.createGoalState();

            // Plan 计划  规划行动
            //伐木工  可以选择的行动  手中有的东西  需要做的事情
            Queue<GoapAction> plan = planner.plan(gameObject, availableActions, worldState, goal);
            if (plan != null)
            {
                // we have a plan, hooray! 我们有个计划，万岁!
                currentActions = plan;
                dataProvider.planFound(goal, plan);

                fsm.popState(); // move to PerformAction state 移动到PerformAction状态
                fsm.pushState(performActionState);
            }
            else
            {
                // ugh, we couldn't get a plan 呃，我们没有计划
                Debug.Log("<color=orange>Failed Plan 计划失败:</color>" + prettyPrint(goal));
                dataProvider.planFailed(goal);
                fsm.popState(); // move back to IdleAction state
                fsm.pushState(idleState);
            }
        };
    }

    /// <summary>
    /// 创建移动状态
    /// </summary>
    private void createMoveToState()
    {
        moveToState = (fsm, gameObj) =>
        {
            // move the game object

            GoapAction action = currentActions.Peek(); //队列的头部元素，即最早被添加到队列中的元素，而不会将其从队列中移除
            if (action.requiresInRange() && action.target == null)
            {
                Debug.Log(
                    "<color=red>Fatal error:</color> Action requires a target but has none. " +
                    "Planning failed. You did not assign the target in your Action.checkProceduralPrecondition()");
                fsm.popState(); // move
                fsm.popState(); // perform
                fsm.pushState(idleState);
                return;
            }

            // get the agent to move itself 让智能体自己移动
            if (!dataProvider.moveAgent(action)) return;
            fsm.popState();

            /*MovableComponent movable = (MovableComponent) gameObj.GetComponent(typeof(MovableComponent));
            if (movable == null) {
                Debug.Log("<color=red>Fatal error:</color> Trying to move an Agent that doesn't have a MovableComponent. Please give it one.");
                fsm.popState(); // move
                fsm.popState(); // perform
                fsm.pushState(idleState);
                return;
            }

            float step = movable.moveSpeed * Time.deltaTime;
            gameObj.transform.position = Vector3.MoveTowards(gameObj.transform.position, action.target.transform.position, step);

            if (gameObj.transform.position.Equals(action.target.transform.position) ) {
                // we are at the target location, we are done
                action.setInRange(true);
                fsm.popState();
            }*/
        };
    }


    /// <summary>
    /// 创建执行一个动作
    /// </summary>
    private void createPerformActionState()
    {
        performActionState = (fsm, gameObj) =>
        {
            // perform the action

            if (!hasActionPlan())
            {
                // no actions to perform
                Debug.Log("<color=red>Done actions</color>");
                fsm.popState();
                fsm.pushState(idleState);
                dataProvider.actionsFinished();
                return;
            }

            GoapAction action = currentActions.Peek();
            if (action.isDone())
            {
                // the action is done. Remove it so we can perform the next one
                //操作完成。删除它，以便我们可以执行下一个
                currentActions.Dequeue();
            }

            if (hasActionPlan())
            {
                // perform the next action
                //执行下一个操作
                action = currentActions.Peek();
                bool inRange = action.requiresInRange() ? action.isInRange() : true;

                if (inRange)
                {
                    // we are in range, so perform the action
                    //我们在射程内，所以执行动作
                    bool success = action.perform(gameObj);

                    if (!success)
                    {
                        // action failed, we need to plan again
                        //行动失败了，我们需要重新计划
                        fsm.popState();
                        fsm.pushState(idleState);
                        dataProvider.planAborted(action);
                    }
                }
                else
                {
                    // we need to move there first 我们得先搬过去
                    // push moveTo state push moveTo状态
                    fsm.pushState(moveToState);
                }
            }
            else
            {
                // no actions left, move to Plan state 没有动作，移动到计划状态
                fsm.popState();
                fsm.pushState(idleState);
                dataProvider.actionsFinished();
            }
        };
    }

    /// <summary>
    /// 查找数据提供程序
    /// </summary>
    private void findDataProvider()
    {
        foreach (Component comp in gameObject.GetComponents(typeof(Component)))
        {
            if (typeof(IGoap).IsAssignableFrom(comp.GetType()))
            {
                dataProvider = (IGoap)comp;
                return;
            }
        }
    }

    /// <summary>
    /// 加载行动
    /// </summary>
    private void loadActions()
    {
        GoapAction[] actions = gameObject.GetComponents<GoapAction>();
        foreach (GoapAction a in actions)
            AddAction(a);
        Debug.Log($"{gameObject.name} -> Found actions 找到行动: " + prettyPrint(actions));
    }

    /// <summary>
    /// 输出打印
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    public static string prettyPrint(HashSet<KeyValuePair<string, object>> state)
    {
        String s = "";
        foreach (KeyValuePair<string, object> kvp in state)
        {
            s += kvp.Key + ":" + kvp.Value.ToString();
            s += ", ";
        }

        return s;
    }

    /// <summary>
    /// 输出打印
    /// </summary>
    /// <param name="actions"></param>
    /// <returns></returns>
    public static string prettyPrint(Queue<GoapAction> actions)
    {
        String s = "";
        foreach (GoapAction a in actions)
        {
            s += a.GetType().Name;
            s += "-> ";
        }

        s += "GOAL";
        return s;
    }

    /// <summary>
    /// 输出打印
    /// </summary>
    /// <param name="actions"></param>
    /// <returns></returns>
    public static string prettyPrint(GoapAction[] actions)
    {
        String s = "";
        foreach (GoapAction a in actions)
        {
            s += a.GetType().Name;
            s += ", ";
        }

        return s;
    }

    /// <summary>
    /// 输出打印
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public static string prettyPrint(GoapAction action)
    {
        String s = "" + action.GetType().Name;
        return s;
    }
}