using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class Limitation
{
    public enum LimitationType
    {
        Count = 0,
        Time = 1
    }

    [SerializeField] private LimitationType _limitationType;
    
    // ToDo: Handle appearance of the following fields using custom inspector (editor)
    [SerializeField] private float _limitTimeBy = 0;
    [SerializeField] private int _limitCountBy = 0;

    [SerializeField] private UnityEvent _onSpawnStartEvent;
//    [SerializeField] private UnityEvent _onSpawnEndEvent;
//    [SerializeField] private UnityEvent _onSpawnableSpawnedEvent;
    [SerializeField] private UnityEvent _onLimitationReachedEvent;

    private float _spawnStartTime = 0;
    
    
    
    
//    
//    
//    /// <summary>
//    /// Checks the spawn conditions.
//    /// </summary>
//    /// <returns></returns>
//    private bool ContinueSpawing()
//    {
////        if (_stopSpawningBy == SpawnLimitationType.None) return true;
//
//        if (_limitationType == LimitationType.Time)
//        {
//            _passedTime += _perSpawnInterval;
//            if (_passedTime < _stopSpawningWhenSpawnTimeReached) return true;
//            return false;
//        }
//        
//        if (_stopSpawningBy == SpawnLimitationType.Count)
//        {
//            if (_allSpwnedObjects.Count < _stopSpawningWhenSpawnedObjectsCountReached) return true;
//            return false;
//        }
//        
//        return true;
//    }
//    
//    
    
    
    

    public bool LimitationReached(int spawnedSpawnablesCount)
    {
        var limitationReached = false;

        if (_limitationType == LimitationType.Count)
        {
            limitationReached = spawnedSpawnablesCount >= _limitCountBy;
        }
        else
        {
            limitationReached = Time.time - _spawnStartTime >= _limitTimeBy;
        }

        if (limitationReached)
        {
            OnLimitationReached();
        }
        
        return limitationReached;
    }

    public void OnSpawnStart()
    {
        if (_limitationType == LimitationType.Time)
        {
            _spawnStartTime = Time.time;
        }
        
        _onSpawnStartEvent?.Invoke();
    }

    public void OnLimitationReached()
    {
        _onLimitationReachedEvent?.Invoke();
    }

}
