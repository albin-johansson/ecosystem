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
    [SerializeField] private Reproducer reproducer;
    private bool _isDead;

    public GameObject EatingFromGameObject { get; set; }
    public double Hunger { get; set; }
    public bool IsAttacking { get; set; }

    public bool CollideActive { get; set; }

    public delegate void FoodEatenEvent(GameObject food);

    /// <summary>
    /// This event is emitted every time a food resource is consumed.
    /// </summary>
    public static event FoodEatenEvent OnFoodEaten;

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

      if (EatingFromGameObject && EatingFromGameObject.activeSelf)
      {
        Hunger -= 4 * Time.deltaTime;
      }
      else
      {
        EatingFromGameObject = null;
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
      if (Tags.IsStaticFood(other.gameObject))
      {
        OnFoodEaten?.Invoke(other.gameObject);
        EatingFromGameObject = other.gameObject;
      }

      if (Tags.IsFood(other.gameObject))
      {
        OnFoodEaten?.Invoke(other.gameObject);
        var gameObjectTag = other.gameObject.tag;
        Hunger = 0;
        if (ObjectPoolHandler.instance.isPoolValid(gameObjectTag))
        {
          ObjectPoolHandler.instance.ReturnToPool(gameObjectTag, other.gameObject);
        }
        else
        {
          Destroy(other.gameObject);
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