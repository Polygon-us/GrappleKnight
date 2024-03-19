
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerLife : MonoBehaviour , ILife
{
    private int _currentLife;
    [SerializeField] private Slider _slider;

    private int _maxLife;

    SpriteRenderer _damageColor;
    private void Awake()
    {
        _damageColor = GetComponent<SpriteRenderer>();
    }
    public void Configure(int maxlife)
    {
        _maxLife = maxlife;
        _currentLife = maxlife;
       _slider.maxValue = _currentLife;
    }
    
    public void ReduceLife(int amount)
    {
        amount = amount * _maxLife / 100;
        _currentLife -= amount;
        _slider.value = _currentLife;
        
        if (_currentLife<=0)
        {
            Deactivate();
        }
        else
        {
            StartCoroutine(DamageIndicator());
            StartCoroutine(CInvulnerability());
        }
        if (_currentLife <= 0) 
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        Debug.Log(_currentLife);
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