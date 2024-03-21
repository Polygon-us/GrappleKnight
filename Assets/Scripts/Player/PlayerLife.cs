
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerLife : MonoBehaviour , ILife
{
    [SerializeField] private Slider _slider;

    private int _currentLife;
    private int _maxLife;

    private int CurrentLife
    {  
        get 
        { 
            return _currentLife;
        } 
        set 
        {
            _currentLife = value;
            _slider.value = value * _maxLife / 100;
        } 
    }

    SpriteRenderer _damageColor;
    private void Awake()
    {
        _damageColor = GetComponent<SpriteRenderer>();
    }
    public void Configure(int maxlife)
    {
        _slider.maxValue = maxlife;
        _maxLife = maxlife;
        _currentLife = maxlife;
    }
    
    public void ReduceLife(int amount)
    {
        CurrentLife -= amount;
        
        
        if (CurrentLife <= 0)
        {
            GetComponent<PlayerRespawnPosition>().RespawnLastPosition();
            CurrentLife = _maxLife;
        }
        else
        {
            StartCoroutine(CInvulnerability());
            StartCoroutine(DamageIndicator());
        }
        //Debug.Log(_currentLife);
    }
    
    public void Activate()
    {
        gameObject.SetActive(true);
    }
    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    IEnumerator CInvulnerability()
    {
        gameObject.layer = LayerMask.NameToLayer("Invulnerability");
        yield return new WaitForSeconds(1);
        gameObject.layer = LayerMask.NameToLayer("Player");
    }

    private IEnumerator DamageIndicator()
    {
        _damageColor.color = Color.red;
        yield return new WaitForSeconds(0.05f);
        _damageColor.color = Color.white;
    }
}