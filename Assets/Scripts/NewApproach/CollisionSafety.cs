using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

[Serializable]
public class CollisionSafety
{
    [SerializeField] private bool _isPlacement;

    [SerializeField] private bool _isGridPlacement;
    // GridPlacement _gridPlacement
    
    
    
    
    
    
    
    
    
    
//    protected async UniTask PlaceSpawnable(BasicSpawnable2D spawnable)
//    public async UniTask<bool> PlaceSpawnable(Spawnable spawnable)
    public async UniTask<bool> PlaceSpawnable(Spawnable spawnable, Action<Spawnable> basePlaceSpawnable)
    {

        spawnable.IsCollisionSafe = true;
        
//        var collisionSafeSpawnable = spawnable as CollisionSafeSpawnable2D;
        var cancellationTokenSource = new CancellationTokenSource();
        
        try
        {
            cancellationTokenSource.CancelAfter(500);
//            await TryPlaceCollisionSafeSpawnable2DInAFreeSpace(collisionSafeSpawnable, cancellationTokenSource.Token);
//            await TryPlaceCollisionSafeSpawnable2DInAFreeSpace(spawnable, cancellationTokenSource.Token);
            await TryPlaceCollisionSafeSpawnable2DInAFreeSpace(spawnable, cancellationTokenSource.Token, basePlaceSpawnable);
            return true;
        }
        catch (OperationCanceledException)
        {
            return false;
            
//            _allSpwnedObjects.Remove(spawnable);
//            Destroy(spawnable.gameObject);
        }
        finally
        {
            cancellationTokenSource.Dispose();
            cancellationTokenSource = null;
        }
    }

//    protected async UniTask TryPlaceCollisionSafeSpawnable2DInAFreeSpace(CollisionSafeSpawnable2D collisionSafeSpawnable, CancellationToken cancellationToken)
//    private async UniTask TryPlaceCollisionSafeSpawnable2DInAFreeSpace(Spawnable collisionSafeSpawnable, CancellationToken cancellationToken)
    private async UniTask TryPlaceCollisionSafeSpawnable2DInAFreeSpace(Spawnable collisionSafeSpawnable, CancellationToken cancellationToken, Action<Spawnable> basePlaceSpawnable)
    {
        do
        {
            cancellationToken.ThrowIfCancellationRequested();
//            await base.PlaceSpawnable(collisionSafeSpawnable);
            basePlaceSpawnable(collisionSafeSpawnable);
            await UniTask.Delay(100);
//        } while (collisionSafeSpawnable.IsCollided);
        } while (collisionSafeSpawnable.IsCollided);
    }

//    protected override async void ReleaseSpawnable(BasicSpawnable2D spawnable)
    public async void ReleaseSpawnable(Spawnable spawnable, Action<Spawnable> basePlaceSpawnable)
    {
//        if  ((spawnable as CollisionSafeSpawnable2D).IsCollided)
        if  (spawnable.IsCollided)
        {
//            await PlaceSpawnable(spawnable);
            await PlaceSpawnable(spawnable, basePlaceSpawnable);
        }

//        base.ReleaseSpawnable(spawnable);
    }
    
    
    
    
    
    
}
