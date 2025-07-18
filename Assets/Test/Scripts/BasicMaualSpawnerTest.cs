using UnityEngine;

public class BasicMaualSpawnerTest : MonoBehaviour
{

    [SerializeField]
    private ManualBasicSpawner2D _manualBasicSpawner2D;

    [SerializeField] private AutomaticBasicSpawner2D _automaticBasicSpawner2D;
    [SerializeField] private ManualCollisionSafeSpawner2D _manualCollisionSafeSpawner2D;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("_manualBasicSpawner2D.Spawn();");
            _manualBasicSpawner2D.Spawn();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("_automaticBasicSpawner2D.StartSpawningAutomatically()");
            _automaticBasicSpawner2D.StartSpawningAutomatically();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("_automaticBasicSpawner2D.StopSpawning()");
            _automaticBasicSpawner2D.StopSpawning();
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("_manualCollisionSafeSpawner2D.Spawn()");
            _manualCollisionSafeSpawner2D.Spawn();
        }
    }
}
