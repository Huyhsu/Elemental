using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCastState : PlayerAbilityState
{
    #region w/ Variables

    private float _velocityToSet;

    private bool _shouldSetVelocity;
    private bool _shouldCheckFlip;

    #endregion

    #region w/ Skill

    private Skill _skill;

    public void SetSkill(Skill skill)// 初始化設置，傳入 state 跟 core
    {
        _skill = skill;
        _skill.InitializeSkill(this, Player);
    }

    public void SetPlayerVelocity(float velocity)// 設定 x velocity
    {
        _velocityToSet = velocity;
        _shouldSetVelocity = true;
    }

    public void SetPlayerFlip(bool value)// 根據布林值判斷是否要 flip
    {
        _shouldCheckFlip = value;
    }
    
    #endregion

    #region w/ State Workflow
    
    public override void Enter()// 進入 (根據 skill )
    {
        base.Enter();
        _skill.EnterSkill();

        _shouldSetVelocity = false;
    }

    public override void LogicUpdate()// 邏輯更新 (根據 skill )
    {
        base.LogicUpdate();
        _skill.LogicUpdateSkill();

        // 動作判斷
        if (_shouldCheckFlip)// 是否要 flip
        {
            Movement.CheckIfShouldFlip(XInput);
        }
        if (_shouldSetVelocity)// 是否能變更 x velocity
        {
            Movement.SetVelocityX(_velocityToSet * Movement.FacingDirection);
        }
    }

    public override void Exit()// 結束 (根據 skill )
    {
        base.Exit();
        _skill.ExitSkill();
    }

    #endregion

    #region w/ Animation Trigger Functions
    
    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        IsAbilityDone = true;
    }

    #endregion
}
