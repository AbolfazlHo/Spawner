using System;
using UnityEngine;
using UnityEngine.Events;

public class BasicSpawnable2D : MonoBehaviour
{
    public UnityEvent onSpawnDoneEvent;

    [SerializeField]
    protected Renderer _renderer;
    
    
    protected void OnEnable()
    {
        if (_renderer == null)
        {
            _renderer = GetComponent<Renderer>();
        }
        
        _renderer.enabled = false;
    }

    public virtual void OnReadyForUse()
    {
        onSpawnDoneEvent?.Invoke();
    }
}
