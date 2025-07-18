using UnityEngine;

public class TestScript : MonoBehaviour
{

    [SerializeField]
    private ManualBasicSpawner2D _spawner;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("_spawner.Spawn();");
            _spawner.Spawn();
        }
    }
}
