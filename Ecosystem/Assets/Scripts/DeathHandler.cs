using Ecosystem.Logging;
using UnityEngine;

namespace Ecosystem
{
  /// <summary>
  /// Handles killing the game object.
  /// </summary>
  public sealed class DeathHandler : MonoBehaviour
  {
    public delegate void DeathEvent(CauseOfDeath cause, GameObject gameObject);

    /// <summary>
    /// This event is emitted every time an entity dies.
    /// </summary>
    public static event DeathEvent OnDeath;

    public void Die(CauseOfDeath cause)
    {
      OnDeath?.Invoke(cause, gameObject.gameObject);
      Destroy(gameObject.gameObject, 3); // Make sure that there's enough time to display the death animation
      gameObject.gameObject.SetActive(false); // Without this, the animal will die more than once
      gameObject.gameObject.GetComponent<EcoAnimationController>().EnterDeathAnimation();
    }
  }
}