using System;
using System.Collections;
using Ecosystem.Logging;
using UnityEngine;

namespace Ecosystem
{
  /// <summary>
  /// Handles killing the game object.
  /// </summary>
  public sealed class DeathHandler : MonoBehaviour
  {
    [SerializeField] private EcoAnimationController animationController;
    [SerializeField] private string tagInPool;
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
      gameObject.SetActive(false);
      ObjectPool.instance.ReturnToPool(tagInPool, _gameObject);
      //TODO:Reset all scripts that have a 'state' 
    }
  }
}