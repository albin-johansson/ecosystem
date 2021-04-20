﻿using Ecosystem.Genes;
using Ecosystem.Logging;
using Ecosystem.UI;
using Ecosystem.Util;
using UnityEngine;

namespace Ecosystem.Consumer
{
  public sealed class BearConsumer : MonoBehaviour, IConsumer
  {
    public delegate void PreyConsumedEvent();

    /// <summary>
    /// This event is emitted every time a prey is consumed.
    /// </summary>
    public static event PreyConsumedEvent OnPreyConsumed;

    public delegate void FoodEatenEvent(GameObject food);

    /// <summary>
    /// This event is emitted every time a food resource is consumed.
    /// </summary>
    public static event FoodEatenEvent OnFoodEaten;

    [SerializeField] private AbstractGenome genome;
    [SerializeField] private ResourceBar resourceBar;
    [SerializeField] private DeathHandler deathHandler;
    [SerializeField] private Reproducer reproducer;
    [SerializeField] private float maxHunger = 100;

    private bool _isDead;
    private const int Scaler = 4;

    public float Hunger { get; private set; }

    public bool ColliderActive { get; set; }

    public bool IsConsuming { get; set; }

    public GameObject EatingFromGameObject { get; set; }

    private void Start()
    {
      resourceBar.SetMaxValue(maxHunger);
    }

    private void Update()
    {
      if (_isDead)
      {
        return;
      }

      if (EatingFromGameObject && EatingFromGameObject.activeSelf)
      {
        Hunger -= Scaler * Time.deltaTime;
        if (Hunger <= 0)
        {
          Hunger = 0;
          EatingFromGameObject = null;
        }
      }
      else
      {
        EatingFromGameObject = null;
      }

      if (reproducer.IsPregnant)
      {
        Hunger += genome.Metabolism * AbstractGenome.ChildFoodConsumptionFactor * Time.deltaTime;
      }
      else
      {
        Hunger += genome.Metabolism * Time.deltaTime;
      }

      resourceBar.SetValue(Hunger);
      if (Hunger > maxHunger)
      {
        _isDead = true;
        deathHandler.Die(CauseOfDeath.Starvation);
      }
    }

    private void OnTriggerEnter(Collider other)
    {
      if (!ColliderActive || IsConsuming)
      {
        return;
      }

      var otherObject = other.gameObject;
      if (Tags.IsPrey(otherObject))
      {
        var otherDeathHandler = otherObject.GetComponentInParent<DeathHandler>();
        if (!otherDeathHandler.isDead)
        {
          var nutritionController = otherDeathHandler.Die(CauseOfDeath.Eaten);
          IsConsuming = true;
          Hunger -= nutritionController.Consume(Hunger);
          OnPreyConsumed?.Invoke();
        }
      }
      else if (Tags.IsMeat(otherObject))
      {
        if (otherObject.TryGetComponent(out NutritionController otherNutritionController))
        {
          Hunger -= otherNutritionController.Consume(Hunger);
        }
      }
      else if (Tags.IsStaticFood(otherObject))
      {
        OnFoodEaten?.Invoke(otherObject);
        EatingFromGameObject = otherObject;
      }
    }

    private void OnTriggerStay(Collider other)
    {
      var otherObject = other.gameObject;
      if (ColliderActive)
      {
        if (Tags.IsMeat(otherObject))
        {
          if (otherObject.TryGetComponent(out NutritionController otherNutritionController))
          {
            Hunger -= otherNutritionController.Consume(Hunger);
          }
        }
      }
    }

    public bool IsHungry()
    {
      return Hunger > genome.GetHungerThreshold().Value;
    }

    public void SetSaturation(float value)
    {
      Hunger = maxHunger - value;
      resourceBar.SetSaturationValue(value);
    }
  }
}