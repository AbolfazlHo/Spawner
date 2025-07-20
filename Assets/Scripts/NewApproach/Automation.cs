using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class Automation
{
    [Tooltip("Delay between two consecutive spawns in milliseconds.")]
    [SerializeField] private float _perSpawnInterval;

    [SerializeField] private bool _stopSpawningAutomatically;
//    [SerializeField] private bool _stopSpawningAutomatically;
    // ToDo: Handle the appearance of the following field using custom inspector (editor)
//    [SerializeField] private Limitation _limitationSettings;
    [SerializeField] public Limitation _limitationSettings;
    
    
    
    [SerializeField] private UnityEvent _onSpawnStartEvent;
    [SerializeField] private UnityEvent _onSpawnEndEvent;



    public bool StopSpawningAutomatically => _stopSpawningAutomatically;

    public float PerSpawnInterval => _perSpawnInterval;

    private Coroutine _spawnRoutine = null;

//    public void StartAutomaticSpawn()
//    {
//        
//    }

//
//    public void StartSpawnAutomatically()
//    {
//        _spawnRoutine = MonoBehaviour.StartCoroutine(SpawnQoroutine());
//    }
//
//    public void StopSpawnRoutine()
//    {
//        
//    }
//
////    public IEnumerator HandleAutomaticSpawning(int spawnedObjectsReached)
//    public async UniTask<bool> HandleAutomaticSpawning(int spawnedObjectsReached)
////    public IEnumerator<bool> HandleAutomaticSpawning(int spawnedObjectsReached)
////    public IEnumerator HandleAutomaticSpawning()
////    public IEnumerator SpawnQoroutine()
//    {
//        if (_stopSpawningAutomatically)
//        {
//            while (!_limitationSettings.LimitationReached(spawnedObjectsReached))
//            {
//                
//            }
//        }
//        
//        while (ContinueSpawing())
//        {
//            Spawn();
//            yield return new WaitForSeconds(_perSpawnInterval);
//        }
//    }


//
//    /// <summary>
//    /// Checks the spawn conditions.
//    /// </summary>
//    /// <returns></returns>
//    private bool ContinueSpawing()
//    {
////        if (_stopSpawningBy == SpawnLimitationType.None) return true;
//
//        if (_stopSpawningBy == SpawnLimitationType.Time)
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
