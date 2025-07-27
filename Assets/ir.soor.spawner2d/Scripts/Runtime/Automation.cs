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
        [SerializeField] public Limitation _limitationSettings;
    
        // ToDo: Hide the following events if limitation has set
        [SerializeField] private UnityEvent _onSpawnStartEvent;
        [SerializeField] private UnityEvent _onSpawnEndEvent;

        public bool StopSpawningAutomatically => _stopSpawningAutomatically;
        public float PerSpawnInterval => _perSpawnInterval;

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
            _onSpawnEndEvent?.Invoke();
        }
    }
}
