using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class SkillManager
{
    private List<ISkill> _skillContainerLeft = new List<ISkill>();
    private List<ISkill> _skillContainerRight = new List<ISkill>();
    private int _currentLeftSkillIndex;
    private int _currentRightSkillIndex;
    
    public void AddLeftSkill(ISkill skill)
    {
        _skillContainerLeft.Add(skill);
    } 
    public void AddRightSkill(ISkill skill)
    {
        _skillContainerRight.Add(skill);
    }

    public ISkill GetNextLeftSkill(out PlayerMovementEnum playerMovementEnum)
    {
        int index = _currentLeftSkillIndex;
        _currentLeftSkillIndex = (_currentLeftSkillIndex + 1) % _skillContainerLeft.Count;
        ISkill currentSkill = _skillContainerLeft[index];
        playerMovementEnum = currentSkill.SendActionMapTypeEnum();
        return currentSkill;
    }
    public ISkill GetNextRightSkill(out PlayerMovementEnum playerMovementEnum)
    {
        int index = _currentRightSkillIndex;
        _currentRightSkillIndex = (_currentRightSkillIndex + 1) % _skillContainerRight.Count;
        ISkill currentSkill = _skillContainerRight[index];
        playerMovementEnum = currentSkill.SendActionMapTypeEnum();
        return currentSkill;
    }

    public void UnsubscribeActions()
    {
        foreach (ISkill item in _skillContainerLeft)
        {
            item.UnsubscribeActions();
        }
    }
}