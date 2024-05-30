using System.Collections.Generic;
using UnityEngine;

/**
 * Stack-based Finite State Machine.基于堆栈的有限状态机。
 * Push and pop states to the FSM.向FSM推送和弹出状态。
 *
 * States should push other states onto the stack状态应该将其他状态推入堆栈
 * and pop themselves off.然后跳下去。
 */
public class FSM
{
    private Stack<FSMState> stateStack = new Stack<FSMState>();

    public delegate void FSMState(FSM fsm, GameObject gameObject);


    public void Update(GameObject gameObject)
    {
        if (stateStack.Peek() == null) return;
        stateStack.Peek().Invoke(this, gameObject);
    }

    /// <summary>
    /// 操作用于将元素压入栈顶
    /// </summary>
    /// <param name="state"></param>
    public void pushState(FSMState state) => stateStack.Push(state);

    /// <summary>
    /// 移除并返回栈顶元素
    /// </summary>
    public void popState() => stateStack.Pop();
}