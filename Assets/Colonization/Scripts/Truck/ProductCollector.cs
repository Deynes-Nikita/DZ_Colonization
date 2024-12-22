using UnityEngine;

namespace Colonization
{
    public class ProductCollector : MonoBehaviour
    {
        [SerializeField] private BootPoint bootPoint;
        
        private Vector3 _bootPoint;
        private Product _products;

        public bool TryPickup(Product product)
        {
            if (product == null || product.transform.parent == true)
                return false;

            _products = product;

            _products.transform.SetParent(transform);
            _products.transform.localPosition = _bootPoint;
            _products.transform.localRotation = Quaternion.identity;

            return true;
        }

        public void Drop(SuperMarket targetSuperMarket)
        {
            if (_products == null)
                return;

            _products.transform.parent = null;
            targetSuperMarket.AcceptProduct(_products);
        }
    }
}