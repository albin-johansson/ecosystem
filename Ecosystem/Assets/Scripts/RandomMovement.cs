using UnityEngine;
using UnityEngine.AI;

public sealed class RandomMovement : MonoBehaviour
{
  [SerializeField] private NavMeshAgent navAgent;

  [SerializeField, Tooltip("What is considered to be the ground")]
  private LayerMask groundMask;

  private Transform _transform;

  private void Start()
  {
    _transform = transform;
  }

  private void Update()
  {
    if (!navAgent.hasPath)
    {
      TargetRandomDestination();
    }
  }

  private void TargetRandomDestination()
  {
    var randomX = Random.Range(-5f, 5f);
    var randomZ = Random.Range(-5f, 5f);

    var position = _transform.position;
    var destination = new Vector3(position.x + randomX, position.y, position.z + randomZ);

    if (Physics.Raycast(destination, -_transform.up, 2f, groundMask))
    {
      navAgent.SetDestination(destination);
    }
  }
}