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
    [SerializeField] private MovementController movementController;

    private string _keyToPool;

    public bool isDead; // TODO can probably remove this when realistic food is implemented

    private void Start()
    {
      _keyToPool = gameObject.tag;
    }

    private void OnEnable()
    {
      isDead = false;
    }

    public NutritionController Die(CauseOfDeath cause)
    {
      isDead = true; // TODO this is a temporary fix so that multiple wolves can't eat the same prey

      movementController.DisableNavMeshAgent();

      OnDeath?.Invoke(cause, gameObject.gameObject);
      animationController.EnterDeathAnimation();

      StartCoroutine(InactivateAfterDelay(4));

      return InstantiateCarrion();
    }

    private IEnumerator InactivateAfterDelay(int seconds)
    {
      yield return new WaitForSeconds(seconds);
      ObjectPoolHandler.Instance.ReturnToPool(_keyToPool, gameObject);
    }

    private NutritionController InstantiateCarrion()
    {
      var carrion = ObjectPoolHandler.Instance.Construct("Meat");

      var currentTransform = gameObject.transform;
      carrion.transform.position = currentTransform.position;
      carrion.transform.rotation = currentTransform.rotation;
      carrion.SetActive(true);

      var nutritionController = carrion.GetComponent<NutritionController>();
      nutritionController.SetNutritionalValue(Nutrition.GetNutritionalValue(gameObject));
      return nutritionController;
    }
  }
}