using System.Collections;
using Ecosystem.Logging;
using Ecosystem.Spawning;
using UnityEngine;

namespace Ecosystem
{
  /// <summary>
  ///   Handles the deaths of animals.
  /// </summary>
  public sealed class DeathHandler : MonoBehaviour
  {
    public delegate void DeathEvent(CauseOfDeath cause, GameObject gameObject);

    /// <summary>
    ///   This event is emitted every time an entity dies.
    /// </summary>
    public static event DeathEvent OnDeath;

    [SerializeField] private EcoAnimationController animationController;
    [SerializeField] private string keyToPool;

    private CauseOfDeath _cause;
    private GameObject _gameObject;
    public bool isDead; // TODO can probably remove this when realistic food is implemented

    public void Die(CauseOfDeath cause)
    {
      isDead = true; // TODO this is a temporary fix so that multiple wolves can't eat the same prey

      _gameObject = gameObject;
      _cause = cause;

      OnDeath?.Invoke(_cause, _gameObject.gameObject);
      animationController.EnterDeathAnimation();

      StartCoroutine(InactivateAfterDelay(3));
    }

    private IEnumerator InactivateAfterDelay(int seconds)
    {
      yield return new WaitForSeconds(seconds);
      ObjectPoolHandler.instance.ReturnToPool(keyToPool, _gameObject);
    }
  }
}