using System;
using UnityEngine;

namespace Colonization
{
    [RequireComponent(typeof(Outline))]
    public class Interactable : MonoBehaviour
    {
        [SerializeField] private Color _colorWhenHover = Color.yellow;
        [SerializeField] private Color _colorWhenSelect = Color.green;

        private Outline _outline;

        public event Action Selected;

        private void Awake()
        {
            _outline = GetComponent<Outline>();
            _outline.enabled = false;
        }

        public void OnHoverEnter()
        {
            _outline.OutlineColor = _colorWhenHover;
            _outline.enabled = true;
        }

        public void OnHoverExit()
        {
            _outline.enabled = false;
        }

        public void OnSelect()
        {
            _outline.OutlineColor = _colorWhenSelect;
            _outline.enabled = true;

            Selected?.Invoke();
        }
    }
}
