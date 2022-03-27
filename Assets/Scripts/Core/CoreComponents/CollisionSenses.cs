using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSenses : CoreComponent
{
    #region w/ Core Components
    // Move
    private Movement Movement => _movement ? _movement : Core.GetCoreComponent(ref _movement);
    private Movement _movement;

    #endregion

    #region w/ Check Transform and Check Distance
    // 負責確認碰撞的物件之 transform 和 距離設置
    // LayerMask
    [SerializeField] private LayerMask whatIsGround;
    // Transform
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Transform ledgeHorizontalCheck;
    [SerializeField] private Transform ceilingCheck;
    // Distance
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private float wallCheckDistance;
    // ----------------------------------------------------------------------------------------------------------------
    // LayerMask
    public LayerMask WhatIsGround { get => whatIsGround; private set => whatIsGround = value; }
    // Transform
    public Transform GroundCheck { get => GenericNotImplementedError<Transform>.TryGet(groundCheck, CoreParentName); private set => groundCheck = value; }
    public Transform WallCheck { get => GenericNotImplementedError<Transform>.TryGet(wallCheck, CoreParentName); private set => wallCheck = value; }
    public Transform LedgeHorizontalCheck { get => GenericNotImplementedError<Transform>.TryGet(ledgeHorizontalCheck, CoreParentName); private set => ledgeHorizontalCheck = value; }
    public Transform CeilingCheck { get => GenericNotImplementedError<Transform>.TryGet(ceilingCheck, CoreParentName); private set => ceilingCheck = value; }
    // Distance
    public float GroundCheckRadius { get => groundCheckRadius; private set => groundCheckRadius = value; }
    public float WallCheckDistance { get => wallCheckDistance; private set => wallCheckDistance = value; }
    
    #endregion

    #region w/ Check Bools
    // 確認是否碰撞的布林值
    public bool Ground =>
        Physics2D.OverlapCircle(GroundCheck.position, GroundCheckRadius, WhatIsGround);
    public bool WallFront =>
        Physics2D.Raycast(WallCheck.position, Vector2.right * Movement.FacingDirection, WallCheckDistance, WhatIsGround);
    public bool WallBack =>
        Physics2D.Raycast(WallCheck.position, Vector2.right * -Movement.FacingDirection, WallCheckDistance, WhatIsGround);
    public bool LedgeHorizontal =>
        Physics2D.Raycast(LedgeHorizontalCheck.position, Vector2.right * Movement.FacingDirection, WallCheckDistance, WhatIsGround);
    public bool Ceiling => Physics2D.OverlapCircle(CeilingCheck.position, GroundCheckRadius, WhatIsGround);

    #endregion

    #region w/ Draw On Scene
    // 繪製出範圍到 Scene 上面
    public virtual void OnDrawGizmos()
    {
        if (Core == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(GroundCheck.position, GroundCheckRadius);
        Gizmos.DrawWireSphere(CeilingCheck.position, GroundCheckRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(WallCheck.position, WallCheck.position + (Vector3)(Vector2.right * Movement.FacingDirection * WallCheckDistance));
        Gizmos.color = Color.red;
        Gizmos.DrawLine(LedgeHorizontalCheck.position, LedgeHorizontalCheck.position + (Vector3)(Vector2.right * Movement.FacingDirection * WallCheckDistance));
    }

    #endregion
}
