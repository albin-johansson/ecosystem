using System;
using System.Numerics;
using Ecosystem.Genes;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace Ecosystem.AnimalBehaviour
{
  /// <summary>
  ///  This class contains all functions used for movement and navigation. This is to be used
  /// with a state machine.
  /// </summary>
  public sealed class MovementController : MonoBehaviour
  {
    [SerializeField] private NavMeshAgent navAgent;
    [SerializeField] private AbstractGenome genome;

    private Vector3 _fleeDestination;
    private float _previousSpeed;
    private readonly float[] _fleeingAngles = {-90, 90, 180};


    #region PublicFunctions

    /// <summary>
    ///   Returns distance to target, zero if target is not reachable.
    /// </summary>
    public bool IsTargetInRange(Vector3 targetPosition)
    {
      if (IsReachable(targetPosition))
      {
        return (genome.GetVision().Value < Vector3.Distance(navAgent.transform.position, targetPosition));
      }
      else
      {
        return false;
      }
    }

    /// <summary>
    ///   Checks whether a position is reachable through the NavMesh
    /// </summary>
    public bool IsReachable(Vector3 targetPosition)
    {
      return true;
      //TODO: Fix this function
      //var potentialPath = new NavMeshPath();
      //return (navAgent.CalculatePath(targetPosition, potentialPath));
    }

    /// <summary>
    ///   Makes the animal run to target if target is valid.
    /// </summary>
    public bool RunToTarget(Vector3 targetPosition)
    {
      var validatedPosition = ValidateDestination(targetPosition);
      if (validatedPosition != Vector3.zero)
      {
        SetTarget(validatedPosition);
        return true;
      }

      return false;
    }

    /// <summary>
    ///   Hunting function, sets speed and target.
    ///   Function assumes that position of the victim is valid.
    /// </summary>
    public void StartHunting(Vector3 targetPosition)
    {
      SetNavAgentSpeed(true);
      SetTarget(targetPosition);
    }

    /// <summary>
    ///   Updates hunting navigation uses StartHunting as of now.
    /// </summary>
    public void UpdateHunting(Vector3 targetPosition)
    {
      StartHunting(targetPosition);
    }

    /// <summary>
    ///   Finds a destination to flee to opposite of given threat position and updates speed.
    /// </summary>
    public void StartFleeing(Vector3 threatPosition)
    {
      _fleeDestination = FindFleeDestination(threatPosition);
      SetNavAgentSpeed(true);
      SetTarget(_fleeDestination);
    }


    /// <summary>
    ///   Updates animals navigation when fleeing if it doesn't have a target destination,
    ///   or it is getting close to it.
    /// </summary>
    public void UpdateFleeing(Vector3 threatPosition)
    {
      if (!navAgent.hasPath || navAgent.remainingDistance < 1.0f)
      {
        StartFleeing(threatPosition);
      }
    }


    /// <summary>
    ///   Pauses or resumes the navAgents movement, harmless to call even if the
    ///   desired action already has been taken.
    /// </summary>
    public void StandStill(bool shouldPause)
    {
      if (shouldPause)
      {
        if (_previousSpeed > 0)
        {
          _previousSpeed = navAgent.speed;
        }

        navAgent.speed = 0;
      }
      else
      {
        navAgent.speed = _previousSpeed;
      }
    }


    /// <summary>
    ///   Sets a target at the edge of the vision range in an angle within a half circle
    ///   of the direction the animal look.
    /// </summary>
    public bool StartWander()  //borde raycastas så den hittar terräng?
    {
      var direction = navAgent.transform.forward.normalized * (float) genome.GetVision().Value;
      var randomAngle = UnityEngine.Random.Range(-90f, 90f);
      direction = RotateDirection(direction, randomAngle);
      var position = navAgent.transform.position;
      var destination = position + direction;

      var validatedDestination = ValidateDestination(destination);
      if (!(validatedDestination == Vector3.zero))
      {
        SetTarget(validatedDestination);
        return true;
      }

      return false;
    }

    /// <summary>
    ///  Gets called from Tick in the wander state, updates a random position if it is sufficiently
    ///  close to its last destination
    /// </summary>
    public void UpdateWander()
    {
      if (!navAgent.hasPath || navAgent.remainingDistance < 1.0f)
      {
        StartWander();
      }
    }

    #endregion

    #region PrivateHelperFunctions

    /// <summary>
    ///   Sets the destination for the navMeshAgent
    /// </summary>
    private void SetTarget(Vector3 destination)
    {
      //TODO: rotate before moving
      navAgent.SetDestination(destination);
    }

    //
    /// <summary>
    ///   Rotates a vector the specified amount in the Y-plane
    /// </summary>
    private static Vector3 RotateDirection(Vector3 direction, float amount)
    {
      return Quaternion.Euler(0, amount, 0) * direction;
    }

    /// <summary>
    ///   Checks if a position is valid by checking if there is a valid position within 1.0f radius,
    ///   returns that valid position. If not valid returns zero vector.
    /// </summary>
    private static Vector3 ValidateDestination(Vector3 destination)
    {
      if (NavMesh.SamplePosition(destination, out var hit, 1.0f, NavMesh.AllAreas))
      {
        destination = hit.position;
      }
      else
      {
        destination = Vector3.zero;
      }

      return destination;
    }

    /// <summary>
    ///   Sets a destination opposite direction of threat at the verge of its vision range
    ///   if that position isn't valid the animal will try a different rotations.
    ///   Returns a valid position or zero vector. 
    /// </summary>
    private Vector3 FindFleeDestination(Vector3 threatPosition)
    {
      var navAgentPosition = navAgent.transform.position;
      var visionRange = (float) genome.GetVision().Value;
      var directionFromThreat = navAgentPosition - threatPosition;
      for (var i = 0; i < _fleeingAngles.Length; i++)
      {
        _fleeDestination = (navAgentPosition + directionFromThreat) * visionRange;
        var validatedFleeDestination = ValidateDestination(_fleeDestination);
        if (validatedFleeDestination == Vector3.zero)
        {
          directionFromThreat = RotateDirection(directionFromThreat, _fleeingAngles[i]);
        }
        else
        {
          return _fleeDestination;
        }
      }

      return Vector3.zero;
    }

    private void SetNavAgentSpeed(bool run)
    {
      //TODO: Should use stamina or energy from genome (not implemented in genome yet)
      navAgent.speed = (float) genome.GetSpeedFactor().Value;
    }

    #endregion
  }
}