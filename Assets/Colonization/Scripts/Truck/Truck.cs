using UnityEngine;

namespace BotsPickers
{
    [RequireComponent(typeof(TruckMovement))]
    [RequireComponent(typeof(ProductsHandler))]
    public class Truck : MonoBehaviour
    {
        [SerializeField] private float _interactionDistance = 3.5f;
        [SerializeField] private SuperMarket _superMarket;

        private TruckMovement _movement;
        private ProductsHandler _handler;

        private Product _product;

        private bool _isBusy = false;
        private bool _isProductAvailable = false;

        public bool IsBusy => _isBusy;

        private void Awake()
        {
            _movement = GetComponent<TruckMovement>();
            _handler = GetComponent<ProductsHandler>();
        }

        private void OnEnable()
        {
            _movement.Arrived += OnHandlingProduct;
        }

        private void OnDisable()
        {
            _movement.Arrived += OnHandlingProduct;
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

        private void StartMove(Vector3 targetPosition, ITargeted targeted)
        {
            _movement.OnMove(targetPosition, _interactionDistance, targeted);
        }

        private void StopMove()
        {
            _movement.StopMoving();
        }

        private void ReturnToSuperMarket()
        {
            StartMove(_superMarket.ReceivingPoint, _superMarket);
        }

        private void ResetTask()
        {
            StopMove();
            _isBusy = false;
        }

        private void OnHandlingProduct()
        {
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
                    ResetTask();
                }
            }
        }
    }
}