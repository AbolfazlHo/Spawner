using System;
using UnityEngine;
using UnityEngine.Events;

namespace Soor.Spawner2d
{
    [Serializable]
    public class Automation
    {
        [Tooltip("Delay between two consecutive spawns in milliseconds.")]
        [SerializeField] private float _perSpawnInterval;
        [SerializeField] private bool _stopSpawningAutomatically;
        [SerializeField] private Limitation _limitationSettings;
        [SerializeField] private UnityEvent _onSpawnStartEvent = new UnityEvent();
        [SerializeField] private UnityEvent _onSpawnEndEvent = new UnityEvent();

        public bool StopSpawningAutomatically => _stopSpawningAutomatically;
        public float PerSpawnInterval => _perSpawnInterval;
        public Limitation LimitationSettings => _limitationSettings;

        public void OnSpawnStart()
        {
            _onSpawnStartEvent?.Invoke();
        
            if (_stopSpawningAutomatically)
            {
                _limitationSettings.OnSpawnStart();
            }
        }
    
        public void OnSpawnEnd()
        {
            if (_stopSpawningAutomatically)
            {
                _limitationSettings.OnSpawnEnd();
            }
            else
            {
                _onSpawnEndEvent?.Invoke();
            }
        }
    }
}