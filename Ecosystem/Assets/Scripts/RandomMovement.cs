using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Ecosystem
{
  public sealed class RandomMovement : MonoBehaviour
  {
    [SerializeField] private NavMeshAgent navAgent;
    [SerializeField] private EcoAnimationController ecoAnimationController;

    [SerializeField, Tooltip("What is considered to be the ground")]
    private LayerMask groundMask;

    private Transform _transform;
    private float _timer;

    private void Start()
    {
      _transform = navAgent.transform;
    }

    //The TargetRandomDestination function is called if the agent has no path or the timer has run out
    private void Update()
    {
      if (!navAgent.hasPath || _timer < 0)
      {
        TargetRandomDestination();
        _timer = 10;
      }
      else
      {
        _timer -= Time.deltaTime;
      }
    }

    private void TargetRandomDestination()
    {
      var randomX = Random.Range(-8f, 8f);
      var randomZ = Random.Range(-8f, 8f);

      var position = _transform.position;
      var destination = new Vector3(position.x + randomX, position.y, position.z + randomZ);

      if (Physics.Raycast(destination, -_transform.up, 2f, groundMask))
      {
        navAgent.SetDestination(destination);
        ecoAnimationController.MoveAnimation();
      }
    }
  }
}