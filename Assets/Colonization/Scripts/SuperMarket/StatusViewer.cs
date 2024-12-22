using TMPro;
using UnityEngine;

namespace Colonization
{
    public class StatusViewer : MonoBehaviour
    {
        [SerializeField] private Counter _counter;
        [SerializeField] private TMP_Text _showProductsCount;
        [SerializeField] private TMP_Text _showTruckCount;

        private void OnEnable()
        {
            _counter.ScoreRecalculated += OnShowProductsCount;
            _counter.TruckRecalculated += OnShowTruckCount;
        }

        private void OnDisable()
        {
            _counter.ScoreRecalculated -= OnShowProductsCount;
            _counter.TruckRecalculated -= OnShowTruckCount;
        }

        private void OnShowTruckCount(int truckCount)
        {
            _showTruckCount.text = truckCount.ToString();
        }

        private void OnShowProductsCount(int productsCount)
        {
            _showProductsCount.text = productsCount.ToString();
        }
    }
}