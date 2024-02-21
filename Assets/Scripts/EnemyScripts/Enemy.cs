using UnityEngine;
public class Enemy : MonoBehaviour
{
    private EnemyLife _enemyLife;
    [SerializeField]private int _maxLife;
    private void Awake()
    {
        _enemyLife = new EnemyLife(gameObject,_maxLife);
    }

    public void ReduceEnemyLife(int amount)
    {
        _enemyLife.ReduceLife(amount);
    }
    
}
