using System.Collections.Generic;
using UnityEngine;

namespace Colonization
{
    public class Scanner : MonoBehaviour
    {
        [SerializeField] private float _radius = 100f;

        public List<Product> Scan()
        {
            List<Product> products = new List<Product>();

            foreach (Product product in GetProducts())
            {
                products.Add(product);
            }

            return products;
        }

        private List<Product> GetProducts()
        {
            List<Product> products = new List<Product>();

            Collider[] hits = Physics.OverlapSphere(transform.position, _radius);

            foreach (Collider hit in hits)
            {
                if (hit.TryGetComponent<Product>(out Product product) && product.transform.parent == false)
                    products.Add(product);
            }

            return products;
        }
    }
}
