using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new_SkillData", menuName = "Data/Skill Data/Skill")]
public class SkillData : ScriptableObject
{
    [Header("Combo Input")]
    public List<int> comboInputs = new List<int>(4) {0, 1, 2, 3};
}
