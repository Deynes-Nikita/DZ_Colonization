using System.Collections.Generic;
using UnityEngine;

namespace BotsPickers
{
    public class Scanner : MonoBehaviour
    {
        [SerializeField] private float _radius = 1f;

        public List<Product> Scan()
        {
            List<Product> products = new List<Product>();

            foreach (Product product in GetGoods())
            {
                products.Add(product);
            }

            return products;
        }

        private List<Product> GetGoods()
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
