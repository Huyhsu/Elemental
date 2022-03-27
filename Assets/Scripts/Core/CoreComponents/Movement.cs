using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : CoreComponent
{
    public Rigidbody2D Rigidbody2D { get; private set; }
    public int FacingDirection { get; private set; }
    public bool CanSetVelocity { get; private set; }
    public Vector2 CurrentVelocity { get; private set; }
    
    private Vector2 _workspace;

    protected override void Awake()
    {
        base.Awake();
        Rigidbody2D = GetComponentInParent<Rigidbody2D>();
        FacingDirection = 1;
        CanSetVelocity = true;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        CurrentVelocity = Rigidbody2D.velocity;// 更新當前速度
    }

    #region w/ Flip Functions
    // 翻轉
    public void Flip()
    {
        FacingDirection *= -1;
        Rigidbody2D.transform.Rotate(0.0f,180.0f,0.0f);
    }

    public void CheckIfShouldFlip(int xInput)
    {
        if (xInput != 0 && xInput != FacingDirection)
        {
            Flip();
        }
    }
    
    #endregion
    
    #region w/ Set Velocity Functions
    // 設置速度
    public void SetVelocity(float velocity, Vector2 angle, int direction)
    {
        angle.Normalize();
        _workspace.Set(angle.x * velocity * direction, angle.y * velocity);
        SetFinalVelocity();
    }
    
    public void SetVelocityZero()
    {
        _workspace = Vector2.zero;
        SetFinalVelocity();
    }

    public void SetVelocityX(float velocityX)
    {
        _workspace.Set(velocityX, CurrentVelocity.y);
        SetFinalVelocity();
    }

    public void SetVelocityXZeroY(float velocityX)
    {
        _workspace.Set(velocityX, 0);
        SetFinalVelocity();
    }
    
    public void SetVelocityY(float velocityY)
    {
        _workspace.Set(CurrentVelocity.x, velocityY);
        SetFinalVelocity();
    }
    
    private void SetFinalVelocity()
    {
        if (!CanSetVelocity) return;
        Rigidbody2D.velocity = _workspace;
        CurrentVelocity = _workspace;
    }
    
    #endregion
}
