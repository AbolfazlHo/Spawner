using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class Automation
{
    [Tooltip("Delay between two consecutive spawns in milliseconds.")]
    [SerializeField] private float _perSpawnInterval;

    [SerializeField] private bool _stopSpawningAutomatically;
    [SerializeField] public Limitation _limitationSettings;
    
    [SerializeField] private UnityEvent _onSpawnStartEvent;
    [SerializeField] private UnityEvent _onSpawnEndEvent;

    public bool StopSpawningAutomatically => _stopSpawningAutomatically;
    public float PerSpawnInterval => _perSpawnInterval;

    public void OnSpawnStart()
    {
        _onSpawnStartEvent?.Invoke();
        
        if (_stopSpawningAutomatically)
        {
            _limitationSettings.OnSpawnStart();
        }
    }
    
    public void OnSpawnEnd()
    {
        _onSpawnEndEvent?.Invoke();
    }
}