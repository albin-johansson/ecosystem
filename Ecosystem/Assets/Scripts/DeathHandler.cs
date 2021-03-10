using System;
using Ecosystem.Logging;
using UnityEngine;

namespace Ecosystem
{
  /// <summary>
  /// Handles killing the game object.
  /// </summary>
  public sealed class DeathHandler : MonoBehaviour
  {
    [SerializeField] private EcoAnimationController animationController;
    private CauseOfDeath _cause;

    public delegate void DeathEvent(CauseOfDeath cause, GameObject gameObject);

    /// <summary>
    /// This event is emitted every time an entity dies.
    /// </summary>
    public static event DeathEvent OnDeath;

    public void Die(CauseOfDeath cause)
    {
      _cause = cause;
      OnDeath?.Invoke(_cause, gameObject.gameObject);
      Destroy(gameObject.gameObject, 3); // Make sure that there's enough time to display the death animation 
      animationController.EnterDeathAnimation();
    }
  }
}