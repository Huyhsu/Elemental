using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new_MoveState", menuName = "State/Player State/Move State")]
public class PlayerMoveState : PlayerGroundedState
{
    // 1 IdleState
    
    public override void Initialize(Player player)
    {
        base.Initialize(player);
        AnimationBoolName = "move";
    }

    #region w/ State Workflow
    
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (IsExitingState) return;

        Movement.CheckIfShouldFlip(XInput);
        Movement.SetVelocityAccelerationX(XInput * PlayerData.movementVelocity);
        
        if (XInput == 0 && Mathf.Abs(Movement.CurrentVelocity.x) <= 0.01f)
        {
            // Idle
            StateMachine.ChangeState(typeof(PlayerIdleState));
        }
    }

    #endregion
}
