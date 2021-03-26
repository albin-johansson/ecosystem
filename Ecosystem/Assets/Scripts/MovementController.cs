using Ecosystem.Genes;
using Ecosystem.Util;
using UnityEngine;
using UnityEngine.AI;

namespace Ecosystem
{
  /// <summary>
  ///  This class contains all functions used for movement and navigation. This is to be used
  ///  with a state machine.
  /// </summary>
  public sealed class MovementController : MonoBehaviour
  {
    [SerializeField] private NavMeshAgent navAgent;
    [SerializeField] private AbstractGenome genome;
    [SerializeField] private SphereCollider sphereCollider;

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
        return sphereCollider.radius < Vector3.Distance(navAgent.transform.position, targetPosition);
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
      var path = new NavMeshPath();
      return NavMesh.CalculatePath(navAgent.transform.position, targetPosition, Terrains.Walkable, path);
    }

    /// <summary>
    ///   Makes the animal run to target if target is valid.
    /// </summary>
    public void RunToTarget(Vector3 targetPosition)
    {
      if (ValidateDestination(targetPosition, out var validPosition))
      {
        SetTarget(validPosition);
      }
    }

    /// <summary>
    ///   Hunting function, sets speed and target.
    ///   Function assumes that position of the victim is valid.
    /// </summary>
    public void StartHunting(Vector3 targetPosition)
    {
      SetNavAgentSpeed();
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
      SetNavAgentSpeed();
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
    public void StartWander() // TODO Should this be ray casted in order to find terrain?
    {
      var agentTransform = navAgent.transform;

      var direction = agentTransform.forward.normalized * sphereCollider.radius;
      direction = RotateDirection(direction, Random.Range(-90f, 90f));

      var position = agentTransform.position;
      var destination = position + direction;

      if (ValidateDestination(destination, out var validPosition))
      {
        SetTarget(validPosition);
      }
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
    private static bool ValidateDestination(Vector3 destination, out Vector3 validPosition)
    {
      if (NavMesh.SamplePosition(destination, out var hit, 1.0f, NavMesh.AllAreas))
      {
        validPosition = hit.position;
        return true;
      }
      else
      {
        validPosition = Vector3.zero;
        return false;
      }
    }

    /// <summary>
    ///   Sets a destination opposite direction of threat at the verge of its vision range
    ///   if that position isn't valid the animal will try a different rotations.
    ///   Returns a valid position or zero vector. 
    /// </summary>
    private Vector3 FindFleeDestination(Vector3 threatPosition)
    {
      var navAgentPosition = navAgent.transform.position;

      var directionFromThreat = navAgentPosition - threatPosition;
      var visionRange = sphereCollider.radius;

      foreach (var angle in _fleeingAngles)
      {
        _fleeDestination = (navAgentPosition + directionFromThreat) * visionRange;

        if (ValidateDestination(_fleeDestination, out var validPosition))
        {
          return validPosition;
        }
        else
        {
          directionFromThreat = RotateDirection(directionFromThreat, angle);
        }
      }

      return Vector3.zero;
    }

    private void SetNavAgentSpeed()
    {
      //TODO: Should use stamina or energy from genome (not implemented in genome yet)
      navAgent.speed = genome.GetSpeedFactor().Value;
    }

    #endregion
  }
}