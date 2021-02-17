using UnityEngine;

/// <summary>
/// Handles killing the game object.
/// </summary>
public sealed class DeathHandler : MonoBehaviour
{
  private void DestroyObjectDelayed()
  {
    //TODO: make the time depend on the death animation
    Destroy(gameObject.gameObject, 5);
  }

  public void Die(CauseOfDeath cause)
  {
    Debug.Log("Something is about to die due to: " + cause);
    DestroyObjectDelayed();
  }
}