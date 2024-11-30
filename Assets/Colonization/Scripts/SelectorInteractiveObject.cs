using System;
using UnityEngine;

namespace Colonization
{
    public class SelectorInteractiveObject : MonoBehaviour
    {
        private float _raycastDistance = 100f;
        private Camera _camera;
        private Interactable _previousInteractable;
        private Interactable _selectInteractable;
        RaycastHit _hit;
        Ray _ray;

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            _ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(_ray, out _hit, _raycastDistance))
            {
                Interactable interactable = _hit.collider.GetComponent<Interactable>();

                if (Input.GetMouseButtonDown(0) && _selectInteractable != null)
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

                    if (Input.GetMouseButtonDown(0))
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
