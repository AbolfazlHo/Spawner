using System.Linq;
//using UnityEditorInternal;
using UnityEngine;

public abstract class CollisionSafeSpawnable2D : BasicSpawnable2D
{
    public bool IsCollided => _isCollided;
    protected Collider2D _collider;

    protected Rigidbody2D _rigidbody2D;
    protected bool _isCollided = false;
    
    protected void SetTag()
    {
              
#if UNITY_EDITOR
//        var tags = InternalEditorUtility.tags;
        var tags = UnityEditorInternal.InternalEditorUtility.tags;
        
        if (!tags.ToList().Contains("SoorSpawnable"))
        {
            UnityEditorInternal.InternalEditorUtility.AddTag("SoorSpawnable");
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
    
    protected override void OnEnable()
    {
        _isCollided = false;
        base.OnEnable();
        SetTag();
        GetCollider();
        SetRigidbody2DSleepMode();
        SetCollisionInfrastructure();
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

    public override void Release()
    {
        base.Release();
        ReturnCollisionPropertiesToDefault();
    }

    protected abstract void SetCollisionInfrastructure();
    protected abstract void ReturnCollisionPropertiesToDefault();
}