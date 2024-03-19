using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data Enemy", menuName = "Enemy")]
public class EnemiesScriptableObjectTemplate : ScriptableObject
{
    //Variables de configuracion del boss
    [SerializeField] private float _jumpHeight;
    [SerializeField] private float _waveDuracion = 0.5f;
    [SerializeField] private float _waveImpactForce = 3f;
    [SerializeField] private float _forceDown = 20f;
    [SerializeField] private float _delayBeforeReturn = 0.1f;
    [SerializeField] private int _maxLife;

    [SerializeField] private Vector3 _finalWaveScale = new Vector3(16, 0.1f);
    [SerializeField] private Vector3 _inicialWaveScale = new Vector3(1, 0.1f);
}
