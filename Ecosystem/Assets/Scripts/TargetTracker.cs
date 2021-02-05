using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.AI;

public class TargetTracker : MonoBehaviour
{
  [SerializeField] private NavMeshAgent navAgent;
  [SerializeField] private ResourceFinder resourceFinder;
  private GameObject _target;
  private float _timeRemaining;
  private bool _hasTarget = false;

  private const double StopTrackingThreshold = 0.5f;

  //Runs a timer
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
        resourceFinder.SetHasTarget(false);
      }
    }
  }

  //Sets a target to hone in on and start a timer
  public void SetTarget(GameObject target)
  {
    navAgent.SetDestination(target.transform.position);
    _target = target;
    _timeRemaining = 5;
    _hasTarget = true;
  }

  //When a target i set the onTriggerStay will trigger each tick the object is in range and set the navAgent to go to it each tick
  private void OnTriggerStay(Collider other)
  {
    if (!_hasTarget)
    {
      return;
    }

    if (other.gameObject.Equals(_target))
    {
      navAgent.SetDestination(_target.transform.position);
      if (navAgent.remainingDistance < StopTrackingThreshold)
      {
        resourceFinder.SetHasTarget(false);
        _hasTarget = false;
        _timeRemaining = 0;
      }
    }
  }
}