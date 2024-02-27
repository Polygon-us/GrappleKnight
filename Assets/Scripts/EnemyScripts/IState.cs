using Unity.VisualScripting;
using System;
using UnityEngine;

public interface IState
{

    bool DoState(out EnemyStateEnum enemyStateEnum);
    Action<Collision2D> CollisionAction();
}