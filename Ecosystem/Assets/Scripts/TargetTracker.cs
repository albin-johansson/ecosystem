using System;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public sealed class TargetTracker : MonoBehaviour
{
  [SerializeField] private NavMeshAgent navAgent;

  private GameObject _target;
  private float _timeRemaining;
  private bool _hasTarget = false;

  private const double StopTrackingThreshold = 0.1f;

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

  public bool HasTarget => _hasTarget;

  //When a target is acquired the onTriggerStay will trigger each tick the object is in range and set the navAgent to go to it each tick
  private void OnTriggerStay(Collider other)
  {
    int i = 0;
    /*
    if (!_hasTarget)
    {
      return;
    }


    if (other.gameObject.Equals(_target))
    {
      navAgent.SetDestination(_target.transform.position);
      if (navAgent.remainingDistance < StopTrackingThreshold)
      {
        _hasTarget = false;
        _timeRemaining = 0;
      }
    }
    */
  }
}