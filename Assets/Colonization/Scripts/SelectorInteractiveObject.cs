using System;
using UnityEngine;

namespace Colonization
{
    public class SelectorInteractiveObject : MonoBehaviour
    {
        private const int LeftMouseButtonIndex = 0;

        private float _raycastDistance = 100f;
        private Camera _camera;
        private Interactable _previousInteractable;
        private Interactable _selectInteractable;

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            Ray _ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(_ray, out RaycastHit _hit, _raycastDistance))
            {
                Interactable interactable = _hit.collider.GetComponent<Interactable>();

                if (Input.GetMouseButtonDown(LeftMouseButtonIndex) && _selectInteractable != null)
                {
                    _selectInteractable.OnHoverExit();
                    _selectInteractable = null;
                }

                if (interactable != null)
                {
                    if (interactable != _previousInteractable && interactable != _selectInteractable)
                    {
                        interactable.OnHoverEnter();
                        _previousInteractable = interactable;
                    }

                    if (Input.GetMouseButtonDown(LeftMouseButtonIndex))
                    {
                        if (_selectInteractable != null)
                        {
                            _selectInteractable.OnHoverExit();
                        }

                        interactable.OnSelect();
                        _selectInteractable = interactable;
                    }
                }
                else
                {
                    if (_previousInteractable != null && _previousInteractable != _selectInteractable)
                    {
                        _previousInteractable.OnHoverExit();
                        _previousInteractable = null;
                    }
                }

            }
        }
    }
}
