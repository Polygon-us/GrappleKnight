using UnityEngine;

public class PlayerSkillController
{
    private ISkill _currentSkill;

    public void StartSkill()
    {
        _currentSkill.DoSkill();
    }

    public void ChangeCurrentSkill(ISkill skill)
    {
        _currentSkill = skill;
        Debug.Log(_currentSkill);
    }
   
}