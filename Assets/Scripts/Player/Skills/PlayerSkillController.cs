using UnityEngine;

public class PlayerSkillController : MonoBehaviour
{
    private ISkill _currentSkill;
    private ISkill _queueSkill;
    
    private bool _isSkillReady;
    private bool _queueStartSkill;

    private void Update()
    {
        if (_isSkillReady)
        {
            if (!_currentSkill.DoSkill())
            {
                if (_queueSkill != null)
                {
                    _currentSkill = _queueSkill;
                    _queueSkill = null;
                    
                }
                _isSkillReady = false;
                if (_queueStartSkill)
                {
                    StartSkill();
                    _queueStartSkill = false;
                }
            }
        }
    }

    public void StartSkill()
    {
        if (!_isSkillReady)
        {
            _currentSkill.InitSkill();
            _isSkillReady = true;
        }
        else
        {
            _queueStartSkill = true;
        }
    }

    public void StopSkill()
    {
        _currentSkill.UndoSkill();
    }

    public void ChangeCurrentSkill(ISkill skill)
    {
        if (!_isSkillReady)
        {
            _currentSkill = skill;
        }
        else
        {
            _queueSkill = skill;
        }
    }
}