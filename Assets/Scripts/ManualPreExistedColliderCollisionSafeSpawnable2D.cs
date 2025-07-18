using UnityEngine;

public class ManualPreExistedColliderCollisionSafeSpawnable2D : CollisionSafeSpawnable2D
{
    #region COLLIDER_PREMITIVE_PROPERTIES

    private bool _isTrigger = false;

    #endregion COLLIDER_PREMITIVE_PROPERTIES

    protected override void SetCollisionInfrastructure()
    {
        _isTrigger = _collider.isTrigger;
        _collider.isTrigger = true;
    }

    protected override void ReturnCollisionPropertiesToDefault()
    {
        _collider.isTrigger = _isTrigger;
    }
}
