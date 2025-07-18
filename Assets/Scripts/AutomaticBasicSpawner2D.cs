using System.Collections;
using UnityEngine;

public class AutomaticBasicSpawner2D : ManualBasicSpawner2D
{
    public enum SpawnLimitationType
    {
        None,
        Time,
        Count
    }

    [SerializeField] private SpawnLimitationType _stopSpawningBy;
    
    //ToDo: handle the appearance of the following fields in custom inspector(Editor) class
    [SerializeField] private int _stopSpawningWhenSpawnedObjectsCountReached;
    [SerializeField] private int _stopSpawningWhenSpawnTimeReached;
    

    [SerializeField] private float _perSpawnInterval = 0.1f;

    private Coroutine _spawningRoutine;
    private float _passedTime = 0;
    
    public void StartSpawningAutomatically()
    {
         _spawningRoutine = StartCoroutine("HandleAutomaticSpawning");
    }
    
    
    private IEnumerator HandleAutomaticSpawning()
    {
        while (ContinueSpawing())
        {
            Spawn();
            yield return new WaitForSeconds(_perSpawnInterval);
        }
    }

    public void StopSpawning()
    {
        StopCoroutine(_spawningRoutine);
    }

    /// <summary>
    /// Checks the spawn conditions.
    /// </summary>
    /// <returns></returns>
    private bool ContinueSpawing()
    {
        if (_stopSpawningBy == SpawnLimitationType.None) return true;

        if (_stopSpawningBy == SpawnLimitationType.Time)
        {
            _passedTime += _perSpawnInterval;
            if (_passedTime < _stopSpawningWhenSpawnTimeReached) return true;
            return false;
        }
        
        if (_stopSpawningBy == SpawnLimitationType.Count)
        {
            if (_allSpwnedObjects.Count < _stopSpawningWhenSpawnedObjectsCountReached) return true;
            return false;
        }
        
        return true;
    }
}
