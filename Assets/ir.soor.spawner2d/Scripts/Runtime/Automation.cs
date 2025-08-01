using System;
using UnityEngine;
using UnityEngine.Events;

namespace Soor.Spawner2d
{
    /// <summary>
    /// Configures the automated spawning behavior of the spawner.
    /// Manages properties like spawn interval, and connects to limitation settings.
    /// </summary>
    [Serializable]
    public class Automation
    {
        #region SERIALIZED_FIELDS

        /// <summary>
        /// The delay in seconds between two consecutive spawns.
        /// </summary>
        [Tooltip("Delay between two consecutive spawns in milliseconds.")]
        [SerializeField] private float _perSpawnInterval;
        
        /// <summary>
        /// When true, the spawner will automatically stop spawning based on the defined limitations.
        /// </summary>
        [SerializeField] private bool _stopSpawningAutomatically;
        
        /// <summary>
        /// The settings used to define and manage the spawning limitations (e.g., max count, max time).
        /// This is only used when <see cref="_stopSpawningAutomatically"/> is enabled.
        /// </summary>
        [SerializeField] private Limitation _limitationSettings;

        #endregion SERIALIZED_FIELDS
        

        #region EVENTS

        /// <summary>
        /// Event triggered at the beginning of the automated spawning process.
        /// </summary>
        public UnityEvent onSpawnStartEvent = new UnityEvent();
        
        /// <summary>
        /// Event triggered when the automated spawning process ends.
        /// </summary>
        public UnityEvent onSpawnEndEvent = new UnityEvent();

        #endregion EVENTS


        #region PROPERTIES

        /// <summary>
        /// Indicates whether the spawner should automatically stop based on limitations.
        /// </summary>
        public bool StopSpawningAutomatically => _stopSpawningAutomatically;
        
        /// <summary>
        /// The time interval between each spawn.
        /// </summary>
        public float PerSpawnInterval => _perSpawnInterval;
        
        /// <summary>
        /// Provides access to the limitation settings.
        /// </summary>
        public Limitation LimitationSettings => _limitationSettings;

        #endregion PROPERTIES


        #region METHODS

        /// <summary>
        /// Initiates the spawning process, triggering the start event and the limitation setup if needed.
        /// </summary>
        public void OnSpawnStart()
        {
            onSpawnStartEvent?.Invoke();
        
            if (_stopSpawningAutomatically)
            {
                _limitationSettings.OnSpawnStart();
            }
        }
    
        /// <summary>
        /// Finalizes the spawning process, triggering the end event.
        /// </summary>
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

        #endregion METHODS
    }
}