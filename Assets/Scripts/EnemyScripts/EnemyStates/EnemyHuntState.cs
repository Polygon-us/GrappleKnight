using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyHuntStatee : IState
{

    [SerializeField] private float _distanceToPlayer;
    [SerializeField] private Vector3 _inicialPosition;
    private Transform _playerTransform;
  

     public void HuntPlayer()
    {

    }

    public bool DoState(out EnemyStateEnum enemyStateEnum)
    {
        throw new System.NotImplementedException();
    }
}
