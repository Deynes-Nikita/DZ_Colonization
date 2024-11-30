using UnityEngine;

namespace Colonization
{
    public class TruckSpawner : MonoBehaviour
    {
        [SerializeField] private Truck _truckPrefab;
        [SerializeField] private Transform _spawnPoint;

        public Truck Create()
        {
            return Instantiate(_truckPrefab, _spawnPoint.position, Quaternion.identity);
        }
    }
}