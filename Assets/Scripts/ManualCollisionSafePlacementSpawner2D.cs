using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ManualCollisionSafePlacementSpawner2D : ManualBasicSpawner2D
{
    //ToDo: Think about cancellation token
    protected override async UniTask PlaceSpawnable(BasicSpawnable2D spawnable2D)
    {
        var collisionSafeSpawnable = spawnable2D as CollisionSafeSpawnable2D;
        var cancellationTokenSource = new CancellationTokenSource();
        
        try
        {
//            cancellationTokenSource.CancelAfter(1000);
            cancellationTokenSource.CancelAfter(500);
            await TryPlaceCollisionSafeSpawnable2DInAFreeSpace(collisionSafeSpawnable, cancellationTokenSource.Token);
        }
        catch (OperationCanceledException)
        {
//            Debug.Log("Try place failed.");

            _allSpwnedObjects.Remove(spawnable2D);
            
            Destroy(spawnable2D.gameObject);
            
        }
        finally
        {
            cancellationTokenSource.Dispose();
            cancellationTokenSource = null;
        }
        
        
        
//        do
//        {
//            Debug.Log("IsCollided");
//            await base.PlaceSpawnable(collisionSafeSpawnable);
//
//            await UniTask.Delay(100);
//
//        } while (collisionSafeSpawnable.IsCollided);
    }

//    private void OnDestroy()
//    {
//        throw new NotImplementedException();
//    }

    protected async UniTask TryPlaceCollisionSafeSpawnable2DInAFreeSpace(CollisionSafeSpawnable2D collisionSafeSpawnable, CancellationToken cancellationToken)
    {
        do
        {
            cancellationToken.ThrowIfCancellationRequested();
            
//            Debug.Log("IsCollided");
            await base.PlaceSpawnable(collisionSafeSpawnable);

            await UniTask.Delay(100);

        } while (collisionSafeSpawnable.IsCollided);
    }

    //ToDo: Think about cancellation token
//    protected override void ReleaseSpawnable(BasicSpawnable2D spawnable2D)
    protected override async void ReleaseSpawnable(BasicSpawnable2D spawnable2D)
    {

        if  ((spawnable2D as CollisionSafeSpawnable2D).IsCollided)
        {
            await PlaceSpawnable(spawnable2D);
        }
        
        
//        
//        while ((spawnable2D as CollisionSafeSpawnable2D).IsCollided)
//        {
////            Debug.Log("PlaceSpawnable");
//            
//            await PlaceSpawnable(spawnable2D);
//            
//            await UniTask.Delay(200);
//        }
        
        base.ReleaseSpawnable(spawnable2D);
    }
}
