using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

namespace Colonization
{
    public class ProductsSpawner : MonoBehaviour
    {
        [SerializeField] private Product _productPrefab;
        [SerializeField] private float _repeatRate = 3f;
        [SerializeField] private int _maxCountProducts = 1;
        [SerializeField] private Terrain _ground;

        private ObjectPool<Product> _productsPool;
        private int _countSpawnGoods = 0;

        private void Awake()
        {
            _productsPool = new ObjectPool<Product>(
                createFunc: () => CreatePooledProduct(),
                actionOnGet: (product) => SetParameters(product),
                actionOnRelease: (product) => TurnOffProduct(product),
                actionOnDestroy: (product) => DestroyProduct(product),
                collectionCheck: false);
        }

        private void Start()
        {
            StartCoroutine(Spawn());
        }

        private Product CreatePooledProduct()
        {
            Product product = Instantiate(_productPrefab);
            product.gameObject.SetActive(false);
            product.Collected += ReturnGoodToPool;

            return product;
        }

        private void SetParameters(Product product)
        {
            product.transform.position = SelectSpawnPoint();
            product.gameObject.SetActive(true);
        }

        private void TurnOffProduct(Product product)
        {
            product.gameObject.SetActive(false);
        }

        private void DestroyProduct(Product product)
        {
            product.Collected -= ReturnGoodToPool;
            Destroy(product.gameObject);
        }

        private void GetProduct()
        {
            if (_maxCountProducts > _countSpawnGoods)
            {
                _productsPool.Get();
                _countSpawnGoods++;
            }
        }

        private void ReturnGoodToPool(Product product)
        {
            _countSpawnGoods--;
            _productsPool.Release(product);
        }

        private Vector3 SelectSpawnPoint()
        {
            bool isSelectPoint = true;
            float deadZoneDistance = _productPrefab.transform.localScale.x;
            Vector3 spawnPoint = Vector3.one;

            while (isSelectPoint)
            {
                spawnPoint = new Vector3(
                    Random.Range(0, _ground.terrainData.size.x),
                    spawnPoint.y,
                    Random.Range(0, _ground.terrainData.size.z));

                if (Physics.Raycast(spawnPoint, transform.forward, deadZoneDistance) == false)
                    isSelectPoint = false;
            }

            return spawnPoint;
        }

        private IEnumerator Spawn()
        {
            bool isWork = true;
            WaitForSecondsRealtime waitForSecondsRealtime = new WaitForSecondsRealtime(_repeatRate);

            while (isWork)
            {
                GetProduct();
                yield return waitForSecondsRealtime;
            }
        }
    }
}
