using System;
using System.Collections;
using Ecosystem.Logging;
using Ecosystem.Spawning;
using UnityEngine;

namespace Ecosystem
{
  /// <summary>
  /// Handles killing the game object.
  /// </summary>
  public sealed class DeathHandler : MonoBehaviour
  {
    [SerializeField] private EcoAnimationController animationController;
    [SerializeField] private string keyToPool;
    private CauseOfDeath _cause;
    private GameObject _gameObject;

    public delegate void DeathEvent(CauseOfDeath cause, GameObject gameObject);

    /// <summary>
    /// This event is emitted every time an entity dies.
    /// </summary>
    public static event DeathEvent OnDeath;

    public void Die(CauseOfDeath cause)
    {
      _gameObject = gameObject;
      _cause = cause;
      OnDeath?.Invoke(_cause, _gameObject.gameObject);
      animationController.EnterDeathAnimation();
      StartCoroutine(InactivateAfterDelay(3));
    }

    private IEnumerator InactivateAfterDelay(int delay)
    {
      yield return new WaitForSeconds(delay);
      ObjectPool.instance.ReturnToPool(keyToPool, _gameObject);
    }
  }
}