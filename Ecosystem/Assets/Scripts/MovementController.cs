using System;
using Ecosystem.Genes;
using Ecosystem.Util;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Ecosystem
{
  /// <summary>
  ///  This class contains all functions used for movement and navigation.
  /// </summary>
  public sealed class MovementController : MonoBehaviour
  {
    [SerializeField] private NavMeshAgent navAgent;
    [SerializeField] private AbstractGenome genome;
    [SerializeField] private SphereCollider sphereCollider;
    [SerializeField] private StaminaController staminaController;

    private static readonly float[] FleeingAngles = {-45, 90, 45, -180, -90};
    private static readonly float[] WanderAngles = {0f, -90f, 90f, 180f};

    private float _previousSpeed;

    public Vector3 GetPosition() => navAgent.transform.position;

    #region Basic navigation

    /// <summary>
    ///   Indicates whether or not the specified point is reachable in the nav mesh.
    /// </summary>
    /// <param name="position">the target position that will be checked.</param>
    /// <returns><c>true</c> if there is a valid point within a radius from the point; <c>false</c> otherwise.</returns>
    public bool IsReachable(Vector3 position)
    {
      return ValidateDestination(position, out _); // Ignore the resulting valid position
    }

    /// <summary>
    ///   Indicates whether or not the specified position is within range of the associated sphere collider.
    /// </summary>
    /// <param name="position">the position that will be checked.</param>
    /// <returns><c>true</c> if the position is within the sphere collider; <c>false</c> otherwise.</returns>
    public bool IsWithinSphere(Vector3 position)
    {
      if (IsReachable(position))
      {
        return sphereCollider.radius >= Math.Floor(Vector3.Distance(GetPosition(), position));
      }
      else
      {
        return false;
      }
    }

    /// <summary>
    ///   Sets whether or not the nav agent is stationary.
    /// </summary>
    /// <remarks>This function is harmless to call even if the desired action already has been taken.</remarks>
    /// <param name="standStill"><c>true</c> if the nav agent should be stopped; <c>false</c> otherwise.</param>
    public void SetStandingStill(bool standStill)
    {
      if (standStill)
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
    ///   Updates the speed of the associated NavAgent, according to the current stamina.
    /// </summary>
    private void UpdateSpeed()
    {
      if (staminaController.IsRunning)
      {
        navAgent.speed = genome.WalkingSpeed * 1.5f;
      }
      else
      {
        navAgent.speed = genome.WalkingSpeed;
      }
    }

    /// <summary>
    ///   Sets the destination of the animal and updates the speed of the animal.
    /// </summary>
    /// <remarks>This function doesn't validate the specified position.</remarks>
    /// <param name="destination">the new destination of the animal.</param>
    public void SetDestination(Vector3 destination)
    {
      UpdateSpeed();
      navAgent.SetDestination(destination);
    }

    /// <summary>
    ///   Makes the animal go to the specified target position, if the position is valid.
    /// </summary>
    /// <param name="targetPosition">the target position that will be validated and potentially pursued.</param>
    public void SetDestinationIfValid(Vector3 targetPosition)
    {
      if (ValidateDestination(targetPosition, out var validPosition))
      {
        SetDestination(validPosition);
      }
    }

    #endregion

    #region Fleeing

    /// <summary>
    ///   Attempts to start fleeing from a threat.
    /// </summary>
    /// <param name="threatPosition">the position of the threat.</param>
    /// <returns><c>true</c> if the animal successfully started fleeing; <c>false</c> otherwise.</returns>
    public bool FleeFrom(Vector3 threatPosition)
    {
      if (FindFleeDestination(threatPosition, out var fleeDestination))
      {
        SetDestination(fleeDestination);
        return true;
      }
      else
      {
        return false;
      }
    }

    /// <summary>
    ///   Updates the fleeing destination of the animal if it doesn't already have a target destination, or if the
    ///   animal has almost reached the current flee destination. 
    /// </summary>
    /// <param name="threatPosition">the position of the threat.</param>
    /// <returns><c>true</c> if the fleeing destination was successfully updated; <c>false</c> otherwise.</returns>
    public bool UpdateFleeing(Vector3 threatPosition)
    {
      if (!navAgent.hasPath || navAgent.remainingDistance < 1.0f)
      {
        return FleeFrom(threatPosition);
      }

      return true;
    }

    /// <summary>
    ///   Attempts to find a valid flee destination to avoid a threat at the specified position. This function will try
    ///   different destinations in the opposite direction of the threat, at the verge of the vision range of the animal.
    /// </summary>
    /// <param name="threatPosition">the position of the threat.</param>
    /// <param name="fleeDestination">the resulting flee destination, set to <c>Vector3.zero</c> upon failure.</param>
    /// <returns><c>true</c> if a valid flee destination was found; <c>false</c> otherwise.</returns>
    private bool FindFleeDestination(in Vector3 threatPosition, out Vector3 fleeDestination)
    {
      var navAgentPosition = GetPosition();
      var visionRange = sphereCollider.radius;
      var directionFromThreat = (navAgentPosition - threatPosition).normalized * visionRange;

      foreach (var angle in FleeingAngles)
      {
        var destination = navAgentPosition + directionFromThreat.normalized * visionRange;
        if (ValidateDestination(destination, out var validDestination))
        {
          fleeDestination = validDestination;
          return true;
        }
        else
        {
          directionFromThreat = MathUtils.RotateDirectionY(destination - navAgentPosition, angle);
        }
      }

      fleeDestination = Vector3.zero;
      return false;
    }

    #endregion

    #region Wandering

    /// <summary>
    ///   Make the animal move towards a position at the edge of the vision range, at an angle within a half circle of
    ///   the direction that the animal is currently facing.
    /// </summary>
    public void StartWander()
    {
      var agentTransform = navAgent.transform;
      var direction = agentTransform.forward.normalized * sphereCollider.radius;

      foreach (var angle in WanderAngles)
      {
        var rotatedDirection = MathUtils.RotateDirectionY(direction, Random.Range(angle - 45f, angle + 45f));

        var position = agentTransform.position;
        var destination = position + rotatedDirection;

        if (ValidateDestination(destination, out var validPosition))
        {
          SetDestination(validPosition);
          return;
        }
      }
    }

    /// <summary>
    ///   Updates the wandering of the animal if it has no current destination of if it is close to its current
    ///   destination.
    /// </summary>
    public void UpdateWander()
    {
      if (!navAgent.hasPath || navAgent.remainingDistance < 1.0f)
      {
        StartWander();
      }
    }

    #endregion

    #region Validation

    /// <summary>
    ///   Validates if there is a path to a given point
    /// </summary>
    private bool ValidatePath(Vector3 destination)
    {
      var navMeshPath = new NavMeshPath();
      NavMesh.CalculatePath(GetPosition(), destination, Terrains.Walkable, navMeshPath);
      return navMeshPath.status == NavMeshPathStatus.PathComplete;
    }

    /// <summary>
    ///   Attempts to validate a specified destination. 
    /// </summary>
    /// <param name="destination">the destination that will be validated.</param>
    /// <param name="validPosition">the resulting valid position; set to <c>Vector3.zero</c> on failure.</param>
    /// <returns><c>true</c> if a valid position was found; <c>false</c> otherwise.</returns>
    private bool ValidateDestination(Vector3 destination, out Vector3 validPosition)
    {
      destination.y = Terrain.activeTerrain.SampleHeight(destination);
      if (NavMesh.SamplePosition(destination, out var hit, navAgent.height * 2, Terrains.Walkable))
      {
        validPosition = hit.position;
        return ValidatePath(validPosition);
      }
      else
      {
        validPosition = Vector3.zero;
        return false;
      }
    }

    #endregion
  }
}