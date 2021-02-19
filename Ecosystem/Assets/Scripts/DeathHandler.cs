using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

namespace Ecosystem
{
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
      /*
      IDictionary<string, object> d = new Dictionary<string, object>();
      DeathData dd = new DeathData(cause,1);
      d.Add("death", dd);
      Analytics.CustomEvent("Death", d);
      */

      LoggingManager.OnDeath(new DeathData(cause, gameObject.gameObject));

      DestroyObjectDelayed();
    }
  }
}