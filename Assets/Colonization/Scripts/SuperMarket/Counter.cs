using System;
using UnityEngine;

namespace Colonization
{
    public class Counter : MonoBehaviour
    {
        private int _score = 0;
        private int _truckCount = 0;

        public event Action<int> ScoreRecalculated;
        public event Action<int> TruckRecalculated;

        public int Score => _score;

        public void GetReward(int reward)
        {
            if (reward <= 0)
                return;

            _score += reward;
            ScoreRecalculated?.Invoke(_score);
        }

        public bool TryBuy(int price)
        {
            if (price < 0 || _score < price)
                return false;

            DeductScore(price);
            return true;
        }

        public void SetTruckCount(int truckCount)
        {
            if (truckCount <= 0)
                return;

            _truckCount = truckCount;
            TruckRecalculated?.Invoke(_truckCount);
        }

        private void DeductScore(int price)
        {
            _score -= price;
            ScoreRecalculated?.Invoke(_score);
        }
    }
}