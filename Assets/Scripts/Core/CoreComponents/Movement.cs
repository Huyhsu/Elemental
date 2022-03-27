using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : CoreComponent
{
    public Rigidbody2D Rigidbody2D { get; private set; }
    public int FacingDirection { get; private set; }
    public bool CanSetVelocity { get; private set; }
    public Vector2 CurrentVelocity { get; private set; }

    private float _acceleration;
    private float _deceleration;
    
    private Vector2 _workspace;
    private Vector2 _temp;

    protected override void Awake()
    {
        base.Awake();
        Rigidbody2D = GetComponentInParent<Rigidbody2D>();
        FacingDirection = 1;
        CanSetVelocity = true;
        _acceleration = 18.0f;
        _deceleration = 24.0f;
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

    public void SetVelocityAccelerationX(float velocityX)// 加速度變化 velocity x
    {
        if (velocityX > 0)// 向右移動
        {
            if (CurrentVelocity.x < velocityX)// 還沒到達目標速度
            {
                _temp.x = CurrentVelocity.x;
                if (_temp.x < 0)
                {
                    _temp.x = 0;
                }
                _temp.x += Time.deltaTime * _acceleration;
                if (_temp.x >= velocityX)
                {
                    _temp.x = velocityX;
                }
                _workspace.Set(_temp.x, CurrentVelocity.y);
            }
            else// 到達目標速度
            {
                _workspace.Set(velocityX, CurrentVelocity.y);
            }
            SetFinalVelocity();
        }
        else if(velocityX < 0)// 向左移動
        {
            if (CurrentVelocity.x > velocityX)// 還沒到達目標速度
            {
                _temp.x = CurrentVelocity.x;
                if (_temp.x > 0)
                {
                    _temp.x = 0;
                }
                _temp.x -= Time.deltaTime * _acceleration;
                if (_temp.x <= velocityX)
                {
                    _temp.x = velocityX;
                }
                _workspace.Set(_temp.x, CurrentVelocity.y);
            }
            else// 到達目標速度
            {
                _workspace.Set(velocityX, CurrentVelocity.y);
            }
            SetFinalVelocity();
        }
        else if(velocityX == 0)// 無移動
        {
            if (CurrentVelocity.x > 0)// 原本向右
            {
                _temp.x = CurrentVelocity.x;
                _temp.x -= Time.deltaTime * _deceleration;
                if (_temp.x < 0)
                {
                    _temp.x = 0;
                }
                _workspace.Set(_temp.x, CurrentVelocity.y);
            }
            else if (CurrentVelocity.x < 0)// 原本向左
            {
                _temp.x = CurrentVelocity.x;
                _temp.x += Time.deltaTime * _deceleration;
                if (_temp.x > 0)
                {
                    _temp.x = 0;
                }
                _workspace.Set(_temp.x, CurrentVelocity.y);
            }
            else// 為零
            {
                _workspace.Set( 0.0f, CurrentVelocity.y);
            }
            SetFinalVelocity();
        }

    }
    
    private void SetFinalVelocity()
    {
        if (!CanSetVelocity) return;
        Rigidbody2D.velocity = _workspace;
        CurrentVelocity = _workspace;
    }

    #endregion
}
