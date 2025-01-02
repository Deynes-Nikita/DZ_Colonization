using UnityEngine;

namespace Colonization
{
    public class SelectorInteractiveObject : MonoBehaviour
    {
        private const int MouseButtonIndexForSelect = 0;

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
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, _raycastDistance))
            {
                TrySelectObject(hit.collider.GetComponent<Interactable>());
            }
        }

        private bool TrySelectObject(Interactable interactable)
        {
            if (Input.GetMouseButtonDown(MouseButtonIndexForSelect) && _selectInteractable != null)
            {
                DeselectObject(_selectInteractable);
                _selectInteractable = null;
            }

            if (interactable != null)
            {
                if (interactable != _previousInteractable && interactable != _selectInteractable)
                {
                    HighlightObject(interactable);
                }

                if (Input.GetMouseButtonDown(MouseButtonIndexForSelect))
                {
                    if (_selectInteractable != null)
                    {
                        DeselectObject(_selectInteractable);
                    }

                    SelectObject(interactable);
                }

                return true;
            }
            else
            {
                if (_previousInteractable != null && _previousInteractable != _selectInteractable)
                {
                    DeselectObject(_previousInteractable);
                    _previousInteractable = null;
                }

                return false;
            }
        }

        private void HighlightObject(Interactable interactable)
        {
            interactable.HoverEnter();
            _previousInteractable = interactable;
        }

        private void SelectObject(Interactable interactable)
        {
            interactable.Select();
            _selectInteractable = interactable;
        }

        private void DeselectObject(Interactable interactable)
        {
            interactable.HoverExit();
        }
    }
}
