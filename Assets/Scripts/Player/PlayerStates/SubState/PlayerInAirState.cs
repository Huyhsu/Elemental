using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new_InAirState", menuName = "State/Player State/InAir State")]
public class PlayerInAirState : PlayerState
{
    // 1 MoveState (GroundedState)
    // 2 IdleState (GroundedState)
    // 3 JumpState (AbilityState)

    #region w/ Constructor

    private PlayerJumpState _jumpState;

    public override void Initialize(Player player)
    {
        base.Initialize(player);
        AnimationBoolName = "inAir";
        
        foreach (var state in Player.States)
        {
            if (state.GetType() == typeof(PlayerJumpState))
            {
                _jumpState = (PlayerJumpState)state;
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
    private int _xInput;
    private bool _jumpInput;
    private bool _jumpInputStop;
    // Check
    private bool _isGrounded;
    private bool _isTouchingWall;
    private bool _isTouchingLedge;

    #endregion
    
    #region w/ Jump
    
    private bool _isJumping;
    private bool _isJumpCoyoteTime;
    public void SetIsJumping() => _isJumping = true;// 由 JumpState 來設定是否在跳躍
    public void StartJumpCoyoteTime() => _isJumpCoyoteTime = true;// 開始計算郊狼時間
    public void EndJumpCoyoteTime() => _isJumpCoyoteTime = false;// 結束計算郊狼時間
    private void CheckJumpMultiplier()// 若正在跳躍，根據 JumpInput 是否停止來實現不同跳躍高度
    {
        if (!_isJumping) return;
        
        if (_jumpInputStop)
        {
            Movement.SetVelocityY(Movement.CurrentVelocity.y * PlayerData.variableJumpHeightMultiplier);
            _isJumping = false;
        }
        else if (Movement.CurrentVelocity.y < 0.01f)
        {
            _isJumping = false;
        }
    }
    private void CheckJumpCoyoteTime()// 確認郊狼時間，若超過設定時間，則減少跳躍次數
    {
        if (_isJumpCoyoteTime && Time.time >= StartTime + PlayerData.coyoteTime)
        {
            _isJumpCoyoteTime = false;
            _jumpState.DecreaseAmountOfJumpsLeft();
        }
    }

    #endregion

    #region w/ State Workflow

    public override void DoCheck()
    {
        base.DoCheck();

        if (CollisionSenses)
        {
            _isGrounded = CollisionSenses.Ground;
            _isTouchingWall = CollisionSenses.WallFront;
            _isTouchingLedge = CollisionSenses.LedgeHorizontal;
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        _xInput = Player.InputHandler.NormalizedXInput;
        _jumpInput = Player.InputHandler.JumpInput;
        _jumpInputStop = Player.InputHandler.JumpInputStop;

        CheckJumpCoyoteTime();// 郊狼時間
        CheckJumpMultiplier();// 不同跳躍高度

        if (_isGrounded && Movement.CurrentVelocity.y < 0.01f && _xInput != 0)
        {
            // Move
            StateMachine.ChangeState(typeof(PlayerMoveState));
        }
        else if (_isGrounded && Movement.CurrentVelocity.y < 0.01f)
        {
            // Idle
            StateMachine.ChangeState(typeof(PlayerIdleState));
        }
        else if (_jumpInput && _jumpState.CanJump)
        {
            // Jump
            StateMachine.ChangeState(typeof(PlayerJumpState));
        }
        else
        {
            Movement.SetVelocityX(_xInput * PlayerData.movementVelocity);
            Movement.CheckIfShouldFlip(_xInput);
        
            Player.Animator.SetFloat("yVelocity", Movement.CurrentVelocity.y);// Set up Jump/Fall Animation
        }
    }

    #endregion
}
