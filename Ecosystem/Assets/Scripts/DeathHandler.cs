using System.Collections;
using Ecosystem.Logging;
using Ecosystem.Spawning;
using Ecosystem.Util;
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
    [SerializeField] private GameObject meatModel;
    private CauseOfDeath _cause;
    private GameObject _gameObject;
    public bool _isDead; //can probebly remove this when realistic food is implemented

    public delegate void DeathEvent(CauseOfDeath cause, GameObject gameObject);

    /// <summary>
    /// This event is emitted every time an entity dies.
    /// </summary>
    public static event DeathEvent OnDeath;

    public void Die(CauseOfDeath cause)
    {
      _isDead = true; //temporary bug fix so that multiple wolves can´t eat the same prey
      _gameObject = gameObject;
      _cause = cause;
      OnDeath?.Invoke(_cause, _gameObject.gameObject);
      animationController.EnterDeathAnimation();
      StartCoroutine(InactivateAfterDelay(3));
      if (Tags.IsPrey(_gameObject))
      {
        GameObject carrion = Instantiate(meatModel, _gameObject.transform.position, Quaternion.identity);
        NutritionController nutritionController = carrion.AddComponent<NutritionController>();
        nutritionController.SetNutrition(Nutrition.getNutrition(_gameObject));
        Debug.Log(nutritionController.nutritionalValue);
      }
    }

    private IEnumerator InactivateAfterDelay(int delay)
    {
      yield return new WaitForSeconds(delay);
      ObjectPoolHandler.instance.ReturnToPool(keyToPool, _gameObject);
    }
  }
}