using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    // 延長玩家的輸入判斷時間
    [SerializeField] private float inputHoldTime = 0.2f;

    #region w/ Components

    private PlayerInput _playerInput;

    #endregion
    
    #region w/ Input Start Time
    // 輸入的開始時間
    private float _jumpInputStartTime;// 跳躍
    private float _dashInputStartTime;// 衝刺

    #endregion

    #region w/ Input Variables
    // 玩家輸入
    private Vector2 _rawMovementInput;
    public int NormalizedXInput { get; private set; }
    public int NormalizedYInput { get; private set; }
    public bool JumpInput { get; private set; }
    public bool JumpInputStop { get; private set; }

    #endregion

    #region w/ Unity Callback Functions

    private void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        
    }

    private void Update()
    {
        CheckJumpInputHoldTime();
    }    

    #endregion

    #region w/ Movement
    // 移動輸入
    public void OnMovementInput(InputAction.CallbackContext context)
    {
        _rawMovementInput = context.ReadValue<Vector2>();
        NormalizedXInput = Mathf.RoundToInt(_rawMovementInput.x);
        NormalizedYInput = Mathf.RoundToInt(_rawMovementInput.y);
    }

    #endregion

    #region w/ Jump
    // 跳躍輸入
    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            JumpInput = true;
            JumpInputStop = false;
            _jumpInputStartTime = Time.time;
        }

        if (context.canceled)
        {
            JumpInputStop = true;
        }
    }

    public void UseJumpInput() => JumpInput = false;

    private void CheckJumpInputHoldTime()
    {
        if (Time.time >= _jumpInputStartTime + inputHoldTime)
        {
            JumpInput = false;
        }
    }

    #endregion
}
