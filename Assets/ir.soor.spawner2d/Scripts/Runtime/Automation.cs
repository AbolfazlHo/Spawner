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

        #region EVENTS

        public UnityEvent onSpawnStartEvent = new UnityEvent();
        public UnityEvent onSpawnEndEvent = new UnityEvent();

        #endregion EVENTS
        
        

        public bool StopSpawningAutomatically => _stopSpawningAutomatically;
        public float PerSpawnInterval => _perSpawnInterval;
        public Limitation LimitationSettings => _limitationSettings;

        public void OnSpawnStart()
        {
            onSpawnStartEvent?.Invoke();
        
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
                onSpawnEndEvent?.Invoke();
            }
        }
    }
}