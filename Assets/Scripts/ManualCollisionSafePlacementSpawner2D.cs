using System;
using System.Threading;
using Cysharp.Threading.Tasks;

public class ManualCollisionSafePlacementSpawner2D : ManualBasicSpawner2D
{
    protected override async UniTask PlaceSpawnable(BasicSpawnable2D spawnable2D)
    {
        var collisionSafeSpawnable = spawnable2D as CollisionSafeSpawnable2D;
        var cancellationTokenSource = new CancellationTokenSource();
        
        try
        {
            cancellationTokenSource.CancelAfter(500);
            await TryPlaceCollisionSafeSpawnable2DInAFreeSpace(collisionSafeSpawnable, cancellationTokenSource.Token);
        }
        catch (OperationCanceledException)
        {
            _allSpwnedObjects.Remove(spawnable2D);
            Destroy(spawnable2D.gameObject);
        }
        finally
        {
            cancellationTokenSource.Dispose();
            cancellationTokenSource = null;
        }
    }

    protected async UniTask TryPlaceCollisionSafeSpawnable2DInAFreeSpace(CollisionSafeSpawnable2D collisionSafeSpawnable, CancellationToken cancellationToken)
    {
        do
        {
            cancellationToken.ThrowIfCancellationRequested();
            await base.PlaceSpawnable(collisionSafeSpawnable);
            await UniTask.Delay(100);
        } while (collisionSafeSpawnable.IsCollided);
    }

    protected override async void ReleaseSpawnable(BasicSpawnable2D spawnable2D)
    {
        if  ((spawnable2D as CollisionSafeSpawnable2D).IsCollided)
        {
            await PlaceSpawnable(spawnable2D);
        }

        base.ReleaseSpawnable(spawnable2D);
    }
}