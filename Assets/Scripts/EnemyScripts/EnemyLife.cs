using UnityEngine;

public class EnemyLife
{
    private int _currentLife;
    private GameObject _enemyGameObject;

    public EnemyLife(GameObject enemyGameObject, int maxlife)
    {
        _enemyGameObject = enemyGameObject;
        ConfigureInitialLife(maxlife);
    }
    public void ConfigureInitialLife(int maxlife)
    {
        _currentLife = maxlife;
    }
    
    public void ReduceLife(int amount)
    {
        _currentLife -= amount;
        if (_currentLife<=0)
        {
            _enemyGameObject.SetActive(false);
        }
        Debug.Log(_currentLife);
    }
    
    
}

