using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityState : PlayerState
{

    // 1 IdleState (GroundedState)
    // 2 InAirState (State)
    
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
    // Check
    protected bool IsGrounded;
    protected bool IsTouchingWall;
    // Ability Check
    protected bool IsAbilityDone;

    #endregion

    #region w/ State Workflow

    public override void DoCheck()
    {
        base.DoCheck();

        if (CollisionSenses)
        {
            IsGrounded = CollisionSenses.Ground;
            IsTouchingWall = CollisionSenses.WallFront;
        }
    }

    public override void Enter()
    {
        base.Enter();
        IsAbilityDone = false;
    }
    
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        XInput = Player.InputHandler.NormalizedXInput;

        if (IsAbilityDone)
        {
            if (IsGrounded && Movement.CurrentVelocity.y < 0.01f)
            {
                // Idle
                StateMachine.ChangeState(typeof(PlayerIdleState));
            }
            else
            {
                // InAir
                StateMachine.ChangeState(typeof(PlayerInAirState));
            }    
        }
    }

    #endregion

}
