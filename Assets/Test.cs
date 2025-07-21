using UnityEngine;

public class Test : MonoBehaviour
{


    [SerializeField] private Spawner _spawner;
    
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _spawner.Spawn();
        }
    }
}
