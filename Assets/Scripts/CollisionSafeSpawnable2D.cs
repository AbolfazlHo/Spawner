using System.Linq;
using UnityEditorInternal;
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
        var tags = InternalEditorUtility.tags;
        
        if (!tags.ToList().Contains("SoorSpawnable"))
        {
            InternalEditorUtility.AddTag("SoorSpawnable");
        }
#endif

        gameObject.tag = "SoorSpawnable";
    }

    private void GetCollider()
    {
        if (_collider == null)
        {
            _collider = GetComponent<Collider2D>();
        }

        _collider.enabled = true;
    }

    private void SetRigidbody2DSleepMode()
    {
        if (_rigidbody2D == null)
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
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
//        SetRigidbody2DSleepMode();

        // TODO: Set isTrigger "true" and ...
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

//    public override void OnReadyForUse()
    public override void Release()
    {
//        base.OnReadyForUse();
        base.Release();
        ReturnCollisionPropertiesToDefault();
    }

    protected abstract void SetCollisionInfrastructure();
    protected abstract void ReturnCollisionPropertiesToDefault();
}