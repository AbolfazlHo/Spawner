using System;
using UnityEngine;
using UnityEngine.Events;

namespace Soor.Spawner2d
{
    [Serializable]
    public class Limitation
    {
        public enum LimitationType
        {
            Count = 0,
            Time = 1
        }

        [SerializeField] private LimitationType _limitationType = LimitationType.Count;
        [SerializeField] private float _limitTimeBy = 0;
        [SerializeField] private int _limitCountBy = 0;
        [SerializeField] private UnityEvent _onSpawnStartEvent = new UnityEvent();
        [SerializeField] private UnityEvent _onSpawnEndEvent = new UnityEvent();
        [SerializeField] private UnityEvent _onLimitationReachedEvent = new UnityEvent();

        private float _spawnStartTime = 0;

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

        public void OnSpawnStart()
        {
            if (_limitationType == LimitationType.Time)
            {
                _spawnStartTime = Time.time;
            }
        
            _onSpawnStartEvent?.Invoke();
        }

        public void OnLimitationReached()
        {
            _onLimitationReachedEvent?.Invoke();
            OnSpawnEnd();
        }

        public void OnSpawnEnd()
        {
            _onSpawnEndEvent?.Invoke();
        }
    }
}