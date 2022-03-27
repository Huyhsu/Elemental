using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboUI : MonoBehaviour
{
    [SerializeField] private Image comboInputFirst;
    [SerializeField] private Image comboInputSecond;
    [SerializeField] private Image comboInputThird;
    [SerializeField] private Image comboInputFourth;
    [SerializeField] private Slider timeSlider;
    
    public Animator ComboFirstAnimator { get; private set; }
    public Animator ComboSecondAnimator { get; private set; }
    public Animator ComboThirdAnimator { get; private set; }
    public Animator ComboFourthAnimator { get; private set; }

    public Slider TimeSlider
    {
        get => timeSlider;
        private set => timeSlider = value;
    }

    
    private void Awake()
    {
        ComboFirstAnimator = comboInputFirst.GetComponent<Animator>();
        ComboSecondAnimator = comboInputSecond.GetComponent<Animator>();
        ComboThirdAnimator = comboInputThird.GetComponent<Animator>();
        ComboFourthAnimator = comboInputFourth.GetComponent<Animator>();
    }
}
