using System;
using Ecosystem.Genes;
using Ecosystem.Logging;
using Ecosystem.Spawning;
using Ecosystem.UI;
using Ecosystem.Util;
using UnityEngine;
using Object = System.Object;

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
        OnFoodEaten?.Invoke(other.gameObject);
        var gameObjectTag = other.gameObject.tag;
        if (ObjectPoolHandler.instance.isPoolValid(gameObjectTag))
        {
          ObjectPoolHandler.instance.ReturnToPool(gameObjectTag, other.gameObject);
        }
        Destroy(other.gameObject);
        Hunger = 0;
      }
    }

    public bool IsHungry()
    {
      return Hunger > genome.GetHungerThreshold().Value;
    }
  }
}