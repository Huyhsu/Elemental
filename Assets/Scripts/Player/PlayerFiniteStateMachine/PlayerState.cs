using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState: ScriptableObject, IState
{
    #region w/ Components
    // 元件
    protected Player Player { get; private set; }
    
    protected Core Core { get; private set; }
    protected PlayerData PlayerData { get; private set; }
    protected PlayerStateMachine StateMachine { get; private set; }
    protected PlayerInputHandler InputHandler { get; private set; }

    // State Booleans
    protected bool IsAnimationFinished;// 動畫是否結束
    protected bool IsExitingState;// 父 state 是否已經變更了當前 state

    protected float StartTime;// state 的開始時間

    protected string AnimationBoolName;// animation 布林值參數名稱

    #endregion

    #region w/ Constructor

    public virtual void Initialize(Player player)
    {
        Player = player;
        // 根據 player 來設置對應的 core, data, state machine 與 Input handler
        Core = player.Core;
        PlayerData = player.PlayerData;
        StateMachine = player.StateMachine;
        InputHandler = player.InputHandler;
    }

    #endregion
    
    #region w/ State Workflow
    // state 工作流程
    public virtual void DoCheck() { }

    public virtual void Enter()
    {
        DoCheck();
        Player.Animator.SetBool(AnimationBoolName,true);
        StartTime = Time.time;
        IsAnimationFinished = false;
        IsExitingState = false;
    }
    
    public virtual void Exit()
    {
        Player.Animator.SetBool(AnimationBoolName, false);
        IsExitingState = true;
    }
    
    public virtual void LogicUpdate() { }
    
    public virtual void PhysicsUpdate()
    {
        DoCheck();
    }    

    #endregion

    #region w/ Animation Trigger Functions
    // 動畫觸發相關
    public virtual void AnimationTrigger() { }
    public virtual void AnimationFinishTrigger() => IsAnimationFinished = true;

    #endregion
}
