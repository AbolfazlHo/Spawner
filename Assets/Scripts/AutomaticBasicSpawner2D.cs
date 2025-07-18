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
    
//    [SerializeField] private int _stopSpawningbyReach;
    [SerializeField] private int _stopSpawningWhenSpawnedObjectsCountReached;
    [SerializeField] private int _stopSpawningWhenSpawnTimeReached;
    

    [SerializeField] private float _perSpawnInterval = 0.1f;

    

//    private bool _continue = true;

//    private bool _stop = false;


//    private float _spawnStartTime;

    private Coroutine _spawningRoutine;


    private float _passedTime = 0;
    
    
    public void StartSpawningAutomatically()
    {
        Debug.Log("public void StartSpawningAutomatically()");
        
//        _spawnStartTime = Time.time;
//         _spawningRoutine = StartCoroutine(nameof(HandleAutomaticSpawning));
         _spawningRoutine = StartCoroutine("HandleAutomaticSpawning");
    }
    
    
//ToDo: Use coroutine or async for such methods.
//    public void StartSpawningAutomatically()
    private IEnumerator HandleAutomaticSpawning()
    {
        Debug.Log("private IEnumerable HandleAutomaticSpawning()");
        
//        _stop = false;
        
        while (ContinueSpawing())
        {
            Spawn();
            
            yield return new WaitForSeconds(_perSpawnInterval);
            
        }
        
        Debug.Log("Spawning finished");

//        _stop = false;
    }

    public void StopSpawning()
    {
        Debug.Log("Spawning Stopped");
        StopCoroutine(_spawningRoutine);
        
//        _stop = true;
        
        
    }

    /// <summary>
    /// Checks the spawn conditions.
    /// </summary>
    /// <returns></returns>
    private bool ContinueSpawing()
    {
        
        Debug.Log("private bool ContinueSpawing()");
        
//        if (_stop) return false;
        if (_stopSpawningBy == SpawnLimitationType.None) return true;

        if (_stopSpawningBy == SpawnLimitationType.Time)
        {
            _passedTime += _perSpawnInterval;

            Debug.Log("passed time   :::   "+_passedTime);
            
            if (_passedTime < _stopSpawningWhenSpawnTimeReached) return true;

            return false;

//            var passedTime = 
        }
        
        
        if (_stopSpawningBy == SpawnLimitationType.Count)
        {
            if (_allSpwnedObjects.Count < _stopSpawningWhenSpawnedObjectsCountReached) return true;
            return false;
        }
        
        
        return true;
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
