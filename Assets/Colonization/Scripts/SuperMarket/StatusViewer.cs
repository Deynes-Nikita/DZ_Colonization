using TMPro;
using UnityEngine;

namespace BotsPickers
{
    public class StatusViewer : MonoBehaviour
    {
        [SerializeField] private Counter _counter;
        [SerializeField] private TMP_Text _showProductsCount;
        [SerializeField] private TMP_Text _showTruckCount;

        private void OnEnable()
        {
            _counter.ScoreRecalculated += ShowProductsCount;
            _counter.TruckRecalculated += ShowTruckCount;
        }

        private void OnDisable()
        {
            _counter.ScoreRecalculated -= ShowProductsCount;
            _counter.TruckRecalculated -= ShowTruckCount;
        }

        private void ShowTruckCount(int truckCount)
        {
            _showTruckCount.text = truckCount.ToString();
        }

        private void ShowProductsCount(int productsCount)
        {
            _showProductsCount.text = productsCount.ToString();
        }
    }
}