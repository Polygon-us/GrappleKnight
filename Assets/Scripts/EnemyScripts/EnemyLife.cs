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
    
    public bool ReduceLife(int amount)
    {
        _currentLife -= amount;
        if (_currentLife<=0)
        {
            return true;
        }
        return false;
    }
    
    public void ActivateEnemy()
    {
        _enemyGameObject.SetActive(true);
    }
    public void DeactivateEnemy()
    {
        _enemyGameObject.SetActive(false);
    }
}

