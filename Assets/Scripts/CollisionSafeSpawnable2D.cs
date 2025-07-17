using System;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;

public abstract class CollisionSafeSpawnable2D : BasicSpawnable2D
{

    private bool _isCollided = false;

    public bool IsCollided => _isCollided;

    protected Collider2D _collider;

//    [SerializeField]
//    protected Renderer _renderer;


    protected virtual void OnEnable()
    {

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

//        if (_renderer == null)
//        {
//            _renderer = GetComponent<Renderer>();
//        }

//        _renderer.enabled = false;
        _collider.enabled = true;

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
}
