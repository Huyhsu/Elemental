using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : ScriptableObject
{
    #region w/ Skill Data

    [SerializeField] protected SkillData skillData;

    public SkillData SkillData
    {
        get => skillData;
        private set => skillData = value;
    }

    #endregion

    #region w/ Components

    public PlayerCastState State { get; private set; }

    protected Core Core;

    protected Animator BaseAnimator;
    protected GameObject BaseGameObject;

    #endregion

    #region w/ Skill

    public void InitializeSkill(PlayerCastState state, Player player)// 取得 cast state 跟 core 以便引用
    {
        State = state;
        Core = player.Core;
        BaseGameObject = player.SkillGameObject;
        BaseAnimator = player.SkillAnimator;
        BaseGameObject.SetActive(false);
    }

    #endregion

    #region w/ Skill Workflow

    public virtual void EnterSkill()// 進入
    {
        BaseGameObject.SetActive(true);
        BaseAnimator.SetBool("cast", true);
    }

    public virtual void LogicUpdateSkill() { }// 邏輯更新

    public virtual void ExitSkill()// 結束
    {
        BaseAnimator.SetBool("cast", false);
        BaseGameObject.SetActive(false);
    }
    
    #endregion

    #region w/ Animation Trigger Functions
    // 基於動畫當前幀，調用特定 method
    public virtual void AnimationFinishTrigger()// 動畫結束 => IsAbilityDone = true
    {
        State.AnimationFinishTrigger();
    }

    public virtual void AnimationTurnOffFlipTrigger()// 是否能翻轉 = false
    {
        State.SetPlayerFlip(false);
    }
    
    public virtual void AnimationTurnOnFlipTrigger()// 是否能翻轉 = true
    {
        State.SetPlayerFlip(true);
    }

    public virtual void AnimationStopMovementTrigger()// 停止移動
    {
        State.SetPlayerVelocity(0f);
    }

    public virtual void AnimationStartMovementTrigger()// 根據當前的攻擊次數來變更 x velocity 以移動
    {
        // State.SetPlayerVelocity(WeaponData.MovementSpeed[AttackCounter]);
    }
    
    public virtual void AnimationActionTrigger() { }// 根據動作需求實現
    
    #endregion
}