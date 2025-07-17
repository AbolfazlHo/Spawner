using UnityEngine;
using UnityEngine.Events;

public class BasicSpawnable2D : MonoBehaviour
{
    public UnityEvent onEnableEvent;
//    public UnityEvent onSpawnDoneEvent;
    public UnityEvent onRelaese;
    public UnityEvent onDisableEvent;

    [SerializeField]
    protected Renderer _renderer;

//    protected bool _hasSpawned = false;

//    public bool HasSpawned => _hasSpawned;

    protected virtual void OnEnable()
    {
//        _hasSpawned = false;
        
//        if (_renderer == null)
//        {
//            _renderer = GetComponent<Renderer>();
//        }
//        
//        _renderer.enabled = false;


        GetRenderer();

        onEnableEvent?.Invoke();
    }

//    public virtual void OnReadyForUse()
    public virtual void Release()
    {
        _renderer.enabled = true;
//        _hasSpawned = true;
//        onSpawnDoneEvent?.Invoke();
        onRelaese?.Invoke();
    }

    protected virtual void OnDisable()
    {
        _renderer.enabled = false;
        onDisableEvent?.Invoke();
    }

    private void GetRenderer()
    {
        if (_renderer == null)
        {
            _renderer = GetComponent<Renderer>();
        }
        
        _renderer.enabled = false;
    }
}