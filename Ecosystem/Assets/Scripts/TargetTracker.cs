using UnityEngine;
using UnityEngine.AI;

namespace Ecosystem
{
  public sealed class TargetTracker : MonoBehaviour
  {
    [SerializeField] private NavMeshAgent navAgent;
    [SerializeField] private AnimationStatesController animationStatesController;
    private GameObject _target;
    private bool _targetInSight = false;
    private float _timeRemaining;
    private bool _hasTarget = false;
    private Desire _targetType;
    private bool _chased = false;
    private Vector3 _fleeDirection;


    int water = 4;
    int food = 6;
    int rabbit = 8;
    //int wolf = 7;

    private const double StopTrackingThreshold = 0.1f;

    public bool HasTarget => _hasTarget;

    //Runs a timer for when to stop looking for the target
    private void Update()
    {
      if (_hasTarget)
      {
        _timeRemaining -= Time.deltaTime;
        if (_timeRemaining < 0)
        {
          //When the timer runs out TargetTracker stops targeting letting ResourceFinder continue working.
          _timeRemaining = 0;
          _hasTarget = false;
          _targetInSight = false;
          _chased = false;
        }
      }

      //When chased the velocity is constantly set to the fleeing direction while the timer for being chased is still running. 
      if (_chased)
      {
        navAgent.velocity = _fleeDirection;
      }
    }

    //Sets a target to hone in on and start a timer
    public void SetTarget(Vector3 target, Desire desire)
    {
      navAgent.SetDestination(target);
      _targetType = desire;
      _timeRemaining = 10;
      _hasTarget = true;
    }

    //Called from ResourceFinder when encountering a predator
    public void FleeFromPredator(GameObject predator)
    {
      SetFleeDirection(predator.transform.position);
      _target = predator;
      _hasTarget = true;
      _targetInSight = true;
      _chased = true;
      _timeRemaining = 5;
    }

    //Sets the _fleeDirection to be away from the predators position. The _fleeDirection works as a velocity and therefore we apply the navAgents speed to the vector. 
    private void SetFleeDirection(Vector3 predatorPosition)
    {
      var velocityDirection = (navAgent.transform.position - predatorPosition).normalized;
      _fleeDirection = velocityDirection * navAgent.speed;
    }

    //When a target is acquired the onTriggerStay will trigger each tick the object is in range and set the navAgent to go to it each tick
    private void OnTriggerStay(Collider other)
    {
      if (!_hasTarget)
      {
        return;
      }

      if (_targetInSight && other.gameObject.Equals(_target))
      {
        //if predator is still in range set the fleeing direction to match the predator. 
        if (_chased)
        {
          SetFleeDirection(other.transform.position);
          _timeRemaining = 5;
          return;
        }

        navAgent.SetDestination(other.transform.position);
        if (navAgent.remainingDistance < StopTrackingThreshold)
        {
          _hasTarget = false;
          _targetInSight = false;
          _timeRemaining = 0;
        }
      }

      if (_targetInSight)
      {
        return;
      }

      switch (_targetType)
      {
        case Desire.Food:
          if (other.gameObject.layer.Equals(food))
          {
            _target = other.gameObject;
            _targetInSight = true;
          }

          break;
        case Desire.Prey:
          if (other.gameObject.layer.Equals(rabbit))
          {
            _target = other.gameObject;
            _targetInSight = true;
          }

          break;
        case Desire.Water:
          if (other.gameObject.layer.Equals(water))
          {
            _target = other.gameObject;
            _targetInSight = true;
          }

          break;
        default: break;
      }
    }
  }
}