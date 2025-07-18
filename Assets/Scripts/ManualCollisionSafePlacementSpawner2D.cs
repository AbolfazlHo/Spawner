using Cysharp.Threading.Tasks;
using UnityEngine;

public class ManualCollisionSafePlacementSpawner2D : ManualBasicSpawner2D
{
    //ToDo: Think about cancellation token
    protected override async UniTask PlaceSpawnable(BasicSpawnable2D spawnable2D)
    {
        var collisionSafeSpawnable = spawnable2D as CollisionSafeSpawnable2D;
        
        do
        {
            Debug.Log("IsCollided");
            await base.PlaceSpawnable(collisionSafeSpawnable);

            await UniTask.Delay(100);

        } while (collisionSafeSpawnable.IsCollided);
    }

//    protected override void ReleaseSpawnable(BasicSpawnable2D spawnable2D)
    protected override async void ReleaseSpawnable(BasicSpawnable2D spawnable2D)
    {
        while ((spawnable2D as CollisionSafeSpawnable2D).IsCollided)
        {
            Debug.Log("PlaceSpawnable");
            
            await PlaceSpawnable(spawnable2D);
            
            await UniTask.Delay(200);
        }
        
        base.ReleaseSpawnable(spawnable2D);
    }
}
