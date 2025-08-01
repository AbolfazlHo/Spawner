using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Soor.Spawner2d
{
    /// <summary>
    /// Main controller for spawning 2D objects in the scene.
    /// It manages features such as automated spawning, collision-safe placement, and grid-based object arrangement.
    /// </summary>
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
        /// This field is only used when the <see cref="_spawnAutomaticaly"/> flag is enabled.
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
        private List<Spawnable> _allSpwnedObjects = new List<Spawnable>();

        /// <summary>
        /// A flag used to signal a manual stop for the spawning process, typically set by calling <see cref="StopSpawning"/>.
        /// </summary>
        private bool _spawnerStopped = false;
        
        #endregion FIELDS

        
        #region MONOBEHAVIOUR_METHODS

        private void Start()
        {
            _allSpwnedObjects = new List<Spawnable>();
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
        /// Performs spawning prerequisites, such as setting the spawn area on the first run.
        /// </summary>
        private void DoSpawnPrerequisite()
        {
            if (!_spawnAreaHasSet) SetSpawnArea();
        }

        /// <summary>
        /// Spawns a random Spawnable object from the `_spawnables` list.
        /// </summary>
        private void SpawnRandomSpawnable()
        {
            var randomSpawnableIndex = Random.Range(0, _spawnables.Count);
            SpawnASpawnableOf(_spawnables[randomSpawnableIndex]);
        }

        /// <summary>
        /// Sets the spawn area based on the Renderer or Collider component of `_spawnAreaGameObject`.
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
        /// Spawns a Spawnable object and places it in the scene based on collision safety settings.
        /// </summary>
        /// <param name="spawnable">The spawnable to be spaened.</param>
        private async void SpawnASpawnableOf(Spawnable spawnable)
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
                        _allSpwnedObjects.Remove(newSpawnable);
                        Destroy(newSpawnable.gameObject);
                    }
                }
                else
                {
                    _collisionSafetySettings.GridPlacementSettings.CalculateCellSizeWithPadding();
                    _collisionSafetySettings.GridPlacementSettings.SetSpawnableSize(newSpawnable);
                    _collisionSafetySettings.GridPlacementSettings.PlaceSpawnableGridly(newSpawnable, _allSpwnedObjects.Count);
                }
            }
            else
            {
                PlaceSpawnable(newSpawnable);
            }

            ReleaseSpawnable(newSpawnable);
        }

        /// <summary>
        /// Instantiates a new instance of the given Spawnable and applies initial settings.
        /// </summary>
        /// <param name="spawnable">The spawnable to instantiate.</param>
        /// <returns>The instantiated spawnable.</returns>
        private Spawnable InstantiateSpawnable(Spawnable spawnable)
        {
            var newSpawnable = Instantiate(spawnable);
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
        /// Performs the final placement of the Spawnable object and adds it to the list of spawned objects.
        /// </summary>
        /// <param name="spawnable">The spawnable to be released.</param>
        private async void ReleaseSpawnable(Spawnable spawnable)
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
                _allSpwnedObjects.Add(spawnable);
            }
            else
            {
                Destroy(spawnable.gameObject);
            }
        }
        
        #endregion PRIVATE_METHODS

        
        #region COROUTINES
        
        /// <summary>
        /// Coroutine that handles the continuous automatic spawning process based on the `Automation` settings.
        /// Spawns objects at regular intervals until a limitation is reached or the process is manually stopped.
        /// </summary>
        /// <returns>An IEnumerator to be used with StartCoroutine.</returns>
        private IEnumerator HandleAutomaticSpawning()
        {
            if (_spawnAutomationSettings.StopSpawningAutomatically)
            {
                if (_spawnAutomatically) _spawnAutomationSettings.OnSpawnStart();
                else _spawnAutomationSettings.OnSpawnStart();

                while (!_spawnAutomationSettings.LimitationSettings.LimitationReached(_allSpwnedObjects.Count) && !_spawnerStopped)
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