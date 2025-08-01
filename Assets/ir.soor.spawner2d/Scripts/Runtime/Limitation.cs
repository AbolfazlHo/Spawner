using System;
using UnityEngine;
using UnityEngine.Events;

namespace Soor.Spawner2d
{
    /// <summary>
    /// Manages the limitations for a spawning process, such as maximum object count or total duration.
    /// This component triggers events when the spawning starts, ends, or reaches a defined limit.
    /// </summary>
    [Serializable]
    public class Limitation
    {
        #region ENUMS

        /// <summary>
        /// Defines the type of limitation to be applied to the spawning process.
        /// </summary>
        public enum LimitationType
        {
            /// <summary>
            /// Limits the number of objects that can be spawned.
            /// </summary>
            Count = 0,
            
            /// <summary>
            /// Limits the total time for which spawning can occur.
            /// </summary>
            Time = 1
        }
        
        #endregion ENUMS

        
        #region SERIALIZED_FIELDS
        
        /// <summary>
        /// The type of limitation to apply (Count or Time).
        /// </summary>
        [SerializeField] private LimitationType _limitationType = LimitationType.Count;
        
        /// <summary>
        /// The maximum time (in seconds) for spawning.
        /// This is only used when <see cref="_limitationType"/> is set to Time.
        /// </summary>
        [SerializeField] private float _limitTimeBy = 0;
        
        /// <summary>
        /// The maximum number of objects to spawn.
        /// This is only used when <see cref="_limitationType"/> is set to Count.
        /// </summary>
        [SerializeField] private int _limitCountBy = 0;
        
        /// <summary>
        /// Event triggered at the beginning of the spawning process.
        /// </summary>
        [SerializeField] private UnityEvent _onSpawnStartEvent = new UnityEvent();
        
        /// <summary>
        /// Event triggered when the spawning process ends for any reason.
        /// </summary>
        [SerializeField] private UnityEvent _onSpawnEndEvent = new UnityEvent();
        
        /// <summary>
        /// Event triggered when the defined limitation (count or time) is reached.
        /// </summary>
        [SerializeField] private UnityEvent _onLimitationReachedEvent = new UnityEvent();

        #endregion SERIALIZED_FIELDS

        
        #region FIELDS

        /// <summary>
        /// The time at which the spawning process began, used for time-based limitation.
        /// </summary>
        private float _spawnStartTime = 0;

        #endregion FIELDS


        #region METHODS
        
        /// <summary>
        /// Checks if the spawning limitation has been reached.
        /// </summary>
        /// <param name="spawnedSpawnablesCount">The current number of spawned objects.</param>
        /// <returns>True if the limitation has been met, otherwise false.</returns>
        public bool LimitationReached(int spawnedSpawnablesCount)
        {
            var limitationReached = false;

            if (_limitationType == LimitationType.Count)
            {
                limitationReached = spawnedSpawnablesCount >= _limitCountBy;
            }
            else
            {
                limitationReached = Time.time - _spawnStartTime >= _limitTimeBy;
            }

            if (limitationReached)
            {
                OnLimitationReached();
            }
        
            return limitationReached;
        }

        /// <summary>
        /// Initializes the limitation process and triggers the `OnSpawnStart` event.
        /// </summary>
        public void OnSpawnStart()
        {
            if (_limitationType == LimitationType.Time)
            {
                _spawnStartTime = Time.time;
            }
        
            _onSpawnStartEvent?.Invoke();
        }

        /// <summary>
        /// Triggers the `OnLimitationReached` event and then calls <see cref="OnSpawnEnd"/>.
        /// </summary>
        public void OnLimitationReached()
        {
            _onLimitationReachedEvent?.Invoke();
            OnSpawnEnd();
        }

        /// <summary>
        /// Triggers the `OnSpawnEnd` event.
        /// </summary>
        public void OnSpawnEnd()
        {
            _onSpawnEndEvent?.Invoke();
        }
        
        #endregion METHODS
    }
}