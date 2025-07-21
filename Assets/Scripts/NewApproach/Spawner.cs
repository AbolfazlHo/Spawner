using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _spawnAreaGameObject;
    [SerializeField] private List<Spawnable> _spawnables;


    [SerializeField] private bool _spawnAutomaticaly;
    // ToDo: Handle appearance of the following fields using custom inspector (editor)
    [SerializeField] private Automation _spawnAutomationSettings;


    [SerializeField] private bool _isCollisionSafe = false;
    [SerializeField] private CollisionSafety _collisionSafetySettings;
    

    [SerializeField] private UnityEvent _onSpawnableSpawnedEvent;

    private Vector4 _spawnArea;
    private bool _spawnAreaHasSet = false;
    private Coroutine _autoSpawnRoutine = null;
    
    protected List<Spawnable> _allSpwnedObjects = new List<Spawnable>();
    
    void Start()
    {
        _allSpwnedObjects = new List<Spawnable>();
    }
    
    public void Spawn()
    {
        Debug.Log("public void Spawn()");
        
        DoSpawnPrerequisite();
        
        if (_spawnAutomaticaly)
        {
            if (_autoSpawnRoutine == null)
            {
                _autoSpawnRoutine = StartCoroutine(HandleAutomaticSpawning());
            }
            
//            return;
        }
        else
        {
//            DoSpawnPrerequisite();
            SpawnRandomSpawnable();
        }
        
        
        
//        if (!_spawnAreaHasSet) SetSpawnArea();
//        var randomSpawnableIndex = Random.Range(0, _spawnables.Count);
//        SpawnASpawnableOf(_spawnables[randomSpawnableIndex]).GetAwaiter();
    }

    private void DoSpawnPrerequisite()
    {
        if (!_spawnAreaHasSet) SetSpawnArea();
    }
    
    private void SpawnRandomSpawnable()
    {
        var randomSpawnableIndex = Random.Range(0, _spawnables.Count);
        SpawnASpawnableOf(_spawnables[randomSpawnableIndex]).GetAwaiter();
    }
    
    private IEnumerator HandleAutomaticSpawning()
    {
        Debug.Log("private IEnumerator HandleAutomaticSpawning()");
        
        if (_spawnAutomationSettings.StopSpawningAutomatically)
        {
            _spawnAutomationSettings.OnSpawnStart();
            
//            while (!_spawnAutomationSettings._limitationSettings.LimitationReached(_spawnables.Count))
            while (!_spawnAutomationSettings._limitationSettings.LimitationReached(_allSpwnedObjects.Count))
            {
//                Spawn();

                SpawnRandomSpawnable();

                yield return new WaitForSeconds(_spawnAutomationSettings.PerSpawnInterval);
            }
        }
        else
        {
            while (true)
            {
                Spawn();
                yield return new WaitForSeconds(_spawnAutomationSettings.PerSpawnInterval);
            }
        }
    }
    
    public void StopSpawning()
    {
        if (_spawnAutomaticaly && _autoSpawnRoutine != null)
        {
            StopCoroutine(_autoSpawnRoutine);
            _spawnAutomationSettings.OnSpawnEnd();
        }
    }

    public void ChangeSpawnAreaGameObject(GameObject newSpawnArea)
    {
        _spawnAreaHasSet = false;
        _spawnAreaGameObject = newSpawnArea;
        SetSpawnArea();
    }

    private void SetSpawnArea()
    {
        var horizontalBoundSize = 0.0f;
        var verticalBoundSize = 0.0f;
        var globalPositionofSpawnAreaGameObject = _spawnAreaGameObject.transform.position;
        var rendererComponentofSpawnAreaGameObject = _spawnAreaGameObject.GetComponent<Renderer>();

        if (rendererComponentofSpawnAreaGameObject != null)
        {
            horizontalBoundSize = rendererComponentofSpawnAreaGameObject.bounds.size.x;
            verticalBoundSize = rendererComponentofSpawnAreaGameObject.bounds.size.y;
        }
        else
        {
            var colliderComponentofSpawnAreaGameObject = _spawnAreaGameObject.GetComponent<Collider>();
            
            if (colliderComponentofSpawnAreaGameObject != null)
            {
                horizontalBoundSize = colliderComponentofSpawnAreaGameObject.bounds.size.x;
                verticalBoundSize = colliderComponentofSpawnAreaGameObject.bounds.size.y;
            }
            else
            {
                Debug.LogError("Current Spawn Area GameObject does not have any kind of renderer or collider component." +
                               "Spawner component uses these components to set the spawn area.");
                _spawnArea = Vector4.zero;
                return;
            }
        }
        
        _spawnArea = new Vector4(
            globalPositionofSpawnAreaGameObject.x - horizontalBoundSize/2.0f,
            globalPositionofSpawnAreaGameObject.y - verticalBoundSize/2.0f,
            globalPositionofSpawnAreaGameObject.x + horizontalBoundSize/2.0f,
            globalPositionofSpawnAreaGameObject.y + verticalBoundSize/2.0f
            );

        _spawnAreaHasSet = true;
    }

    protected virtual async UniTask SpawnASpawnableOf(Spawnable spawnable)
    {
        var newSpawnable = InstantiateSpawnable(spawnable);

        if (_isCollisionSafe)
        {
//            var hasPlaced = await _collisionSafetySettings.PlaceSpawnable(spawnable);
            var hasPlaced = await _collisionSafetySettings.PlaceSpawnable(spawnable, (UniTask) =>
            {
                PlaceSpawnable(spawnable);
            });

            if (!hasPlaced)
            {
                _allSpwnedObjects.Remove(spawnable);
                Destroy(spawnable.gameObject);
            }
            
        }
        
        await PlaceSpawnable(newSpawnable);
        ReleaseSpawnable(newSpawnable);
    }
    
    private Spawnable InstantiateSpawnable(Spawnable spawnable)
    {
        var newSpawnable = Instantiate(spawnable);
        return newSpawnable;
    }

    protected virtual async UniTask PlaceSpawnable(Spawnable spawnable)
    {
        var randomX = 0.0f;
        var randomY = 0.0f;
  
        randomX = Math.Abs(_spawnArea.x - _spawnArea.z) < 0.1f ? _spawnArea.x : Random.Range(_spawnArea.x, _spawnArea.z);
        randomY = Math.Abs(_spawnArea.y - _spawnArea.w) < 0.1f ? _spawnArea.y : Random.Range(_spawnArea.y, _spawnArea.w);

        spawnable.transform.position = new Vector3(randomX, randomY, 0);
    }

    protected virtual void ReleaseSpawnable(Spawnable spawnable)
    {
        spawnable.Release();
        _allSpwnedObjects.Add(spawnable);

        if (_isCollisionSafe)
        {
            _collisionSafetySettings.ReleaseSpawnable(spawnable, (UniTask) => PlaceSpawnable(spawnable));
        }
        
        
    }

    public void OnSpawnableSpawned()
    {
        _onSpawnableSpawnedEvent?.Invoke();
    }
}