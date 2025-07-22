using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

[Serializable]
public class CollisionSafety
{
    [SerializeField] private bool _isPlacement;
    
    public bool IsPlacement
    {
        get => _isPlacement;
    }


    // Placement settings . . .

    [SerializeField] private bool _usePreExistedCollider;


    public bool UsePreExistedCollider => _usePreExistedCollider;
    
        
        
        
        
    [SerializeField] private bool _isGridPlacement;
    
    public async UniTask<bool> PlaceSpawnable(Spawnable spawnable, Action<Spawnable> basePlaceSpawnable)
    {
        spawnable.IsCollisionSafe = true;
        spawnable.UsePreExistedCollider = _usePreExistedCollider;
        spawnable.IsPlacement = _isPlacement;
        
        var cancellationTokenSource = new CancellationTokenSource();
        
        try
        {
            cancellationTokenSource.CancelAfter(200);
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
        if (!_isPlacement)
        {
            basePlaceSpawnable(collisionSafeSpawnable);
        }
        else
        {
            do
            {
                cancellationToken.ThrowIfCancellationRequested();
                basePlaceSpawnable(collisionSafeSpawnable);
                await UniTask.Delay(100);
            } while (collisionSafeSpawnable.IsCollided);
        }
        
    }

//    public async void ReleaseSpawnable(Spawnable spawnable, Action<Spawnable> basePlaceSpawnable)
    public async UniTask<bool> ReleaseSpawnable(Spawnable spawnable, Action<Spawnable> basePlaceSpawnable)
    {
        if  (spawnable.IsCollided && IsPlacement)
        {
            return await PlaceSpawnable(spawnable, basePlaceSpawnable);
        }

        return true;
    }
}