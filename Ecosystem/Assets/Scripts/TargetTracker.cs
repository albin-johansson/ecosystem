using System;
using Ecosystem.Util;
using UnityEngine;
using UnityEngine.AI;

namespace Ecosystem
{
  public sealed class TargetTracker : MonoBehaviour
  {
    [SerializeField] private NavMeshAgent navAgent;
    [SerializeField] private EcoAnimationController animationController;
    private GameObject _target;
    private bool _targetInSight;
    private float _timeRemaining;
    private AnimalState _targetType;
  
    private Vector3 _fleeDirection;

    private const double StopTrackingThreshold = 0.1f;

    public bool HasTarget { get; private set; }
    public bool IsChased { get; private set; }

    //Runs a timer for when to stop looking for the target
    private void Update()
    {
      if (HasTarget)
      {
        _timeRemaining -= Time.deltaTime;
        if (_timeRemaining < 0)
        {
          //When the timer runs out TargetTracker stops targeting letting ResourceFinder continue working.
          _timeRemaining = 0;
          HasTarget = false;
          _targetInSight = false;
          IsChased = false;
        }
      }

      //When chased the velocity is constantly set to the fleeing direction while the timer for being chased is still running.
      if (IsChased)
      {
        navAgent.velocity = _fleeDirection;
      }
    }

    //Sets a target to hone in on and start a timer
    public void SetTarget(Vector3 target, AnimalState state)
    {
      animationController.MoveAnimation();
      navAgent.SetDestination(target);
      _targetType = state;
      _timeRemaining = 10;
      HasTarget = true;
    }

    //Called from ResourceFinder when encountering a predator
    public void FleeFromPredator(GameObject predator)
    {
      SetFleeDirection(predator.transform.position);
      _target = predator;
      HasTarget = true;
      _targetInSight = true;
      IsChased = true;
      _timeRemaining = 5;
    }

    public void StopTracking()
    {
      navAgent.isStopped = true;
      _target = null;
    }

    public void ResumeTracking()
    {
      navAgent.isStopped = false;
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
      if (!HasTarget)
      {
        return;
      }

      if (_targetInSight && other.gameObject.Equals(_target))
      {
        //if predator is still in range set the fleeing direction to match the predator.
        if (IsChased)
        {
          SetFleeDirection(other.transform.position);
          _timeRemaining = 5;
          return;
        }

        navAgent.SetDestination(other.transform.position);
        if (navAgent.remainingDistance < StopTrackingThreshold)
        {
          HasTarget = false;
          _targetInSight = false;
          _timeRemaining = 0;
        }
      }

      if (!_targetInSight)
      {
        LookForTarget(other.gameObject);
      }
    }

    private void LookForTarget(GameObject other)
    {
      if (_targetType == AnimalState.LookingForFood && other.layer == LayerUtil.FoodLayer ||
          _targetType == AnimalState.LookingForPrey && other.layer == LayerUtil.PreyLayer ||
          _targetType == AnimalState.LookingForWater && other.layer == LayerUtil.WaterLayer)
      {
        _target = other.gameObject;
        _targetInSight = true;
      }
    }
  }
}
