using System;
using UnityEngine;
using UnityEngine.Events;

public class BasicSpawnable2D : MonoBehaviour
{
    public UnityEvent onEnableEvent;
    public UnityEvent onSpawnDoneEvent;

    [SerializeField]
    protected Renderer _renderer;
    
    
    protected virtual void OnEnable()
    {
        if (_renderer == null)
        {
            _renderer = GetComponent<Renderer>();
        }
        
        _renderer.enabled = false;
        onEnableEvent?.Invoke();
    }

    public virtual void OnReadyForUse()
    {
        _renderer.enabled = true;
        onSpawnDoneEvent?.Invoke();
    }
}
