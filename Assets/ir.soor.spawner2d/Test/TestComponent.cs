using Soor.Spawner2d;
using UnityEngine;

namespace Test
{
    public class TestComponent : MonoBehaviour
    {
        [SerializeField] private Spawner _spawner;
    
        void Start()
        {
            _spawner.Spawn();
        }
    }
}
