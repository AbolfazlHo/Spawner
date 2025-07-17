using System;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;

public abstract class CollisionSafeSpawnable2D : BasicSpawnable2D
{

    private bool _isCollided = false;

    public bool IsCollided => _isCollided;

    protected Collider2D _collider;

    private Rigidbody2D _rigidbody2D;

    protected override void OnEnable()
    {
        _isCollided = false;

        
        
        base.OnEnable();
        
#if UNITY_EDITOR
        var tags = InternalEditorUtility.tags;
        
        if (!tags.ToList().Contains("SoorSpawnable"))
        {
            InternalEditorUtility.AddTag("SoorSpawnable");
        }
#endif

        gameObject.tag = "SoorSpawnable";
        
        if (_collider == null)
        {
            _collider = GetComponent<Collider2D>();
        }

        _collider.enabled = true;

        SetCollisionInfrastructure();
        
        if (_rigidbody2D == null)
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        _rigidbody2D.sleepMode = RigidbodySleepMode2D.StartAwake;
        _rigidbody2D.WakeUp();

        // TODO: Set isTrigger "true" and ...
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("SoorSpawnable"))
        {
            _isCollided = true;
        }
    }
    
    protected void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("SoorSpawnable"))
        {
            _isCollided = false;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("SoorSpawnable"))
        {
            _isCollided = true;
        }
    }

    public override void OnReadyForUse()
    {
        base.OnReadyForUse();
        ReturnCollisionPropertiesToDefault();
    }

//    protected override void OnDisable()
//    {
//        base.OnDisable();
//        
//        
//    }

    protected abstract void SetCollisionInfrastructure();
    protected abstract void ReturnCollisionPropertiesToDefault();
}
