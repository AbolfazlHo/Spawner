using UnityEngine;

public class PoolableTest : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    
    public void OnPoolableReleased()
    {
        Debug.Log("OnPoolableReleased      ----------------      OnPoolableReleased        ------------------        OnPoolableReleased");
    }

    public void OnPoolableDestroyed()
    {
        Debug.Log("OnPoolableDestroyed      +++++++++++++      OnPoolableDestroyed        +++++++++++++        OnPoolableDestroyed");

    }
    
}
