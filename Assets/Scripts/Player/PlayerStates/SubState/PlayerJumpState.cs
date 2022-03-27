using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new_JumpState", menuName = "State/Player State/Jump State")]
public class PlayerJumpState : PlayerAbilityState
{

    // Transition to InAirState

    private PlayerInAirState _inAirState;

    public override void Initialize(Player player)
    {
        base.Initialize(player);
        AnimationBoolName = "inAir";
        
        foreach (var state in Player.States)
        {
            if (state.GetType() == typeof(PlayerInAirState))
            {
                _inAirState = (PlayerInAirState)state;
            }
        }
    }
    
    #region w/ Jump

    private int _amountOfJumpsLeft;
    public bool CanJump => _amountOfJumpsLeft > 0;// 剩餘跳躍次數大於零即可跳躍
    public void ResetAmountOfJumpsLeft() => _amountOfJumpsLeft = Player.PlayerData.amountOfJumps;// 重設跳躍次數
    public void DecreaseAmountOfJumpsLeft() => _amountOfJumpsLeft--;// 減少跳躍次數

    #endregion
    
    #region w/ State Workflow

    public override void Enter()
    {
        base.Enter();
        Movement.SetVelocityY(PlayerData.jumpVelocity);
        IsAbilityDone = true;
        DecreaseAmountOfJumpsLeft();
        Player.InputHandler.UseJumpInput();// 設定 JumpInput 為 false
        _inAirState.SetIsJumping();// 在 InAirState 設定正在跳躍 _isJumping 用來實現不同的跳躍高度
        _inAirState.EndJumpCoyoteTime();// 結束 JumpCoyoteTime 的判斷，因為已經跳躍過了，這樣才能接續二段跳
    }

    #endregion
}
