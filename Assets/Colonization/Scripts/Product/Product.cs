using System;
using UnityEngine;

namespace BotsPickers
{
    public class Product : MonoBehaviour, ITargeted
    {
        [SerializeField] private int _reward = 1;

        public event Action<Product> Collected;

        private void OnEnable()
        {
            ResetParameters();
        }

        public int GiveReward()
        {
            Collected?.Invoke(this);
            return _reward;
        }

        private void ResetParameters()
        {
            transform.rotation = Quaternion.identity;
        }
    }
}
