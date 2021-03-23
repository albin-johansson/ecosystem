using System;
using Ecosystem.Genes;
using Ecosystem.Logging;
using Ecosystem.Spawning;
using Ecosystem.UI;
using Ecosystem.Util;
using UnityEngine;

namespace Ecosystem
{
  public sealed class FoodConsumer : MonoBehaviour, IConsumer
  {
    [SerializeField] private AbstractGenome genome;
    [SerializeField] private ResourceBar resourceBar;
    [SerializeField] private DeathHandler deathHandler;
    [SerializeField] private double maxHunger = 100;
    private bool _isDead;

    public double Hunger { get; private set; }

    private void OnEnable()
    {
      resourceBar.SetMaxValue((float) maxHunger);
    }

    private void OnDisable()
    {
      Hunger = 0;
    }

    private void Update()
    {
      if (_isDead)
      {
        return;
      }

      Hunger += genome.Metabolism * Time.deltaTime;
      resourceBar.SetValue((float) Hunger);
      if (Hunger > maxHunger)
      {
        _isDead = true;
        deathHandler.Die(CauseOfDeath.Starvation);
      }
    }

    private void OnTriggerEnter(Collider other)
    {
      if (Tags.IsFood(other.gameObject))
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
  }
}