using UnityEngine;

namespace Colonization
{
    public class SuperMarketSpawner : MonoBehaviour
    {
        [SerializeField] private SuperMarket _superMarketPrefab;

        public SuperMarket Create()
        {
            Vector3 spawnPosition = transform.position;
            spawnPosition.y = 0;

            return Instantiate(_superMarketPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
