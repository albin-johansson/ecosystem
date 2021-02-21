using UnityEngine;

namespace Ecosystem
{
  /// <summary>
  /// Handles killing the game object.
  /// </summary>
  public sealed class DeathHandler : MonoBehaviour
  {
    //[SerializeField] private LoggingManager loggingManager;

    private void DestroyObjectDelayed()
    {
      //TODO: make the time depend on the death animation
      Destroy(gameObject.gameObject);
      Debug.Log("Something has died");
    }

    public void Die(CauseOfDeath cause)
    {
      /*
      IDictionary<string, object> d = new Dictionary<string, object>();
      DeathData dd = new DeathData(cause,1);
      d.Add("death", dd);
      Analytics.CustomEvent("Death", d);
      */

      LoggingManager.OnDeath(new DeathData(cause, gameObject));
      DestroyObjectDelayed();
    }
  }
}