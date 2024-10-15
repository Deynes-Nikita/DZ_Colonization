using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace BotsPickers
{
    public class TruckMovement : MonoBehaviour
    {
        private Coroutine _moveCoroutine = null;
        private NavMeshAgent _agent;

        public event Action Arrived;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
        }

        public void OnMove(Vector3 targetPosition, float interactionDistance, ITargeted target)
        {
            if (_moveCoroutine != null)
                StopMoving();

            _moveCoroutine = StartCoroutine(Move(targetPosition, interactionDistance, target));
        }

        public void StopMoving()
        {
            if (_moveCoroutine != null)
                StopCoroutine(_moveCoroutine);

            _moveCoroutine = null;
        }

        private IEnumerator Move(Vector3 targetPosition, float interactionDistance, ITargeted target)
        {
            bool isMoving = true;
            float distance;
            RaycastHit hit;

            _agent.SetDestination(targetPosition);

            while (isMoving)
            {
                distance = Vector3.Distance(transform.position, targetPosition);

                if (distance < interactionDistance)
                    isMoving = false;

                Physics.Raycast(transform.position, transform.forward, out hit, interactionDistance);

                if (hit.collider != null)
                    if (hit.collider.GetComponent<ITargeted>() == target)
                        isMoving = false;

                yield return null;
            }

            Arrived?.Invoke();

            _moveCoroutine = null;
        }
    }
}
