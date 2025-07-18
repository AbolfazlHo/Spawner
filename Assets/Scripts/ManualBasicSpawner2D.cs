using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ManualBasicSpawner2D : MonoBehaviour
{
    [SerializeField] private GameObject _spawnAreaGameObject;
    [SerializeField] private List<BasicSpawnable2D> _spawnables;
    
    private Vector4 _spawnArea;
    private bool _spawnAreaHasSet = false;
    
    private List<BasicSpawnable2D> _allSpwnedObjects = new List<BasicSpawnable2D>();

    void Start()
    {
        _allSpwnedObjects = new List<BasicSpawnable2D>();
    }

    public void Spawn()
    {
        if (!_spawnAreaHasSet) SetSpawnArea();
        var randomSpawnableIndex = Random.Range(0, _spawnables.Count - 1);
        SpawnASpawnableOf(_spawnables[randomSpawnableIndex]);
    }

    public void ChangeSpawnAreaGameObject(GameObject newSpawnArea)
    {
        _spawnAreaHasSet = false;
        _spawnAreaGameObject = newSpawnArea;
        SetSpawnArea();
    }

    private void SetSpawnArea()
    {
//        _spawnArea = _spawnAreaGameObject

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

    protected virtual void SpawnASpawnableOf(BasicSpawnable2D spawnable2DSource)
    {
        var spawnable = InstantiateSpawnable(spawnable2DSource);
        PlaceSpawnable(spawnable);
        ReleaseSpawnable(spawnable);
    }
    
    private BasicSpawnable2D InstantiateSpawnable(BasicSpawnable2D spawnable2DSource)
    {
        var newSpawnable = Instantiate(spawnable2DSource);
        return newSpawnable;
    }

    protected virtual void PlaceSpawnable(BasicSpawnable2D spawnable2D)
    {
        var randomX = 0.0f;
        var randomY = 0.0f;
        
        randomX = Math.Abs(_spawnArea.x - _spawnArea.z) < 0.1f ? _spawnArea.x : Random.Range(_spawnArea.x, _spawnArea.y);
        randomY = Math.Abs(_spawnArea.y - _spawnArea.w) < 0.1f ? _spawnArea.y : Random.Range(_spawnArea.y, _spawnArea.w);

        spawnable2D.transform.position = new Vector3(randomX, randomY, 0);
        spawnable2D.enabled = true;
    }

    protected virtual void ReleaseSpawnable(BasicSpawnable2D spawnable2D)
    {
        spawnable2D.Release();
        _allSpwnedObjects.Add(spawnable2D);
        // We can check the collision here again . . .
    }
}