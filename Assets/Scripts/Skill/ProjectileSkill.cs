using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSkill : Skill
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform instantiateTransform;
    
    #region w/ Skill Workflow

    public override void EnterSkill()
    {
        base.EnterSkill();
        Instantiate(projectilePrefab, instantiateTransform.position, instantiateTransform.rotation);
    }

    public override void LogicUpdateSkill()
    {
        base.LogicUpdateSkill();
    }

    public override void ExitSkill()
    {
        base.ExitSkill();
    }

    #endregion
}
