
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerLife : MonoBehaviour , ILife
{
    [SerializeField] private Slider slider;
    [SerializeField] private PlayerFreezer freezer;

    private Rigidbody2D _rigidbody;
    
    private int _maxLife;

    public int currentLife;

    public int CurrentLife
    {  
        get 
        { 
            return currentLife;
        } 
        set 
        {
            currentLife = value;
            slider.value = value * _maxLife / 100;
        } 
    }

    SpriteRenderer _damageColor;
    private void Awake()
    {
        _damageColor = GetComponent<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Configure(int maxlife)
    {
        slider.maxValue = maxlife;
        _maxLife = maxlife;
        currentLife = maxlife;
    }
    
    public void ReduceLife(int amount)
    {
        CurrentLife -= amount;
        
        if (CurrentLife <= 0)
        {
            GetComponent<PlayerRespawnPosition>().RespawnLastPosition();

            //if the player dead reset variable freeze to freeze player when he comes in to the boss zone
            freezer.freeze = true;

            CurrentLife = _maxLife;
            _rigidbody.velocity = Vector3.zero;
        }
        else
        {
            StartCoroutine(CInvulnerability());
            StartCoroutine(DamageIndicator());
        }
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