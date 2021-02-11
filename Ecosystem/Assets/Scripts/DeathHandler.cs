using UnityEngine;

/// <summary>
/// Handles killing the game object in a more 
/// </summary>
public class DeathHandler : MonoBehaviour
{
  void DestroyObjectDelayed()
  {
    //TODO: make the time depend on the death animation
    Destroy(gameObject.gameObject);
  }

  public void KillMe(CauseOfDeath cause)
  {
    // notify log of death. 
    Debug.Log("Something is about to died");
    DestroyObjectDelayed();
  }
}