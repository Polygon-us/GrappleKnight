using System.Collections.Generic;
using UnityEngine;

public class SkillManager
{
    private List<ISkill> _skillContainer = new List<ISkill>();
    private int _currentSkillIndex;
    
    public void AddSkill(ISkill skill)
    {
        _skillContainer.Add(skill);
    }

    public ISkill GetNextSkill()
    {
        //_currentSkillIndex = (_currentSkillIndex + ((_skillContainer.Count + (int)Mathf.Sign(Input.mouseScrollDelta.y)*(2- _skillContainer.Count)) / 2))%_skillContainer.Count;
        // if (_skillContainer[_currentSkillIndex] != actualGun && _skillContainer[_currentSkillIndex] != null)
        // {
        //     
        //     
        // }
        int index = _currentSkillIndex;
        _currentSkillIndex = (_currentSkillIndex + 1) % _skillContainer.Count;
        return _skillContainer[index];
    }
}