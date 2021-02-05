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
  private GameObject target;
  private float timeRemaining;
  private bool hasTarget = false;

  //Runs a timer
  private void Update()
  {
    if (hasTarget)
    {
      timeRemaining -= Time.deltaTime;
      if (timeRemaining < 0)
      {
        //When the timer runs out TargetTracker stops targeting letting ResourceFinder continue working.
        timeRemaining = 0;
        hasTarget = false;
        resourceFinder.SetHasTarget(false);
      }
    }
  }

  //Sets a target to hone in on and start a timer
  public void SetTarget(GameObject t)
  {
    navAgent.SetDestination(t.transform.position);
    target = t;
    timeRemaining = 5;
    hasTarget = true;
  }

  //When a target i set the onTriggerStay will trigger each tick the object is in range and set the navAgent to go to it each tick
  private void OnTriggerStay(Collider other)
  {
    if (!hasTarget)
    {
      return;
    }

    if (other.gameObject.Equals(target))
    {
      navAgent.SetDestination(target.transform.position);
      if (navAgent.remainingDistance < 0.5f)
      {
        resourceFinder.SetHasTarget(false);
        hasTarget = false;
        timeRemaining = 0;
      }
    }
  }
}