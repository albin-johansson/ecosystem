using UnityEngine;

/// <summary>
/// Handles killing the game object.
/// </summary>
public sealed class DeathHandler : MonoBehaviour
{
  void DestroyObjectDelayed()
  {
    //TODO: make the time depend on the death animation
    Destroy(gameObject.gameObject, 5);
  }

  public void KillMe(CauseOfDeath cause)
  {
    // notify log of death. 
    Debug.Log("Something is about to died");
    DestroyObjectDelayed();
  }
}