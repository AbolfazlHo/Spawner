using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Soor.Spawner2d
{
    public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _spawnAreaGameObject;
    [SerializeField] private List<Spawnable> _spawnables;
    [SerializeField] private bool _spawnAutomaticaly;
    [SerializeField] private Automation _spawnAutomationSettings;
    [SerializeField] private bool _isCollisionSafe = false;
    [SerializeField] private CollisionSafety _collisionSafetySettings;
    [SerializeField] private UnityEvent _onSpawnableSpawnedEvent;
    [SerializeField] private string _spawnableTag = "SoorSpawnable";

    private Vector4 _spawnArea;
    private bool _spawnAreaHasSet = false;
    private Coroutine _autoSpawnRoutine = null;
    private List<Spawnable> _allSpwnedObjects = new List<Spawnable>();


    private bool _spawnerStopped = false;
    
    void Start()
    {
        _allSpwnedObjects = new List<Spawnable>();
    }
    
    public void Spawn()
    {
        DoSpawnPrerequisite();

        
        
        _spawnerStopped = false;
        
        
        
        
        if (_spawnAutomaticaly)
        {
            if (_autoSpawnRoutine == null)
            {
                _autoSpawnRoutine = StartCoroutine(HandleAutomaticSpawning());
            }
        }
        else
        {
            SpawnRandomSpawnable();
        }
    }

    private void DoSpawnPrerequisite()
    {
        if (!_spawnAreaHasSet) SetSpawnArea();
    }
    
    private void SpawnRandomSpawnable()
    {
        var randomSpawnableIndex = Random.Range(0, _spawnables.Count);
        SpawnASpawnableOf(_spawnables[randomSpawnableIndex]);
    }
    
    private IEnumerator HandleAutomaticSpawning()
    {
        
        
        // Here's the problem
        
        if (_spawnAutomationSettings.StopSpawningAutomatically)
        {
            if (_spawnAutomaticaly)
            {
                _spawnAutomationSettings.OnSpawnStart();
            }
            else
            {
                _spawnAutomationSettings.OnSpawnStart();
            }
            
//            while (!_spawnAutomationSettings._limitationSettings.LimitationReached(_allSpwnedObjects.Count))
            while (!_spawnAutomationSettings._limitationSettings.LimitationReached(_allSpwnedObjects.Count) && !_spawnerStopped)
            {
                //////????????????????????????
                SpawnRandomSpawnable();
                yield return new WaitForSeconds(_spawnAutomationSettings.PerSpawnInterval);
            }
        }
        else
        {
            
            
            
            
            
            
            
            
//            while (true)
            while (!_spawnerStopped)
            {
                //////????????????????????????
//                Spawn();
                SpawnRandomSpawnable();

//                yield return new WaitForSeconds(_spawnAutomationSettings.PerSpawnInterval);
                yield return new WaitForSeconds(_spawnAutomationSettings.PerSpawnInterval);
            }
        }
    }
    
    public void StopSpawning()
    {


        _spawnerStopped = true;
        
        
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

    private  async void SpawnASpawnableOf(Spawnable spawnable)
    {
        var newSpawnable = InstantiateSpawnable(spawnable);

        if (_isCollisionSafe && !_collisionSafetySettings.IsGridPlacement)
        {
            var hasPlaced = await _collisionSafetySettings.PlaceSpawnable(newSpawnable, (UniTask) =>
            {
                PlaceSpawnable(newSpawnable);
            });

            if (!hasPlaced)
            {
                _allSpwnedObjects.Remove(newSpawnable);
                Destroy(newSpawnable.gameObject);
            }
        }
        else
        {
            if (_collisionSafetySettings.IsGridPlacement)
            {
                _collisionSafetySettings.GridPlacementSettings.CalculateCellSizeWithPadding();


                _collisionSafetySettings.GridPlacementSettings.SetSpawnableSize(newSpawnable);
                
                
                _collisionSafetySettings.GridPlacementSettings.PlaceSpawnableGridly(newSpawnable, _allSpwnedObjects.Count);
            }
            else
            {
                PlaceSpawnable(newSpawnable);
            }
        }
        
        ReleaseSpawnable(newSpawnable);
    }
    
    private Spawnable InstantiateSpawnable(Spawnable spawnable)
    {
        var newSpawnable = Instantiate(spawnable);
        
        newSpawnable.SetTag(_spawnableTag);

        newSpawnable.enabled = false;
        newSpawnable.IsCollisionSafe = _isCollisionSafe;
        newSpawnable.IsPlacement = _collisionSafetySettings.IsPlacement;
        if (_isCollisionSafe) newSpawnable.ColliderRequired = true;

        
//        newSpawnable.SetTag(_spawnableTag);
        
//        
//        if (_collisionSafetySettings.IsGridPlacement)
//        {
//            newSpawnable.ApplySize(_collisionSafetySettings.GridPlacementSettings.SpawnableSize);
//        }
//        

        newSpawnable.enabled = true;
        return newSpawnable;
    }

    private void PlaceSpawnable(Spawnable spawnable)
    {
        var randomX = 0.0f;
        var randomY = 0.0f;
  
        randomX = Math.Abs(_spawnArea.x - _spawnArea.z) < 0.1f ? _spawnArea.x : Random.Range(_spawnArea.x, _spawnArea.z);
        randomY = Math.Abs(_spawnArea.y - _spawnArea.w) < 0.1f ? _spawnArea.y : Random.Range(_spawnArea.y, _spawnArea.w);

        spawnable.transform.position = new Vector3(randomX, randomY, 0);
    }

    private async void ReleaseSpawnable(Spawnable spawnable)
    {
        var _hasPlaced = true;
        
        if (_isCollisionSafe && !_collisionSafetySettings.IsGridPlacement)
        {
            _hasPlaced = await  _collisionSafetySettings.ReleaseSpawnable(spawnable, (UniTask) => PlaceSpawnable(spawnable));
        }

        if (_hasPlaced)
        {
            spawnable.Release();
            _allSpwnedObjects.Add(spawnable);
        }
        else
        {
            Destroy(spawnable.gameObject);
        }
    }

    public void OnSpawnableSpawned()
    {
        _onSpawnableSpawnedEvent?.Invoke();
    }
}
}