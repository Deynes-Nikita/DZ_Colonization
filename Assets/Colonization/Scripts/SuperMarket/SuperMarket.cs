using System.Collections.Generic;
using UnityEngine;

namespace Colonization
{
    [RequireComponent(typeof(Scanner))]
    [RequireComponent(typeof(Counter))]
    [RequireComponent(typeof(TruckSpawner))]
    [RequireComponent(typeof(Interactable))]
    [RequireComponent(typeof(FlagInstaller))]
    public class SuperMarket : MonoBehaviour, ITargeted
    {
        [SerializeField] private int _truckPrice = 3;
        [SerializeField] private int _superMarketPrice = 5;
        [SerializeField] private int _minLimitTruck = 1;
        [SerializeField] private List<Truck> _trucks = new List<Truck>();
        [SerializeField] private Transform _receivingPoint;

        private Scanner _scanner;
        private Counter _counter;
        private TruckSpawner _truckSpawner;
        private FlagInstaller _flagInstaller;
        private Interactable _interactable;
        private List<Product> _currentProducts = new List<Product>();
        private Truck _buildingTruck = null;
        private bool _isflagInstalled = false;
        private int _score = 0;

        public Vector3 ReceivingPoint => _receivingPoint.position;

        private void Awake()
        {
            _scanner = GetComponent<Scanner>();
            _counter = GetComponent<Counter>();
            _truckSpawner = GetComponent<TruckSpawner>();
            _interactable = GetComponent<Interactable>();
            _flagInstaller = GetComponent<FlagInstaller>();
        }

        private void OnEnable()
        {
            _counter.ScoreRecalculated += OnGetScore;
            _interactable.Selected += OnSelectPointForNewSupermarket;
            _flagInstaller.Installed += OnGetReadyToBuild;

            foreach (Truck truck in _trucks)
            {
                truck.TargetMissed += OnRestartTartgetProduct;
            }
        }

        private void OnDisable()
        {
            _counter.ScoreRecalculated -= OnGetScore;
            _interactable.Selected -= OnSelectPointForNewSupermarket;
            _flagInstaller.Installed -= OnGetReadyToBuild;
        }

        private void Start()
        {
            _counter.GetTruckCount(_trucks.Count);
        }

        private void Update()
        {
            OnCreate();
            SendForProducts();
        }

        public void AcceptProduct(Product product)
        {
            _counter.GetReward(product.GiveReward());
            _currentProducts.Remove(product);
        }

        public void AddTruck(Truck truck)
        {
            _trucks.Add(truck);

            truck.TargetMissed += OnRestartTartgetProduct;
            _counter.GetTruckCount(_trucks.Count);
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
            {
                return;
            }

            truck.GetTask(product);
        }

        private void OnGetScore(int score)
        {
            _score = score;
        }

        private void OnCreate()
        {
            if (_isflagInstalled == true)
            {
                if (_score >= _superMarketPrice && TrySelectTruck (out Truck truck))
                {
                    SendToBuildSuperMarket(_flagInstaller.GetFlag(), truck);
                }
            }
            else if (_score >= _truckPrice)
            {
                CreateTruck();
            }
        }

        private void CreateTruck()
        {
            if (_counter.TryBuy(_truckPrice))
            {
                Truck truck = _truckSpawner.Create();
                truck.SetTargetSuperMarket(this);
                AddTruck(truck);
            }
        }

        private void RemoveTruck(Truck truck)
        {
            _trucks.Remove(truck);
            
            truck.TargetMissed -= OnRestartTartgetProduct;

            _counter.GetTruckCount(_trucks.Count);
        }

        private void OnSelectPointForNewSupermarket()
        {
            if (_trucks.Count > _minLimitTruck)
            {
                _flagInstaller.OnSelectPointForBuilding();
            }
        }

        private void OnGetReadyToBuild()
        {
            _isflagInstalled = true;
        }

        private void SendToBuildSuperMarket(Flag flag, Truck truck)
        {
            if (_buildingTruck == null)
            {
                if (_counter.TryBuy(_superMarketPrice))
                {
                    _buildingTruck = truck;

                    RemoveTruck(_buildingTruck);

                    _buildingTruck.ArrivedToBuilding += OnArrivedToBuilding;
                }
            }

            _buildingTruck.GetTaskOfBuildingSuperMarket(flag);
        }

        private void OnArrivedToBuilding()
        {
            ForgetBuildingTruck();
            _flagInstaller.RemoveFlag();
            _isflagInstalled = false;
        }

        private void ForgetBuildingTruck()
        {
            _buildingTruck.ArrivedToBuilding -= OnArrivedToBuilding;

            _buildingTruck = null;
        }

        private void OnRestartTartgetProduct(Product product)
        {
            if (_currentProducts.Contains(product))
            _currentProducts.Remove(product);
        }
    }
}
