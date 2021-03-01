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
      Destroy(gameObject.gameObject);
      gameObject.gameObject.SetActive(false); // Without this, the animal will die more than once
    }
  }
}