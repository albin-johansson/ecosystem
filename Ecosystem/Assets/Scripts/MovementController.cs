using System;
using Ecosystem.Genes;
using Ecosystem.Util;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

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
    [SerializeField] private StaminaController staminaController;

    private Vector3 _fleeDestination;
    private float _previousSpeed;
    private readonly float[] _fleeingAngles = {-45, 90, 45, -180, -90};

    #region PublicFunctions

    /// <summary>
    ///   Returns distance to target, zero if target is not reachable.
    /// </summary>
    public bool IsTargetInRange(Vector3 targetPosition)
    {
      if (IsReachable(targetPosition))
      {
        return sphereCollider.radius >= Math.Floor(Vector3.Distance(navAgent.transform.position, targetPosition));
      }
      else
      {
        return false;
      }
    }

    /// <summary>
    ///   Checks whether a point is reachable through the navMesh, will return true if there is a valid point
    ///   within a radius from the point
    /// </summary>
    public bool IsReachable(Vector3 targetPosition)
    {
      return ValidateDestination(targetPosition, out var destination);
    }

    public Vector3 GetPosition()
    {
      return navAgent.transform.position;
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
    public bool StartFleeing(Vector3 threatPosition)
    {
      _fleeDestination = FindFleeDestination(threatPosition);
      if (_fleeDestination != Vector3.zero)
      {
        SetTarget(_fleeDestination);
        return true;
      }

      return false;
    }


    /// <summary>
    ///   Updates animals navigation when fleeing if it doesn't have a target destination,
    ///   or it is getting close to it.
    /// </summary>
    public bool UpdateFleeing(Vector3 threatPosition)
    {
      if (!navAgent.hasPath || navAgent.remainingDistance < 1.0f)
      {
        return StartFleeing(threatPosition);
      }

      return true;
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
    public void StartWander()
    {
      float[] wanderAngles = {0f, -90f, 90f, 180f};
      var agentTransform = navAgent.transform;
      var direction = agentTransform.forward.normalized * sphereCollider.radius;
      foreach (var angle in wanderAngles)
      {
        var rotatedDirection = RotateDirection(direction, Random.Range(angle - 45f, angle + 45f));
        var position = agentTransform.position;
        var destination = position + rotatedDirection;
        if (ValidateDestination(destination, out var validPosition))
        {
          SetTarget(validPosition);
          return;
        }
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
      SetNavAgentSpeed();
      navAgent.SetDestination(destination);
    }

    /// <summary>
    ///   Rotates a vector the specified amount in the Y-plane
    /// </summary>
    private static Vector3 RotateDirection(Vector3 direction, float amount)
    {
      return Quaternion.Euler(0, amount, 0) * direction;
    }

    /// <summary>
    ///   Validates if there is a path to a given point
    /// </summary>
    private bool ValidatePath(Vector3 destination)
    {
      var navMeshPath = new NavMeshPath();
      NavMesh.CalculatePath(navAgent.transform.position, destination, Terrains.Walkable, navMeshPath);
      return navMeshPath.status == NavMeshPathStatus.PathComplete;
    }

    /// <summary>
    ///   Checks if a position is valid by checking if there is a valid position within 1.0f radius,
    ///   returns that valid position. If not valid returns zero vector.
    /// </summary>
    private bool ValidateDestination(Vector3 destination, out Vector3 validPosition)
    {
      destination.y = Terrain.activeTerrain.SampleHeight(destination);
      if (NavMesh.SamplePosition(destination, out var hit, navAgent.height * 2, Terrains.Walkable))
      {
        validPosition = hit.position;
        return ValidatePath(validPosition);
      }

      validPosition = Vector3.zero;
      return false;
    }

    /// <summary>
    ///   Sets a destination opposite direction of threat at the verge of its vision range
    ///   if that position isn't valid the animal will try a different rotations.
    ///   Returns a valid position or zero vector. 
    /// </summary>
    private Vector3 FindFleeDestination(Vector3 threatPosition)
    {
      var navAgentPosition = navAgent.transform.position;
      var visionRange =sphereCollider.radius;
      var directionFromThreat = (navAgentPosition - threatPosition).normalized * visionRange;


      foreach (var angle in _fleeingAngles)
      {
        _fleeDestination = navAgentPosition + (directionFromThreat.normalized * visionRange);
        if (ValidateDestination(_fleeDestination, out var validPosition))
        {
          return validPosition;
        }
        else
        {
          directionFromThreat = RotateDirection(_fleeDestination - navAgentPosition, angle);
        }
      }

      return Vector3.zero;
    }

    private void SetNavAgentSpeed()
    {
      if (staminaController.IsRunning)
      {
        navAgent.speed = genome.WalkingSpeed * 2;
      }
      else
      {
        navAgent.speed = genome.WalkingSpeed; 
      }
    }

    #endregion
  }
}