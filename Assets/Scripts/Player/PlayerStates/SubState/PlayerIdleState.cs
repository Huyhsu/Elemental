using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new_IdleState", menuName = "State/Player State/Idle State")]
public class PlayerIdleState : PlayerGroundedState
{
    // 1 MoveState

    #region w/ Constructor

    public override void Initialize(Player player)
    {
        base.Initialize(player);
        AnimationBoolName = "idle";
    }    

    #endregion
    
    #region w/ State Workflow

    public override void Enter()
    {
        base.Enter();
        Movement.SetVelocityX(0f);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (IsExitingState) return;

        if (XInput != 0)
        {
            // Move
            StateMachine.ChangeState(typeof(PlayerMoveState));
        }
    }

    #endregion
}
