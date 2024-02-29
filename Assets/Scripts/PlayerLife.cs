using System.Collections;
using UnityEngine;

public class PlayerLife : MonoBehaviour , ILife
{
    private int _currentLife;
    
    public void Configure(int maxlife)
    {
        _currentLife = maxlife;
       
    }
    
    public void ReduceLife(int amount)
    {
        _currentLife -= amount;
        if (_currentLife<=0)
        {
            Deactivate();
        }
        else
        {
            StartCoroutine(CInvulnerability());
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
        gameObject.layer = LayerMask.NameToLayer("Default");
    }
}