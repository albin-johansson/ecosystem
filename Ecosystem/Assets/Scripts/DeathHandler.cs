﻿using System.Collections;
using Ecosystem.Logging;
using Ecosystem.Spawning;
using Ecosystem.Util;
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

    private string _keyToPool;
    private GameObject _gameObject;

    public bool isDead; // TODO can probably remove this when realistic food is implemented

    private void Start()
    {
      _keyToPool = gameObject.tag;
    }

    public void Die(CauseOfDeath cause)
    {
      isDead = true; // TODO this is a temporary fix so that multiple wolves can't eat the same prey

      _gameObject = gameObject;

      OnDeath?.Invoke(cause, _gameObject.gameObject);
      animationController.EnterDeathAnimation();

      StartCoroutine(InactivateAfterDelay(3));

      InstantiateCarrion();
    }

    private IEnumerator InactivateAfterDelay(int seconds)
    {
      yield return new WaitForSeconds(seconds);
      ObjectPoolHandler.Instance.ReturnToPool(_keyToPool, _gameObject);
    }

    private void InstantiateCarrion()
    {
      var carrion = ObjectPoolHandler.Instance.Construct("Meat");
      carrion.transform.position = _gameObject.transform.position;
      carrion.transform.rotation = _gameObject.transform.rotation;
      carrion.SetActive(true);

      var nutritionController = carrion.GetComponent<NutritionController>();
      nutritionController.SetNutritionalValue(Nutrition.GetNutritionalValue(_gameObject));
    }
  }
}