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
    [SerializeField] private string meatKeyToPool;
    private CauseOfDeath _cause;
    private GameObject _gameObject;
    public bool _isDead; //can probebly remove this when realistic food is implemented

    public delegate void DeathEvent(CauseOfDeath cause, GameObject gameObject);

    /// <summary>
    /// This event is emitted every time an entity dies.
    /// </summary>
    public static event DeathEvent OnDeath;

    public NutritionController Kill()
    {
      Die(CauseOfDeath.Eaten);

      GameObject carrion = ObjectPoolHandler.instance.GetFromPool(meatKeyToPool);
      carrion.transform.position = _gameObject.transform.position;
      carrion.transform.rotation = _gameObject.transform.rotation;
      carrion.SetActive(true);
      NutritionController nutritionController = carrion.GetComponent<NutritionController>();
      nutritionController.SetNutrition(Nutrition.getNutrition(_gameObject));
      nutritionController.SetKeyToPool("Meat");

      return nutritionController;
    }
    
    public void Die(CauseOfDeath cause)
    {
      _isDead = true; //temporary bug fix so that multiple wolves can´t eat the same prey
      _gameObject = gameObject;
      _cause = cause;
      OnDeath?.Invoke(_cause, _gameObject.gameObject);
      animationController.EnterDeathAnimation();
      StartCoroutine(InactivateAfterDelay(3));
    }

    private IEnumerator InactivateAfterDelay(int delay)
    {
      yield return new WaitForSeconds(delay);
      ObjectPoolHandler.instance.ReturnToPool(keyToPool, _gameObject);
    }
  }
}