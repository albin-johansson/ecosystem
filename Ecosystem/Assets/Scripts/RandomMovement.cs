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
    private Vector3 _lastPos;
    private float _timer;

    private void Start()
    {
      _transform = navAgent.transform;
      _lastPos = _transform.position;
    }

    //The TargetRandomDestination function is called if the agent has no path or the timer has run out
    private void Update()
    {
      if (!navAgent.hasPath || _timer < 0)
      {
        _timer = 10;
        TargetRandomDestination();
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

      if (!IsNewPosGood(position, destination))
      {
        return;
      }

      if (Physics.Raycast(destination, -_transform.up, 2f, groundMask))
      {
        _lastPos = position;
        navAgent.SetDestination(destination);
        ecoAnimationController.MoveAnimation();
      }
    }

    /// <summary>
    ///Compares the new suggested destination with the position it had before it arrived at this one.
    ///If the last is closer to the destination than how close destination is to its current position it will choose another one.
    /// This is too avoid having the animal wander back and forth.
    /// </summary>
    public bool IsNewPosGood(Vector3 current, Vector3 newPos)
    {
      var temp1 = new Vector3(current.x - newPos.x, 0, current.z - newPos.z);
      var temp2 = new Vector3(_lastPos.x - newPos.x, 0, _lastPos.z - newPos.z);
      return Vector3.SqrMagnitude(temp1) <= Vector3.SqrMagnitude(temp2);
    }
  }
}