using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    // 1 JumpState (AbilityState)
    // 2 InAirState (State)

    #region w/ Constructor

    private PlayerJumpState _jumpState;
    private PlayerInAirState _inAirState;
    
    public override void Initialize(Player player)
    {
        base.Initialize(player);
        
        foreach (var state in Player.States)
        {
            if (state.GetType() == typeof(PlayerJumpState))
            {
                _jumpState = (PlayerJumpState)state;
            }
            else if (state.GetType() == typeof(PlayerInAirState))
            {
                _inAirState = (PlayerInAirState)state;
            }
        }
    }

    #endregion
    
    #region w/ Core Components
    // Move
    protected Movement Movement => _movement ? _movement : Core.GetCoreComponent(ref _movement);
    private Movement _movement;
    // CollisionSenses
    protected CollisionSenses CollisionSenses => _collisionSenses ? _collisionSenses : Core.GetCoreComponent(ref _collisionSenses);
    private CollisionSenses _collisionSenses;
    
    #endregion
    
    #region w/ Variables

    // Input
    protected int XInput;
    protected bool JumpInput;
    // Check
    protected bool IsGrounded;
    protected bool IsTouchingWall;
    protected bool IsTouchingLedge;
    protected bool IsTouchingCeiling;
    // State Check

    #endregion

    #region w/ State Workflow

    public override void DoCheck()
    {
        base.DoCheck();

        if (CollisionSenses)
        {
            IsGrounded = CollisionSenses.Ground;
            IsTouchingWall = CollisionSenses.WallFront;
            IsTouchingLedge = CollisionSenses.LedgeHorizontal;
            IsTouchingCeiling = CollisionSenses.Ceiling;
        }
    }

    public override void Enter()
    {
        base.Enter();
        _jumpState.ResetAmountOfJumpsLeft();// 重設跳躍次數
    }
    
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        XInput = InputHandler.NormalizedXInput;
        JumpInput = InputHandler.JumpInput;

        if (JumpInput && _jumpState.CanJump)
        {
            // Jump
            StateMachine.ChangeState(typeof(PlayerJumpState));
        }
        else if (!IsGrounded)
        {
            // InAir
            _inAirState.StartJumpCoyoteTime();// 在 InAirState 設定郊狼時間為 true
            StateMachine.ChangeState(typeof(PlayerInAirState));
        }
    }

    #endregion
}

