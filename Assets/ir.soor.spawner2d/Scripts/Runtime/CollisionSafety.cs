using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Soor.Spawner2d
{
    /// <summary>
    /// Manages the placement of objects, ensuring they do not overlap with existing objects.
    /// Supports both random collision-safe placement and grid-based placement.
    /// </summary>
    [Serializable]
    public class CollisionSafety
    {
        #region SERIALIZED_FIELDS

        /// <summary>
        /// When true, enables a collision-safe placement method that avoids forcibly displacing
        /// or rotating other objects to make room for a new spawnable.
        /// </summary>
        [SerializeField] private bool _isPlacement;
        
        /// <summary>
        /// When true, enables grid-based placement instead of random collision-safe placement.
        /// This setting is only relevant if <see cref="_isPlacement"/> is enabled.
        /// </summary>
        [SerializeField] private bool _isGridPlacement;
        
        /// <summary>
        /// The settings used for grid-based placement, including cell size and padding.
        /// This is only used when <see cref="_isGridPlacement"/> is enabled.
        /// </summary>
        [SerializeField] private GridPlacement _gridPlacementSettings;

        #endregion SERIALIZED_FIELDS


        #region PROPERTIES

        /// <summary>
        /// Provides access to the grid placement settings.
        /// </summary>
        public GridPlacement GridPlacementSettings => _gridPlacementSettings;
        
        /// <summary>
        /// Indicates whether collision-safe placement is enabled.
        /// </summary>
        public bool IsPlacement => _isPlacement;
        
        /// <summary>
        /// Indicates whether grid-based placement is enabled.
        /// </summary>
        public bool IsGridPlacement => _isGridPlacement;

        #endregion PROPERTIES

        #region METHODS
        
        /// <summary>
        /// Attempts to find a safe, non-colliding position for a new object within a timeout period.
        /// </summary>
        /// <param name="spawnable">The object to be placed.</param>
        /// <param name="basePlaceSpawnable">A delegate that sets the object's position.</param>
        /// <returns>True if a position is found, otherwise false.</returns>
        public async UniTask<bool> PlaceSpawnable(Spawnable spawnable, Action<Spawnable> basePlaceSpawnable)
        {
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

        /// <summary>
        /// An asynchronous loop that repeatedly tries to place an object until a free, non-colliding space is found.
        /// </summary>
        /// <remarks>
        /// This method continues until a free spot is found or the operation is canceled.
        /// </remarks>
        /// <param name="collisionSafeSpawnable">The object to place.</param>
        /// <param name="cancellationToken">A token to manage the cancellation of the placement attempt.</param>
        /// <param name="basePlaceSpawnable">The delegate that places the object at a new random position.</param>
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

        /// <summary>
        /// Finalizes the placement of a spawned object. Re-attempts placement if the object is still colliding.
        /// </summary>
        /// <param name="spawnable">The object to be placed.</param>
        /// <param name="basePlaceSpawnable">A delegate to the base placement method.</param>
        /// <returns>True if placement is successful, otherwise false.</returns>
        public async UniTask<bool> ReleaseSpawnable(Spawnable spawnable, Action<Spawnable> basePlaceSpawnable)
        {
            if  (spawnable.IsCollided && IsPlacement)
            {
                return await PlaceSpawnable(spawnable, basePlaceSpawnable);
            }

            return true;
        }
        
        #endregion METHODS
    }
}