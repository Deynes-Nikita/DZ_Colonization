using System;
using UnityEngine;

namespace Colonization
{
    [RequireComponent(typeof(TruckMovement))]
    [RequireComponent(typeof(ProductCollector))]
    [RequireComponent(typeof(SuperMarketSpawner))]
    public class Truck : MonoBehaviour
    {
        [SerializeField] private float _interactionDistance = 3.5f;
        [SerializeField] private SuperMarket _superMarket;

        private TruckMovement _movement;
        private ProductCollector _handler;
        private SuperMarketSpawner _superMarketSpawner;

        private Product _product;

        private bool _isBusy = false;
        private bool _isBusyforBuilding = false;
        private bool _isProductAvailable = false;

        public bool IsBusy => _isBusy;

        public event Action ArrivedToBuild;
        public event Action<Product> TargetMissed;

        private void Awake()
        {
            _movement = GetComponent<TruckMovement>();
            _handler = GetComponent<ProductCollector>();
            _superMarketSpawner = GetComponent<SuperMarketSpawner>();
        }

        private void OnEnable()
        {
            _movement.Arrived += OnArrivedToPoint;
        }

        private void OnDisable()
        {
            _movement.Arrived += OnArrivedToPoint;
        }

        public void SetTargetSuperMarket(SuperMarket targetSuperMarket)
        {
            _superMarket = targetSuperMarket;
        }

        public void GetTask(Product product)
        {
            if (product == null)
                return;

            _product = product;

            _isBusy = true;

            StartMove(_product.transform.position, _product);
        }

        public void GetTaskOfBuildSuperMarket(Flag flag)
        {
            StopMove();
            _isBusyforBuilding = true;
            StartMove(flag.transform.position, flag);
        }

        private void StartMove(Vector3 targetPosition, ITargeted targeted)
        {
            _movement.StartMove(targetPosition, _interactionDistance, targeted);
        }

        private void StopMove()
        {
            _movement.StopMove();
        }

        private void ReturnToSuperMarket()
        {
            StartMove(_superMarket.ReceivingPoint, _superMarket);
        }

        private void ResetTask()
        {
            StopMove();

            _isBusy = false;

            _isBusyforBuilding = false;
        }

        private void OnArrivedToPoint()
        {
            if (_isBusyforBuilding == true)
            {
                BuildSuperMarket();
                return;
            }

            if (_isProductAvailable)
            {
                _handler.Drop(_superMarket);
                _isProductAvailable = false;
                _isBusy = false;
            }
            else
            {
                if (_handler.TryPickup(_product))
                {
                    ReturnToSuperMarket();
                    _isProductAvailable = true;
                }
                else
                {
                    TargetMissed?.Invoke(_product);
                    ResetTask();
                }
            }
        }

        private void BuildSuperMarket()
        {
            ArrivedToBuild?.Invoke();

            ResetTask();

            SetTargetSuperMarket(_superMarketSpawner.Create());
            _superMarket.AddTruck(this);
        }
    }
}