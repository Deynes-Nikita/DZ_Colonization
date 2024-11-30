using UnityEngine;

namespace Colonization
{
    public class StatusRotator : MonoBehaviour
    {
        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;
            LookAtCamera();
        }

        private void LookAtCamera()
        {
            transform.LookAt(_camera.transform.position);
        }
    }
}
