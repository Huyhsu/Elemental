using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region w/ Player Data
    // 角色資料
    [SerializeField] private PlayerData playerData;

    public PlayerData PlayerData
    {
        get => playerData;
        private set => playerData = value;
    }
    // 狀態列表
    [SerializeField] private PlayerState[] states;

    public PlayerState[] States
    {
        get => states;
        private set => states = value;
    }

    #endregion

    #region w/ Components
    // 元件
    public Animator Animator { get; private set; }
    public Core Core { get; private set; }
    public PlayerInputHandler InputHandler { get; private set; }
    public PlayerStateMachine StateMachine { get; private set; }
    
    #endregion

    #region w/ Unity Callback Functions

    private void Awake()
    {
        Animator = GetComponent<Animator>();
        Core = transform.parent.GetComponentInChildren<Core>();
        InputHandler = GetComponentInParent<PlayerInputHandler>();
        StateMachine = new PlayerStateMachine
        {
            StateTable = new Dictionary<System.Type, IState>(states.Length)
        };
        // 初始化各狀態，加入到 state machine 的狀態映射表<type, state>
        foreach (var state in states)
        {
            state.Initialize(this);
            StateMachine.StateTable.Add(state.GetType(), state);
        }
    }

    private void Start()
    {
        // 起始狀態設定
        StateMachine.Initialize(typeof(PlayerIdleState));
    }

    private void Update()// 更新 core 的 components 與狀態機的 state
    {
        Core.LogicUpdate();
        StateMachine.CurrentState.LogicUpdate();
        
        // --偵錯用--
        // Debug.Log(StateMachine.CurrentState);
    }

    private void FixedUpdate()// 更新碰撞相關
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }

    #endregion
    
    #region w/ Animation Trigger Functions
    // 動畫觸發相關
    private void AnimationTrigger() => StateMachine.CurrentState.AnimationTrigger();
    private void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();

    #endregion
}
