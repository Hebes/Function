using System.Collections.Generic;
using UnityEngine;

/**
 * Stack-based Finite State Machine.基于堆栈的有限状态机。
 * Push and pop states to the FSM.向FSM推送和弹出状态。
 *
 * States should push other states onto the stack状态应该将其他状态推入堆栈
 * and pop themselves off.然后跳下去。
 */


public class FSM {

    private Stack<FSMState> stateStack = new Stack<FSMState> ();

    public delegate void FSMState (FSM fsm, GameObject gameObject);
	

    public void Update (GameObject gameObject) {
        if (stateStack.Peek() != null)
            stateStack.Peek().Invoke (this, gameObject);
    }

    public void pushState(FSMState state) {
        stateStack.Push (state);
    }

    public void popState() {
        stateStack.Pop ();
    }
}