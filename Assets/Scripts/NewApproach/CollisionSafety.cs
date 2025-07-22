using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

[Serializable]
public class CollisionSafety
{
    [SerializeField] private bool _isPlacement;
    [SerializeField] private GridPlacement _gridPlacementSettings;

    public GridPlacement GridPlacementSettings => _gridPlacementSettings;

    public bool IsPlacement => _isPlacement;
        
    [SerializeField] private bool _isGridPlacement;

    public bool IsGridPlacement => _isGridPlacement;

//    public async UniTask<bool> PlaceSpawnable(Spawnable spawnable, Action<Spawnable> basePlaceSpawnable)
//    public async UniTask<bool> PlaceSpawnable(Spawnable spawnable, Action<Spawnable> basePlaceSpawnable, int index)
    public async UniTask<bool> PlaceSpawnable(Spawnable spawnable, Action<Spawnable> basePlaceSpawnable)
    {


//
//        ///////////////////////////////////////////////////////////////////////////
//        if (_isGridPlacement)
//        {
//            _gridPlacementSettings.CalculateCellSizeWithPadding();
//            _gridPlacementSettings.PlaceSpawnableGridly(spawnable, index);
//            return true;
//        }
//        ///////////////////////////////////////////////////////////////////////////
//
//        
        
        
        
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

//    public async UniTask<bool> ReleaseSpawnable(Spawnable spawnable, Action<Spawnable> basePlaceSpawnable)
//    public async UniTask<bool> ReleaseSpawnable(Spawnable spawnable, Action<Spawnable> basePlaceSpawnable, int index)
    public async UniTask<bool> ReleaseSpawnable(Spawnable spawnable, Action<Spawnable> basePlaceSpawnable)
    {
        if  (spawnable.IsCollided && IsPlacement)
        {
//            return await PlaceSpawnable(spawnable, basePlaceSpawnable);
//            return await PlaceSpawnable(spawnable, basePlaceSpawnable, index);
            return await PlaceSpawnable(spawnable, basePlaceSpawnable);
        }

        return true;
    }
}