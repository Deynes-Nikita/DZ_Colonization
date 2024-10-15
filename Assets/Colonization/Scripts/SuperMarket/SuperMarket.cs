using System.Collections.Generic;
using UnityEngine;

namespace BotsPickers
{
    [RequireComponent(typeof(Scanner))]
    [RequireComponent(typeof(Counter))]
    public class SuperMarket : MonoBehaviour, ITargeted
    {
        [SerializeField] private List<Truck> _trucks = new List<Truck>();
        [SerializeField] private Transform _receivingPoint;

        private Scanner _scanner;
        private Counter _counter;
        private List<Product> _currentProducts = new List<Product>();

        public Vector3 ReceivingPoint => _receivingPoint.position;

        private void Awake()
        {
            _scanner = GetComponent<Scanner>();
            _counter = GetComponent<Counter>();
        }

        private void Start()
        {
            _counter.GetTruckCount(_trucks.Count);
        }

        private void Update()
        {
            SendForProducts();
        }

        public void AcceptProduct(Product product)
        {
            _counter.GetReward(product.GiveReward());
            _currentProducts.Remove(product);
        }

        private bool TrySelectProduct(out Product product)
        {
            List<Product> products = new List<Product>();

            products = _scanner.Scan();

            if (products.Count == 0)
            {
                product = null;
                return false;
            }

            foreach (Product item in products)
            {
                if (item == null || _currentProducts.Contains(item))
                    continue;

                product = item;
                _currentProducts.Add(product);
                return true;
            }

            product = null;
            return false;
        }

        private bool TrySelectTruck(out Truck truck)
        {
            if (_trucks.Count == 0)
            {
                truck = null;
                return false;
            }

            foreach (Truck unit in _trucks)
            {
                if (unit == null)
                    continue;

                if (unit.IsBusy == false)
                {
                    truck = unit;
                    return true;
                }
            }

            truck = null;
            return false;
        }

        private void SendForProducts()
        {
            if (TrySelectTruck(out Truck truck) == false || TrySelectProduct(out Product product) == false)
                return;

            truck.GetTask(product);
        }
    }
}
