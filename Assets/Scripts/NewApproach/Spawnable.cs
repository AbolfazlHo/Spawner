using System.Linq;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Events;

public class Spawnable : MonoBehaviour
{
    public UnityEvent onEnableEvent;
    public UnityEvent onRelaese;
    public UnityEvent onDisableEvent;

    [SerializeField]
    protected Renderer _renderer;
    
    public bool IsCollisionSafe { get; set; }
    public bool UsePreExistedCollider { get; set; }
    
    public bool IsCollided => _isCollided;
    private Collider2D _collider;

    private Rigidbody2D _rigidbody2D;
    private bool _isCollided = false;
    
    protected void SetTag()
    {
              
#if UNITY_EDITOR
        var tags = InternalEditorUtility.tags;
        
        if (!tags.ToList().Contains("SoorSpawnable"))
        {
            InternalEditorUtility.AddTag("SoorSpawnable");
        }
#endif

        gameObject.tag = "SoorSpawnable";
    }

    // ToDo: Remove the following method.
    // ToDo: Change the following method for generative collider.
    private void GetCollider()
    {
        if (_collider == null)
        {
            _collider = GetComponent<Collider2D>();
        }

        if (_collider == null)
        {
            _collider = gameObject.AddComponent<BoxCollider2D>();
        }

        _collider.enabled = true;
    }

    private void SetRigidbody2DSleepMode()
    {
        if (_rigidbody2D == null)
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        if (_rigidbody2D == null)
        {
            _rigidbody2D = gameObject.AddComponent<Rigidbody2D>();
        }

        _rigidbody2D.sleepMode = RigidbodySleepMode2D.StartAwake;
        _rigidbody2D.WakeUp();
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("protected void OnTriggerEnter2D(Collider2D other)");
        
        if (other.CompareTag("SoorSpawnable"))
        {
            _isCollided = true;
        }
    }
    
    protected void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("protected void OnTriggerExit2D(Collider2D other)");

        if (other.CompareTag("SoorSpawnable"))
        {
            _isCollided = false;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log(" private void OnTriggerStay2D(Collider2D other)");
   
        if (other.CompareTag("SoorSpawnable"))
        {
            _isCollided = true;
        }
    }

    private void SetCollisionInfrastructure()
    {
        if (UsePreExistedCollider)
        {
            
        }
        else
        {
            
        }
    }

    private void ReturnCollisionPropertiesToDefault()
    {
        Debug.Log("++++++++++++++++++         private void ReturnCollisionPropertiesToDefault()            ++++++++++++++++");
        
        _collider.isTrigger = false;
        
        if (UsePreExistedCollider)
        {
//            if (!_isPlacement)
//            {
//                
//            }
            _collider.isTrigger = false;
        }
        else
        {
            
        }
    }
    
    private void OnEnable()
    {
        GetRenderer();
        _renderer.enabled = false;
        
        if (IsCollisionSafe)
        {
            _isCollided = false;
            SetTag();
            GetCollider();
            SetRigidbody2DSleepMode();
            SetCollisionInfrastructure();
        }
        
//        GetRenderer();
//
//
//        _renderer.enabled = false;
        
        
        onEnableEvent?.Invoke();
    }

    public virtual void Release()
    {
        if (IsCollisionSafe)
        {
            ReturnCollisionPropertiesToDefault();
        }
        
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