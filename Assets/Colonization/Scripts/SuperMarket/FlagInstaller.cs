using System;
using System.Collections;
using UnityEngine;

namespace Colonization
{
    public class FlagInstaller : MonoBehaviour
    {
        private const int MouseButtonIndexForInstall = 0;

        [SerializeField] private Flag _flagPrefab;

        private Flag _flag;

        public event Action Installed;

        public void SelectPointForBuild()
        {
            StartCoroutine(SelectPointForNewSupermarket());
        }

        public Flag GetFlag()
        {
            return _flag;
        }

        public void RemoveFlag()
        {
            Destroy(_flag.gameObject);
            _flag = null;
        }

        private IEnumerator SelectPointForNewSupermarket()
        {
            WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
            Camera camera = Camera.main;
            bool isSelectPiont = false;
            float raycastDistance = 100f;
            Ray ray;
            RaycastHit hit;

            while (isSelectPiont == false)
            {
                if (Input.GetMouseButton(MouseButtonIndexForInstall))
                {
                    ray = camera.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit, raycastDistance))
                    {
                        if (hit.collider.GetComponent<Ground>())
                        {
                            Vector3 flagPosition = hit.point;
                            InstallFlag(flagPosition);
                            isSelectPiont = true;
                        }
                    }
                }

                yield return waitForEndOfFrame;
            }

            yield return null;
        }

        private void InstallFlag(Vector3 flagPosition)
        {
            if (_flag == null)
            {
                _flag = Instantiate(_flagPrefab);
            }

            _flag.transform.position = flagPosition;

            Installed?.Invoke();
        }
    }
}
