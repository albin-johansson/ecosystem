using UnityEngine;

namespace Ecosystem
{
  /// <summary>
  /// Handles killing the game object.
  /// </summary>
  public sealed class DeathHandler : MonoBehaviour
  {
    public delegate void DeathEvent(CauseOfDeath cause, GameObject gameObject);

    public static event DeathEvent OnDeath;

    private void DestroyObjectDelayed()
    {
      //TODO: make the time depend on the death animation
      Destroy(gameObject.gameObject);
    }

    public void Die(CauseOfDeath cause)
    {
      OnDeath?.Invoke(cause, gameObject);
      DestroyObjectDelayed();
    }
  }
}