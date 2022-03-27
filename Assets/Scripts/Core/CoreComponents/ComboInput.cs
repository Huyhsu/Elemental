using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ComboInput : CoreComponent
{
    protected override void Awake()
    {
        base.Awake();
        _player = GetComponentInParent<Player>();
        _castState = _player.CastState;
        _castInput = false;
    }

    private Player _player;
    private PlayerCastState _castState;

    private bool _castInput;
    private float _startTime;
    private Skill _targetSkill;
    
    #region w/ Combo Input
    //連擊輸入相關
    // Variable
    private List<int> _currentComboInputs = new List<int>(4);// 當前輸入的 List
    private string _currentTap;// 目前對應的 animation state name
    private bool _isCorrect;// 判斷輸入是否符合
    private bool _shouldCheck;
    private bool _shouldReset;// 是否應該 reset 輸入
    private int _comboInputMaxSize = 4;// inputs 最大的大小
    private float _endTime;// 配合延遲些微時間計算用
    // Player State
    private PlayerCastState _targetState;
    // Inputs
    private bool _comboFirstInput;
    private bool _comboSecondInput;
    private bool _comboThirdInput;
    private bool _comboFourthInput;
    // Check Input and Animation Functions
    private void CheckComboInputSize()
    {
        if (_currentComboInputs.Count == _comboInputMaxSize)
        {
            _currentComboInputs.Clear();
            _shouldReset = false;
            _shouldCheck = true;
        }
    }
    private void CheckComboInput()
    {
        if (_comboFirstInput)
        {
            _player.InputHandler.UseComboFirstInput();
            CheckComboInputSize();
            _currentComboInputs.Add(1);
            _currentTap = "Tap1";
        }
        else if (_comboSecondInput)
        {
            _player.InputHandler.UseComboSecondInput();
            CheckComboInputSize();
            _currentComboInputs.Add(2);
            _currentTap = "Tap2";
        }
        else if (_comboThirdInput)
        {
            _player.InputHandler.UseComboThirdInput();
            CheckComboInputSize();
            _currentComboInputs.Add(3);
            _currentTap = "Tap3";
        }
        else if (_comboFourthInput)
        {
            _player.InputHandler.UseComboFourthInput();
            CheckComboInputSize();
            _currentComboInputs.Add(4);
            _currentTap = "Tap4";
        }
    }
    private void CheckAnimation(int count)
    {
        switch (count)
        {
            case 0:
                _player.ComboUI.ComboFirstAnimator.Play("Wait");
                // reset 時，需要設置動畫
                _player.ComboUI.ComboSecondAnimator.Play("TapEmpty");
                _player.ComboUI.ComboThirdAnimator.Play("TapEmpty");
                _player.ComboUI.ComboFourthAnimator.Play("TapEmpty");
                break;
            case 1:
                _player.ComboUI.ComboFirstAnimator.Play(_currentTap);
                _player.ComboUI.ComboSecondAnimator.Play("Wait");
                // 清空後再次輸入時，需要設置動畫
                _player.ComboUI.ComboThirdAnimator.Play("TapEmpty");
                _player.ComboUI.ComboFourthAnimator.Play("TapEmpty");
                break;
            case 2:
                _player.ComboUI.ComboSecondAnimator.Play(_currentTap);
                _player.ComboUI.ComboThirdAnimator.Play("Wait");
                break;
            case 3:
                _player.ComboUI.ComboThirdAnimator.Play(_currentTap);
                _player.ComboUI.ComboFourthAnimator.Play("Wait");
                break;
            case 4:
                _player.ComboUI.ComboFourthAnimator.Play(_currentTap);
                break;
        }
    }

    #endregion

    #region w/ State Workflow

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        _castInput = _player.InputHandler.CastInput;
        if(!_castInput) return;// 若按下施法按鍵，才會繼續執行以下判斷
        
        _player.InputHandler.UseCastInput();// 設定使用過施法按鍵
        _startTime = Time.time;
        
        
        // Combo UI
        _player.ComboUI.gameObject.SetActive(true);
        _player.ComboUI.TimeSlider.maxValue = _player.PlayerData.checkInputTime;// 時間條最大值
        _player.ComboUI.TimeSlider.value = 0;// 時間條起始值
        
        _currentComboInputs.Clear();// 清空 List
        _isCorrect = false;
        _shouldReset = false;
        _shouldCheck = true;

        _comboFirstInput = _player.InputHandler.ComboFirstInput;
        _comboSecondInput = _player.InputHandler.ComboSecondInput;
        _comboThirdInput = _player.InputHandler.ComboThirdInput;
        _comboFourthInput = _player.InputHandler.ComboFourthInput;

        _player.ComboUI.TimeSlider.value = Time.time - _startTime;// 時間值

        if (!_isCorrect)// 若還沒輸入對，繼續判斷輸入
        {
            CheckComboInput();
            // if (!IsAbilityDone)// 避免 UI 結束時( inactive )，仍試圖播放動畫
            // {
            //     CheckAnimation(_currentComboInputs.Count);
            // }
        }

        // --- 退出 ---
        if (!_isCorrect && (_castInput || Time.time >= _startTime + _player.PlayerData.checkInputTime))// 仍不符合 and (取消施放 or 超過時間)
        {
            _player.InputHandler.UseCastInput();
            // 結束 InSpell
            // IsAbilityDone = true;
        }
        // --- 檢查 ---
        else if (!_isCorrect && _shouldCheck && _currentComboInputs.Count == _comboInputMaxSize)// 輸入符合 => _isCorrect = true，_shouldCheck = false (只執行一次)
        {
            foreach (var t in _player.SkillInventory.skills)
            {
                if (_currentComboInputs.SequenceEqual(t.SkillData.comboInputs))
                {
                    _targetSkill = t;
                    _isCorrect = true;
                    _endTime = Time.time;
                    // break;
                }
            }
            _shouldCheck = false;
        }
        // --- 正確 轉換 state ---
        else if (_isCorrect && Time.time >= _endTime + _player.PlayerData.waitCheckInputTime)// // 輸入符合 and 經過些微時間後 => change state
        {
            // Cast
            _castState.SetSkill(_targetSkill);
            _player.StateMachine.ChangeState(typeof(PlayerCastState));
        }
        // --- 錯誤 ---
        else if (!_isCorrect && _currentComboInputs.Count == 4 && !_shouldReset)// 輸入不符合 => _shouldReset = true (只執行一次)
        {
            _endTime = Time.time;
            _shouldReset = true;
        }
        else if (!_isCorrect && _shouldReset && Time.time >= _endTime + _player.PlayerData.waitCheckInputTime && _currentComboInputs.Count == _comboInputMaxSize)// 輸入不符合 and 經過些微時間後 => reset
        {
            _currentComboInputs.Clear();
            CheckAnimation(_currentComboInputs.Count);
            _shouldReset = false;
            _shouldCheck = true;
        }
    }

    #endregion
}
