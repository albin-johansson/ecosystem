using UnityEngine;
using UnityEngine.AI;

namespace Ecosystem
{
  public sealed class TargetTracker : MonoBehaviour
  {
    private const double StopTrackingThreshold = 0.1f;

    [SerializeField] private NavMeshAgent navAgent;
    [SerializeField] private AnimationStatesController animationStatesController;
    private GameObject _target;
    private Vector3 _fleeDirection;
    private float _timeRemaining;
    private bool _hasTarget;
    private bool _chased;

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
    public void SetTarget(GameObject target)
    {
      animationStatesController.AnimAnimationState = AnimationState.Walking;
      navAgent.SetDestination(target.transform.position);
      _target = target;
      _timeRemaining = 5;
      _hasTarget = true;
    }

    //Called from ResourceFinder when encountering a predator
    public void FleeFromPredator(GameObject predator)
    {
      SetFleeDirection(predator.transform.position);
      _target = predator;
      _hasTarget = true;
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

      if (other.gameObject.Equals(_target))
      {
        //if predator is still in range set the fleeing direction to match the predator. 
        if (_chased)
        {
          SetFleeDirection(other.transform.position);
          _timeRemaining = 5;
          return;
        }

        navAgent.SetDestination(_target.transform.position);
        if (navAgent.remainingDistance < StopTrackingThreshold)
        {
          _hasTarget = false;
          _timeRemaining = 0;
        }
      }
    }
  }
}