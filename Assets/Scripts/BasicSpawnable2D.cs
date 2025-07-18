using UnityEngine;
using UnityEngine.Events;

public class BasicSpawnable2D : MonoBehaviour
{
    public UnityEvent onEnableEvent;
    public UnityEvent onRelaese;
    public UnityEvent onDisableEvent;

    [SerializeField]
    protected Renderer _renderer;

    protected virtual void OnEnable()
    {
        GetRenderer();
        onEnableEvent?.Invoke();
    }

    public virtual void Release()
    {
        _renderer.enabled = true;
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