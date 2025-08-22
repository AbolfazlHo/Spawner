using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Soor.Pooler;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

namespace Soor.Spawner2d
{
    /// <summary>
    /// Main controller for spawning 2D objects in the scene.
    /// It manages features such as automated spawning, collision-safe placement, and grid-based object arrangement.
    /// </summary>
    [ExecuteAlways]
    public class Spawner : MonoBehaviour
    {
        #region SERIALIZED_FIELDS

        /// <summary>
        /// GameObject that defines the spawn area. Must have either a Collider2D or Renderer component to determine bounds.
        /// </summary>
        [SerializeField] private GameObject _spawnAreaGameObject;
        
        /// <summary>
        /// List of spawnable prefabs eligible for instantiation.
        /// </summary>
        [SerializeField] private List<Spawnable> _spawnables;
        
        /// <summary>
        /// A list of Prefab objects that the Spawner can instantiate and place in the scene.
        /// Each item in this list must have a <see cref="Spawnable"/> component.
        /// </summary>
        [SerializeField] private bool _spawnAutomatically;
        
        /// <summary>
        /// Settings for the automated spawning behavior, including intervals and limitations.
        /// This field is only used when the <see cref="_spawnAutomatically"/> flag is enabled.
        /// </summary>
        [SerializeField] private Automation _spawnAutomationSettings;
        
        /// <summary>
        /// If enabled, the spawner will find a non-overlapping position for each object.
        /// See <see cref="CollisionSafety"/> for related settings.
        /// </summary>
        [SerializeField] private bool _isCollisionSafe = false;
        
        /// <summary>
        /// Additional settings for collision safety, including distance-based checks or grid-based placement.
        /// This field is only relevant when <see cref="_isCollisionSafe"/> is enabled.
        /// </summary>
        [SerializeField] private CollisionSafety _collisionSafetySettings;
        
        /// <summary>
        /// Tag assigned to the GameObject of each spawned object.
        /// Primarily used for collision detection in placement mode.
        /// </summary>
        [SerializeField] private string _spawnableTag = "SoorSpawnable";

        /// <summary>
        /// When true, the spawner will use an object pool to manage instantiated objects instead of a direct Instantiate/Destroy approach.
        /// </summary>
        [SerializeField] private bool _useObjectPool = false;

        /// <summary>
        /// The settings for the object pool, which defines the objects to be pooled.
        /// This is only used when <see cref="_useObjectPool"/> is enabled.
        /// </summary>
        [SerializeField] private Pooler.Pooler _poolerSettings;
        
        #endregion SERIALIZED_FIELDS

        
        #region FIELDS
        
        /// <summary>
        /// Spawn area defined as (xMin, yMin, xMax, yMax) in world coordinates.
        /// </summary>
        private Vector4 _spawnArea;
        
        /// <summary>
        /// A flag to ensure that the spawn area is calculated only once,
        /// improving performance by preventing redundant calls to <see cref="SetSpawnArea"/>.
        /// </summary>
        private bool _spawnAreaHasSet = false;
        
        /// <summary>
        /// A reference to the active Coroutine for automated spawning, allowing it to be stopped.
        /// </summary>
        private Coroutine _autoSpawnRoutine = null;
        
        /// <summary>
        /// A list of all objects currently spawned, used for tracking and applying limitations.
        /// </summary>
        private List<Spawnable> _allSpawnedObjects = new List<Spawnable>();

        /// <summary>
        /// A flag used to signal a manual stop for the spawning process, typically set by calling <see cref="StopSpawning"/>.
        /// </summary>
        private bool _spawnerStopped = false;
        
        /// <summary>
        /// A reference to the active object pool instance, used to get and release objects efficiently.
        /// </summary>
        private ObjectPool<Poolable> _objectPool;
        
        #endregion FIELDS

        
        #region MONOBEHAVIOUR_METHODS

        private void Start()
        {
            _allSpawnedObjects = new List<Spawnable>();
        }

        private void Update()
        {
#if UNITY_EDITOR || UNITY_EDITOR_OSX
            if (!_useObjectPool) return;
            
            foreach (var spawnable in _spawnables)
            {
                var poolable = spawnable.gameObject.GetComponent<Poolable>();
                    
                if (poolable == null)
                {
                    poolable = spawnable.gameObject.AddComponent<Poolable>();
                }

                if (!_poolerSettings.ObjectsToPool.Contains(poolable))
                {
                    _poolerSettings.AddPoolablePrefab(poolable);
                }
            }
#endif
        }

        #endregion MONOBEHAVIOUR_METHODS

        
        #region PUBLIC_METHODS

        /// <summary>
        /// Initiates the spawning process based on the configured automation settings.
        /// </summary>
        /// <remarks>
        /// If <see cref="_spawnAutomaticaly"/> is true, this method starts a continuous spawning loop.
        /// If false, it spawns a single random object.
        /// </remarks>
        public void Spawn()
        {
            DoSpawnPrerequisite();
            _spawnerStopped = false;

            if (_spawnAutomatically)
            {
                if (_autoSpawnRoutine == null)
                {
                    _autoSpawnRoutine = StartCoroutine(HandleAutomaticSpawning());
                }
            }
            else
            {
                SpawnRandomSpawnable();
            }
        }
        
        /// <summary>
        /// Stops the active automatic spawning loop and invokes the `OnSpawnEnd` event.
        /// </summary>
        public void StopSpawning()
        {
            _spawnerStopped = true;

            if (_spawnAutomatically && _autoSpawnRoutine != null)
            {
                StopCoroutine(_autoSpawnRoutine);
                _spawnAutomationSettings.OnSpawnEnd();
            }
        }

        /// <summary>
        /// Changes the spawn area GameObject and sets the new area.
        /// </summary>
        /// <param name="newSpawnArea">The new GameObject to be used as the spawn area.</param>
        public void ChangeSpawnAreaGameObject(GameObject newSpawnArea)
        {
            _spawnAreaHasSet = false;
            _spawnAreaGameObject = newSpawnArea;
            SetSpawnArea();
        }
        
        #endregion PUBLIC_METHODS

        
        #region PRIVATE_METHODS
        
        /// <summary>
        /// Ensures all necessary spawning conditions are met before an object is spawned.
        /// It primarily calls <see cref="SetSpawnArea"/> if the <see cref="_spawnAreaHasSet"/> flag is false.
        /// </summary>
        private void DoSpawnPrerequisite()
        {
            if (!_spawnAreaHasSet) SetSpawnArea();
        }

        /// <summary>
        /// Selects a random <see cref="Spawnable"/> from the list and passes it to the
        /// <see cref="SpawnASpawnableOf"/> method for instantiation and placement.
        /// </summary>
        private void SpawnRandomSpawnable()
        {
            
            
            
            
            
            
            
            
            
            
            
            var randomSpawnableIndex = Random.Range(0, _spawnables.Count);
            SpawnASpawnableOf(_spawnables[randomSpawnableIndex]).GetAwaiter();
        }

        /// <summary>
        /// Calculates the spawning bounds from the assigned GameObject's Renderer or Collider.
        /// </summary>
        private void SetSpawnArea()
        {
            var horizontalBoundSize = 0.0f;
            var verticalBoundSize = 0.0f;
            var globalPositionofSpawnAreaGameObject = _spawnAreaGameObject.transform.position;
            var rendererComponentofSpawnAreaGameObject = _spawnAreaGameObject.GetComponent<Renderer>();

            if (rendererComponentofSpawnAreaGameObject != null)
            {
                horizontalBoundSize = rendererComponentofSpawnAreaGameObject.bounds.size.x;
                verticalBoundSize = rendererComponentofSpawnAreaGameObject.bounds.size.y;
            }
            else
            {
                var colliderComponentofSpawnAreaGameObject = _spawnAreaGameObject.GetComponent<Collider>();

                if (colliderComponentofSpawnAreaGameObject != null)
                {
                    horizontalBoundSize = colliderComponentofSpawnAreaGameObject.bounds.size.x;
                    verticalBoundSize = colliderComponentofSpawnAreaGameObject.bounds.size.y;
                }
                else
                {
                    Debug.LogError(
                        "Current Spawn Area GameObject does not have any kind of renderer or collider component." +
                        "Spawner component uses these components to set the spawn area.");
                    _spawnArea = Vector4.zero;
                    return;
                }
            }

            _spawnArea = new Vector4(
                globalPositionofSpawnAreaGameObject.x - horizontalBoundSize / 2.0f,
                globalPositionofSpawnAreaGameObject.y - verticalBoundSize / 2.0f,
                globalPositionofSpawnAreaGameObject.x + horizontalBoundSize / 2.0f,
                globalPositionofSpawnAreaGameObject.y + verticalBoundSize / 2.0f
            );

            _spawnAreaHasSet = true;
        }

        /// <summary>
        /// Asynchronously instantiates and places a new object from the provided prefab.
        /// This method handles collision-safe placement, grid-based arrangement, and object pooling,
        /// destroying the object if placement fails and the `_useObjectPool` is false.
        /// </summary>
        /// <param name="spawnable">The spawnable prefab to be instantiated.</param>
        private async Task SpawnASpawnableOf(Spawnable spawnable)
        {
            var newSpawnable = InstantiateSpawnable(spawnable);

            if (_isCollisionSafe)
            {
                if (!_collisionSafetySettings.IsGridPlacement)
                {
                    var hasPlaced = await _collisionSafetySettings.PlaceSpawnable(newSpawnable,
                        (UniTask) => { PlaceSpawnable(newSpawnable); });

                    if (!hasPlaced)
                    {
                        _allSpawnedObjects.Remove(newSpawnable);

                        if (_useObjectPool)
                        {
                            _objectPool.Release(newSpawnable.gameObject.GetComponent<Poolable>());
                        }
                        else
                        {
                            Destroy(newSpawnable.gameObject);
                        }
                    }
                }
                else
                {
                    _collisionSafetySettings.GridPlacementSettings.CalculateCellSizeWithPadding();
                    _collisionSafetySettings.GridPlacementSettings.SetSpawnableSize(newSpawnable);
                    _collisionSafetySettings.GridPlacementSettings.PlaceSpawnableGridly(newSpawnable, _allSpawnedObjects.Count);
                }
            }
            else
            {
                PlaceSpawnable(newSpawnable);
            }

            await ReleaseSpawnable(newSpawnable);
        }

        /// <summary>
        /// Instantiates a new object from the provided prefab and applies initial settings before returning it.
        /// </summary>
        /// <param name="spawnable">The prefab to instantiate.</param>
        /// <returns>The instantiated and configured object.</returns>
        private Spawnable InstantiateSpawnable(Spawnable spawnable)
        {
//            var newSpawnable = Instantiate(spawnable);

            Spawnable newSpawnable = null;


            if (_useObjectPool)
            {
                if (_objectPool == null)
                {
                    _poolerSettings.GenerateObjectPool();
                    _objectPool = _poolerSettings.ObjectPool;
                }

                var newPoolable = _objectPool.Get();
//                var newSpawnable = newPoolable.gameObject.GetComponent<Spawnable>();
                newSpawnable = newPoolable.gameObject.GetComponent<Spawnable>();
            }
            else
            {
//                var newSpawnable = Instantiate(spawnable);
                newSpawnable = Instantiate(spawnable);
            }



//            if (_objectPool == null)
//            {
//                _poolerSettings.GenerateObjectPool();
//                _objectPool = _poolerSettings.ObjectPool;
//            }
//
//
//
//
//            var newPoolable = _objectPool.Get();
//            var newSpawnable = newPoolable.gameObject.GetComponent<Spawnable>();
            
            
            
            
            newSpawnable.SetTag(_spawnableTag);
            newSpawnable.enabled = false;
            newSpawnable.IsCollisionSafe = _isCollisionSafe;
            newSpawnable.IsPlacement = _collisionSafetySettings.IsPlacement;
            if (_isCollisionSafe) newSpawnable.ColliderRequired = true;
            newSpawnable.enabled = true;
            return newSpawnable;
        }

        /// <summary>
        /// Places the Spawnable object at a random position within the spawning area.
        /// </summary>
        /// <param name="spawnable">The spawnable to be placed.</param>
        private void PlaceSpawnable(Spawnable spawnable)
        {
            var randomX = 0.0f;
            var randomY = 0.0f;

            randomX = Math.Abs(_spawnArea.x - _spawnArea.z) < 0.1f
                ? _spawnArea.x
                : Random.Range(_spawnArea.x, _spawnArea.z);
            randomY = Math.Abs(_spawnArea.y - _spawnArea.w) < 0.1f
                ? _spawnArea.y
                : Random.Range(_spawnArea.y, _spawnArea.w);

            spawnable.transform.position = new Vector3(randomX, randomY, 0);
        }

        /// <summary>
        /// Finalizes the object placement and adds it to the list of spawned objects.
        /// If collision-safe placement fails, the object is destroyed.
        /// </summary>
        /// <param name="spawnable">The spawnable object to be finalized.</param>
        private async Task ReleaseSpawnable(Spawnable spawnable)
        {
            var _hasPlaced = true;

            if (_isCollisionSafe && !_collisionSafetySettings.IsGridPlacement)
            {
                _hasPlaced =
                    await _collisionSafetySettings.ReleaseSpawnable(spawnable, (UniTask) => PlaceSpawnable(spawnable));
            }

            if (_hasPlaced)
            {
                spawnable.Release();
                _allSpawnedObjects.Add(spawnable);
            }
            else
            {
//                Destroy(spawnable.gameObject);





                if (_useObjectPool)
                {
                    _objectPool.Release(spawnable.gameObject.GetComponent<Poolable>());
                }
                else
                {
                    Destroy(spawnable.gameObject);
                }
                
                
                
                
            }
        }
        
        #endregion PRIVATE_METHODS

        
        #region COROUTINES
        
        /// <summary>
        /// Manages the continuous spawning of objects at a set interval.
        /// The coroutine stops when a limitation is reached or when manually stopped.
        /// </summary>
        /// <returns>An IEnumerator for use with StartCoroutine.</returns>
        private IEnumerator HandleAutomaticSpawning()
        {
            if (_spawnAutomationSettings.StopSpawningAutomatically)
            {
                _spawnAutomationSettings.OnSpawnStart();

                while (!_spawnAutomationSettings.LimitationSettings.LimitationReached(_allSpawnedObjects.Count) && !_spawnerStopped)
                {
                    SpawnRandomSpawnable();
                    yield return new WaitForSeconds(_spawnAutomationSettings.PerSpawnInterval);
                }
            }
            else
            {
                while (!_spawnerStopped)
                {
                    SpawnRandomSpawnable();
                    yield return new WaitForSeconds(_spawnAutomationSettings.PerSpawnInterval);
                }
            }
        }

        #endregion COROUTINES
    }
}