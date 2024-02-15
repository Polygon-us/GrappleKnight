using System;
using UnityEngine;

public class PlayerSkillController : MonoBehaviour
{
    private ISkill _currentSkill;
    private bool _isSkillReady;
    private void Update()
    {
        if (_isSkillReady)
        {
            if (!_currentSkill.DoSkill())
            {
                _isSkillReady = false;
            }
        }
    }

    public void StartSkill()
    {
        _isSkillReady = true;
        //_currentSkill.DoSkill();
    }

    public void StopSkill(){
        _currentSkill.UndoSkill();
    }
    public void ChangeCurrentSkill(ISkill skill)
    {
        if (_currentSkill!=null)
        {
            _currentSkill.UndoSkill();
        }
        _currentSkill = skill;
    }
   
}