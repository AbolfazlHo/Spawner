using UnityEngine;

public class PreExistedColliderCollisionSafePlacementSpawnable2D : PreExistedColliderCollisionSafeDisplacementSpawnable2D
{

    protected RigidbodyConstraints2D _constraints;
    
    protected override void SetCollisionInfrastructure()
    {
        base.SetCollisionInfrastructure();
        _constraints = _rigidbody2D.constraints;
        _rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    protected override void ReturnCollisionPropertiesToDefault()
    {
        base.ReturnCollisionPropertiesToDefault();
//        _rigidbody2D.constraints = _constraints;
    }
}
