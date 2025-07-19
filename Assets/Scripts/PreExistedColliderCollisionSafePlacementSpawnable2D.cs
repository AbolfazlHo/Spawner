using UnityEngine;

public class PreExistedColliderCollisionSafePlacementSpawnable2D : PreExistedColliderCollisionSafeDisplacementSpawnable2D
{

    protected RigidbodyConstraints2D _constraints;
    
    protected override void SetCollisionInfrastructure()
    {
        
        Debug.Log("---------------     protected override void SetCollisionInfrastructure()      -------------------");
        
        base.SetCollisionInfrastructure();
        
        _constraints = _rigidbody2D.constraints;

//        var constraints = _rigidbody2D.constraints;
        
//        Debug.Log(constraints);
        
        _rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
        
        
//        (_rigidbody2D.constraints & RigidbodyConstraints2D.FreezeAll) != RigidbodyConstraints2D.FreezeAll = RigidbodyConstraints2D.FreezeAll

    }




    protected override void ReturnCollisionPropertiesToDefault()
    {
        base.ReturnCollisionPropertiesToDefault();
//        _rigidbody2D.constraints = _constraints;
    }
    
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
