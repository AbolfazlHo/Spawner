using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Soor.Spawner2d
{
    /// <summary>
    /// Represents an object that can be spawned in the game.
    /// This component handles initial setup, collision detection for placement, and event management.
    /// </summary>
    public class Spawnable : MonoBehaviour
    {
        #region EVENTS

        /// <summary>
        /// Event triggered when the object is enabled.
        /// </summary>
        public UnityEvent onEnableEvent;
        
        /// <summary>
        /// Event triggered when the object is released and fully placed in the scene.
        /// </summary>
        public UnityEvent onRelease;
        
        /// <summary>
        /// Event triggered when the object is disabled.
        /// </summary>
        public UnityEvent onDisableEvent;
        
        #endregion EVENTS


        #region SERIALIZED_FIELDS

        /// <summary>
        /// The renderer component of the spawnable object.
        /// </summary>
        [SerializeField] private SpriteRenderer _spriteRenderer;

        #endregion SERIALIZED_FIELDS

        #region PROPERTIES

        /// <summary>
        /// Gets or sets a value indicating whether collision-safe placement is enabled for this object.
        /// </summary>
        public bool IsCollisionSafe { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether a collider component is required.
        /// </summary>
        public bool ColliderRequired { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether this object is in placement mode.
        /// </summary>
        public bool IsPlacement { get; set; }
        
        /// <summary>
        /// Returns true if the object is currently colliding with another object with the same tag.
        /// </summary>
        public bool IsCollided => _isCollided;

        #endregion PROPERTIES


        #region FIELDS

        /// <summary>
        /// The tag used for collision detection during the placement process.
        /// It ensures an object only detects collisions with other objects that share the same tag.
        /// </summary>
        private string _tag = "SoorSpawnable";
        
        /// <summary>
        /// The collider component of the object.
        /// </summary>
        private Collider2D _collider;
        
        /// <summary>
        /// The Rigidbody2D component of the object.
        /// </summary>
        private Rigidbody2D _rigidbody2D;
        
        /// <summary>
        /// A flag to track if the object is currently in a collision state.
        /// </summary>
        private bool _isCollided = false;
        
        /// <summary>
        /// The default 'isTrigger' state of the collider, saved for restoration.
        /// </summary>
        private bool _defaultIsTrigger;

        #endregion FIELDS


        #region PUBLIC_METHODS

        /// <summary>
        /// Sets a new tag for the object and ensures it exists in Unity's tag manager.
        /// </summary>
        /// <param name="intendedTag">The new tag to be assigned.</param>
        public void SetTag(string intendedTag)
        {
            _tag = tag;
            
#if UNITY_EDITOR || UNITY_EDITOR_OSX
            var tags = UnityEditorInternal.InternalEditorUtility.tags;

            if (!tags.ToList().Contains(_tag))
            {
                UnityEditorInternal.InternalEditorUtility.AddTag(_tag);
            }
#endif

            gameObject.tag = _tag;
        }
        
        /// <summary>
        /// Finalizes the object's placement and makes it interactive in the scene.
        /// </summary>
        public void Release()
        {
            if (IsCollisionSafe) ReturnCollisionPropertiesToDefault();
            _spriteRenderer.enabled = true;
            onRelease?.Invoke();
        }
        
        /// <summary>
        /// Sets the size of the object's sprite and collider for grid-based placement.
        /// </summary>
        /// <param name="size">The target size for the object.</param>
        public void SetSize(Vector2 size)
        {
            if (_spriteRenderer.drawMode != SpriteDrawMode.Sliced)
            {
                Debug.LogWarning("Grid-based spawning requires the SpriteRenderer's 'Draw Mode' to be 'Sliced' for correct sizing and alignment.");
            }

            _spriteRenderer.size = size;

            if (!(_collider is BoxCollider2D))
            {
                Debug.LogWarning("The Spawner requires a BoxCollider2D component on the spawnable object to correctly handle grid-based sizing.");
            }
            
            (_collider as BoxCollider2D).size = size;
        }

        #endregion PUBLIC_METHODS


        #region PRIVATE_METHODS

        /// <summary>
        /// Retrieves the collider component, adding a BoxCollider2D if one does not exist.
        /// </summary>
        private void GetCollider()
        {
            if (_collider == null)
            {
                _collider = GetComponent<Collider2D>();
            }

            if (_collider == null)
            {
                _collider = gameObject.AddComponent<BoxCollider2D>();
            }

            _collider.enabled = true;
        }

        /// <summary>
        /// Ensures the object has a Rigidbody2D component and sets its sleep mode.
        /// </summary>
        private void SetRigidbody2DSleepMode()
        {
            if (_rigidbody2D == null)
            {
                _rigidbody2D = GetComponent<Rigidbody2D>();
            }

            if (_rigidbody2D == null)
            {
                _rigidbody2D = gameObject.AddComponent<Rigidbody2D>();
            }

            _rigidbody2D.sleepMode = RigidbodySleepMode2D.StartAwake;
            _rigidbody2D.WakeUp();
        }

        /// <summary>
        /// Handles the physics infrastructure needed for collision detection during placement.
        /// </summary>
        private void SetCollisionInfrastructure()
        {
            _defaultIsTrigger = _collider.isTrigger;
            _collider.isTrigger = true;

            if (IsPlacement)
            {
                _rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
        }

        /// <summary>
        /// Restores the collider's properties to their default state after placement.
        /// </summary>
        private void ReturnCollisionPropertiesToDefault()
        {
            _collider.isTrigger = _defaultIsTrigger;
        }

        /// <summary>
        /// Retrieves the renderer component, disabling it by default.
        /// </summary>
        private void GetRenderer()
        {
            if (_spriteRenderer == null) _spriteRenderer = GetComponent<SpriteRenderer>();
            _spriteRenderer.enabled = false;
        }

        #endregion PRIVATE_METHODS


        #region MONOBEHAVIOUR_METHODS

        private void OnEnable()
        {
            GetRenderer();
            _spriteRenderer.enabled = false;

            if (IsCollisionSafe)
            {
                _isCollided = false;
                GetCollider();
                SetRigidbody2DSleepMode();
                SetCollisionInfrastructure();
            }

            onEnableEvent?.Invoke();
        }

        private void OnDisable()
        {
            _spriteRenderer.enabled = false;
            onDisableEvent?.Invoke();
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(_tag)) _isCollided = true;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag(_tag)) _isCollided = false;
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag(_tag)) _isCollided = true;
        }

        #endregion MONOBEHAVIOUR_METHODS
    }
}