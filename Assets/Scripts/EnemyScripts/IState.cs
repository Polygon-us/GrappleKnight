using Unity.VisualScripting;
using System;
using UnityEngine;

public interface IState
{
    void StartState();
    bool DoState(out EnemyStateEnum enemyStateEnum);
    Action<Collision2D> CollisionAction();
    Action<Collider2D> ColliderAction();
    
}