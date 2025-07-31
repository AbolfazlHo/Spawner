using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Soor.Spawner2d
{
    /// <summary>
    /// The main class for controlling the spawning process of 2D objects in the scene.
    /// This component enables features like automatic spawning, collision safety, and grid-based placement.
    /// </summary>
    public class Spawner : MonoBehaviour
    {
        #region SERIALIZED_FIELDS

        /// <summary>
        /// The GameObject that defines the spawning area. It should have a Collider2D or Renderer component.
        /// </summary>
        [SerializeField] private GameObject _spawnAreaGameObject;
        
        /// <summary>
        /// A list of Spawnable prefabs that this spawner can create.
        /// </summary>
        [SerializeField] private List<Spawnable> _spawnables;
        
        /// <summary>
        /// Determines whether the spawning process should continue automatically after it started.
        /// </summary>
        [SerializeField] private bool _spawnAutomaticaly;
        
        /// <summary>
        /// Settings for the automatic spawning mode. This is used only when `_spawnAutomaticaly` is enabled.
        /// </summary>
        [SerializeField] private Automation _spawnAutomationSettings;
        
        /// <summary>
        /// Determines whether collision safety checks for spawned objects are active.
        /// </summary>
        [SerializeField] private bool _isCollisionSafe = false;
        
        /// <summary>
        /// Settings for collision safety, including simple placement and grid-based placement.
        /// </summary>
        [SerializeField] private CollisionSafety _collisionSafetySettings;
        
        /// <summary>
        /// A custom tag that is assigned to all spawned objects.
        /// </summary>
        [SerializeField] private string _spawnableTag = "SoorSpawnable";
        
        #endregion SERIALIZED_FIELDS

        
        #region FIELDS
        
        /// <summary>
        /// The spawning area stored as a Vector4 (xMin, yMin, xMax, yMax).
        /// </summary>
        private Vector4 _spawnArea;
        
        /// <summary>
        /// A flag to indicate if the spawn area has been set at least once.
        /// </summary>
        private bool _spawnAreaHasSet = false;
        
        /// <summary>
        /// A reference to the Coroutine that manages automatic spawning.
        /// </summary>
        private Coroutine _autoSpawnRoutine = null;
        
        /// <summary>
        /// A list of all Spawnable objects currently spawned by this Spawner.
        /// </summary>
        private List<Spawnable> _allSpwnedObjects = new List<Spawnable>();

        /// <summary>
        /// A flag to manually stop the spawning process.
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
        /// Spawns one spawnable if the process is not automatic. Spawns a series of spawnables based on `Automation Settings` if the process is automatic.
        /// </summary>
        public void Spawn()
        {
            DoSpawnPrerequisite();
            _spawnerStopped = false;

            if (_spawnAutomaticaly)
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
        /// Stops the automatic spawning process.
        /// </summary>
        public void StopSpawning()
        {
            _spawnerStopped = true;

            if (_spawnAutomaticaly && _autoSpawnRoutine != null)
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
        /// Coroutine that handles the automatic spawning process based on the `Automation` settings.
        /// </summary>
        /// <returns>Returned value of Coroutine in IEnumerator type.</returns>
        private IEnumerator HandleAutomaticSpawning()
        {
            if (_spawnAutomationSettings.StopSpawningAutomatically)
            {
                if (_spawnAutomaticaly) _spawnAutomationSettings.OnSpawnStart();
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