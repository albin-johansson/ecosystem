﻿using Ecosystem.Genes;
using Ecosystem.Logging;
using Ecosystem.UI;
using Ecosystem.Util;
using UnityEngine;

namespace Ecosystem
{
  public sealed class PreyConsumer : MonoBehaviour, IConsumer
  {
    [SerializeField] private AbstractGenome genome;
    [SerializeField] private ResourceBar resourceBar;
    [SerializeField] private DeathHandler deathHandler;
    [SerializeField] private double maxHunger = 100;
    [SerializeField] private EcoAnimationController animationController;
    [SerializeField] private Reproducer reproducer;
    [SerializeField] private GameObject meat;
    private bool _isDead;

    public bool IsAttacking { get; set; }

    public double Hunger { get; set; }

    public bool CollideActive { get; set; }

    public delegate void PreyConsumedEvent();

    /// <summary>
    /// This event is emitted every time a prey is consumed.
    /// </summary>
    public static event PreyConsumedEvent OnPreyConsumed;

    private void Start()
    {
      resourceBar.SetMaxValue((float) maxHunger);
    }

    private void Update()
    {
      if (_isDead)
      {
        return;
      }

      if (reproducer.IsPregnant)
      {
        Hunger += genome.Metabolism * genome.GetChildFoodConsumtionFactor() * Time.deltaTime;
      }
      else
      {
        Hunger += genome.Metabolism * Time.deltaTime;
      }

      resourceBar.SetValue((float) Hunger);
      if (Hunger > maxHunger)
      {
        _isDead = true;
        deathHandler.Die(CauseOfDeath.Starvation);
      }
    }

    private void OnTriggerEnter(Collider other)
    {
      if (!CollideActive || IsAttacking)
      {
        return;
      }

      if (Tags.IsPrey(other.gameObject))
      {
        IsAttacking = true;
        OnPreyConsumed?.Invoke();
        var preyPosition = other.gameObject.transform.position;

        other.gameObject.GetComponent<DeathHandler>().Die(CauseOfDeath.Eaten);

        GameObject corpse = Instantiate(meat, preyPosition, Quaternion.identity);

        if (corpse.TryGetComponent(out NutritionController nutritionController))
        {
          Hunger -= nutritionController.Consume(Hunger);
        }
      }
      else if (Tags.IsMeat(other.gameObject))
      {
        if (other.TryGetComponent(out NutritionController nutritionController))
        {
          Hunger -= nutritionController.Consume(Hunger);
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