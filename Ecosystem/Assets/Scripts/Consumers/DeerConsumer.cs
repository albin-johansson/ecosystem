using Ecosystem.Genes;
using Ecosystem.Logging;
using Ecosystem.Spawning;
using Ecosystem.UI;
using Ecosystem.Util;
using UnityEngine;

namespace Ecosystem.Consumers
{
  public sealed class DeerConsumer : MonoBehaviour, IConsumer
  {
    [SerializeField] private AbstractGenome genome;
    [SerializeField] private ResourceBar resourceBar;
    [SerializeField] private DeathHandler deathHandler;
    [SerializeField] private float maxHunger = 100;
    [SerializeField] private Reproducer reproducer;
    private bool _isDead;
    private double _consumed = 0;
    private double _limit = 30;

    public GameObject EatingFromGameObject { get; set; }
    public float Hunger { get; set; }
    public bool IsConsuming { get; set; }

    public bool ColliderActive { get; set; }
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

      if (IsConsuming)
      {
        Hunger -= 4 * Time.deltaTime;
        _consumed += 4 * Time.deltaTime;
        if (Hunger <= 0 || _consumed > _limit)
        {
          _consumed = 0;
          _limit = Random.Range(20, 40);
          IsConsuming = false;
        }
      }

      if (reproducer.IsPregnant)
      {
        Hunger += genome.Metabolism * AbstractGenome.ChildFoodConsumptionFactor * Time.deltaTime;
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