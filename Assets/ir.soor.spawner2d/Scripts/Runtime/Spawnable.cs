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
        public UnityEvent onRelaese;
        
        /// <summary>
        /// Event triggered when the object is disabled.
        /// </summary>
        public UnityEvent onDisableEvent;
        
        #endregion EVENTS


        #region SERIALIZED_FIELDS

        /// <summary>
        /// The renderer component of the spawnable object.
        /// </summary>
        [SerializeField] private Renderer _renderer;

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
        /// The tag used for collision detection, ensuring it only detects other spawnable objects that spawned with a common `Spawner`.
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


        public void SetTag(string tag)
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

        protected void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(_tag)) _isCollided = true;
        }

        protected void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag(_tag)) _isCollided = false;
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag(_tag)) _isCollided = true;
        }

        private void SetCollisionInfrastructure()
        {
            _defaultIsTrigger = _collider.isTrigger;
            _collider.isTrigger = true;

            if (IsPlacement)
            {
                _rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
        }

        private void ReturnCollisionPropertiesToDefault()
        {
            _collider.isTrigger = _defaultIsTrigger;
        }

        private void OnEnable()
        {
            GetRenderer();
            _renderer.enabled = false;

            if (IsCollisionSafe)
            {
                _isCollided = false;
                GetCollider();
                SetRigidbody2DSleepMode();
                SetCollisionInfrastructure();
            }

            onEnableEvent?.Invoke();
        }

        public virtual void Release()
        {
            if (IsCollisionSafe) ReturnCollisionPropertiesToDefault();
            _renderer.enabled = true;
            onRelaese?.Invoke();
        }

        protected virtual void OnDisable()
        {
            _renderer.enabled = false;
            onDisableEvent?.Invoke();
        }

        private void GetRenderer()
        {
            if (_renderer == null) _renderer = GetComponent<Renderer>();
            _renderer.enabled = false;
        }

        public void SetSize(Vector2 size)
        {
            if ((_renderer as SpriteRenderer).drawMode != SpriteDrawMode.Sliced)
            {
                Debug.LogWarning("Grid-based spawning requires the SpriteRenderer's 'Draw Mode' to be 'Sliced' for correct sizing and alignment.");
            }

            (_renderer as SpriteRenderer).size = size;

            if (!(_collider is BoxCollider2D))
            {
                Debug.LogWarning("The Spawner requires a BoxCollider2D component on the spawnable object to correctly handle grid-based sizing.");
            }
            
            (_collider as BoxCollider2D).size = size;
        }
    }
}