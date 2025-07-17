using System;
using UnityEngine;
using Random = UnityEngine.Random;

//using Random = System.Random;

//using Random = Unity.Mathematics.Random;

public class BasicSpawner2D : MonoBehaviour
{
    private Vector4 _spawnArea;


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
        

        if (Math.Abs(_spawnArea.x - _spawnArea.z) < 0.1f)
        {
            randomX = _spawnArea.x;
        }
        else
        {
            randomX = Random.Range(_spawnArea.x, _spawnArea.y);
        }
        
        if (Math.Abs(_spawnArea.y - _spawnArea.w) < 0.1f)
        {
            randomY = _spawnArea.y;
        }
        else
        {
            randomY = Random.Range(_spawnArea.y, _spawnArea.w);
        }

        spawnable2D.transform.position = new Vector3(randomX, randomY, 0);

        spawnable2D.enabled = true;
//        do
//        {
//            
//        } while (spawnable2D.IsCol);
    }

    protected virtual void ReleaseSpawnable(BasicSpawnable2D spawnable2D)
    {
        spawnable2D.Release();
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
