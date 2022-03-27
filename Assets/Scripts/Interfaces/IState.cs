using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    void Enter();
    void Exit();
    void LogicUpdate();
    void PhysicsUpdate();
    void AnimationTrigger();
    void AnimationFinishTrigger();
}
