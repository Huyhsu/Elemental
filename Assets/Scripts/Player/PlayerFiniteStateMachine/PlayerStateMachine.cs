using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine
{
    public IState CurrentState { get; private set; }
    public Dictionary<System.Type, IState> StateTable;

    private void Initialize(IState startState)// 初始化 state
    {
        CurrentState = startState;
        CurrentState.Enter();
    }

    public void Initialize(System.Type newStateType)
    {
        Initialize(StateTable[newStateType]);
    }
    
    private void ChangeState(IState newState)// 變更 state
    {
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }

    public void ChangeState(System.Type newStateType)
    {
        ChangeState(StateTable[newStateType]);
    }
}
