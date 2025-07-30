using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Soor.Spawner2d
{
    public class Spawnable : MonoBehaviour
    {
        public UnityEvent onEnableEvent;
        public UnityEvent onRelaese;
        public UnityEvent onDisableEvent;

        [SerializeField] private Renderer _renderer;

        private string _tag = "SoorSpawnable";
        
        public bool IsCollisionSafe { get; set; }
        public bool ColliderRequired { get; set; }
        public bool IsPlacement { get; set; }
        public bool IsCollided => _isCollided;

        private Collider2D _collider;
        private Rigidbody2D _rigidbody2D;
        private bool _isCollided = false;
        private bool _defaultIsTrigger;

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
                Debug.LogWarning("You should select Sliced draw mode in SpriteRenderer while you want to spanwn the spawnables gridly.");
            }

            (_renderer as SpriteRenderer).size = size;

            if (!(_collider is BoxCollider2D))
            {
                Debug.LogWarning("You should use BoxCollider2D while you want to spanwn the spawnables gridly.");
            }
            
            (_collider as BoxCollider2D).size = size;
        }
    }
}