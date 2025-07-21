using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

[Serializable]
public class CollisionSafety
{
    [SerializeField] private bool _isPlacement;
    [SerializeField] private bool _isGridPlacement;
    
    public async UniTask<bool> PlaceSpawnable(Spawnable spawnable, Action<Spawnable> basePlaceSpawnable)
    {
        spawnable.IsCollisionSafe = true;
        var cancellationTokenSource = new CancellationTokenSource();
        
        try
        {
            cancellationTokenSource.CancelAfter(500);
            await TryPlaceCollisionSafeSpawnable2DInAFreeSpace(spawnable, cancellationTokenSource.Token, basePlaceSpawnable);
            return true;
        }
        catch (OperationCanceledException)
        {
            return false;
        }
        finally
        {
            cancellationTokenSource.Dispose();
            cancellationTokenSource = null;
        }
    }

    private async UniTask TryPlaceCollisionSafeSpawnable2DInAFreeSpace(Spawnable collisionSafeSpawnable, CancellationToken cancellationToken, Action<Spawnable> basePlaceSpawnable)
    {
        do
        {
            cancellationToken.ThrowIfCancellationRequested();
            basePlaceSpawnable(collisionSafeSpawnable);
            await UniTask.Delay(100);
        } while (collisionSafeSpawnable.IsCollided);
    }

    public async void ReleaseSpawnable(Spawnable spawnable, Action<Spawnable> basePlaceSpawnable)
    {
        if  (spawnable.IsCollided)
        {
            await PlaceSpawnable(spawnable, basePlaceSpawnable);
        }
    }
}