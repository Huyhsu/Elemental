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
    [Header("Player State")]
    [SerializeField] private PlayerState[] states;

    public PlayerState[] States
    {
        get => states;
        private set => states = value;
    }

    [SerializeField] private ComboUI comboUI;

    public ComboUI ComboUI
    {
        get => comboUI;
        private set => comboUI = value;
    }
    
    #endregion

    #region w/ Components
    // 元件
    public GameObject SkillGameObject { get; private set; }
    public Animator Animator { get; private set; }
    public Animator SkillAnimator { get; private set; }
    public Core Core { get; private set; }
    public PlayerInputHandler InputHandler { get; private set; }
    public PlayerStateMachine StateMachine { get; private set; }
    public SkillInventory SkillInventory { get; private set; }
    
    private ComboInput  ComboInput => _comboInput ? _comboInput : Core.GetCoreComponent(ref _comboInput);
    private ComboInput _comboInput;
        
    #endregion

    #region w/ Unity Callback Functions

    private void Awake()
    {
        SkillGameObject = transform.parent.Find("Base").gameObject;
        Animator = GetComponent<Animator>();
        SkillAnimator = transform.parent.Find("Base").GetComponent<Animator>();
        SkillInventory = transform.parent.GetComponentInChildren<SkillInventory>();
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
        // 確認是否有 Cast State
        CheckCastState();
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
        ComboInput.LogicUpdate();
        
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

    #region w/ Skill Check

    public PlayerCastState CastState { get; private set; }
    private void CheckCastState()
    {
        foreach (var state in states)
        {
            if (state.GetType() == typeof(PlayerCastState))
            {
                CastState = (PlayerCastState) state;
                break;
            }
        }

        if (CastState == null)
        {
            Debug.LogError("There is No Cast State on " + gameObject.transform.name);
        }
    }

    #endregion
}
