using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class Automation
{
    [Tooltip("Delay between two consecutive spawns in milliseconds.")]
    [SerializeField] private float _perSpawnInterval;

    [SerializeField] private bool _stopSpawningAutomatically;
    // ToDo: Handle the appearance of the following field using custom inspector (editor)
    [SerializeField] public Limitation _limitationSettings;
    
    [SerializeField] private UnityEvent _onSpawnStartEvent;
    [SerializeField] private UnityEvent _onSpawnEndEvent;

    public bool StopSpawningAutomatically => _stopSpawningAutomatically;
    public float PerSpawnInterval => _perSpawnInterval;

//    private Coroutine _spawnRoutine = null;

    public void OnSpawnStart()
    {
//        Debug.Log("public void OnSpawnStart()");
        
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