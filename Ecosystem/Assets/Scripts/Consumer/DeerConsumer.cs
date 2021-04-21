using Ecosystem.Genes;
using Ecosystem.Logging;
using Ecosystem.UI;
using UnityEngine;

namespace Ecosystem.Consumer
{
  public sealed class DeerConsumer : MonoBehaviour, IConsumer
  {
    [SerializeField] private AbstractGenome genome;
    [SerializeField] private ResourceBar resourceBar;
    [SerializeField] private DeathHandler deathHandler;
    [SerializeField] private Reproducer reproducer;
    [SerializeField] private float maxHunger = 100;

    private bool _isDead;
    private float _consumed = 0;
    private float _limit = 30;
    private const float Scaler = 4;

    public float Hunger { get; private set; }

    public bool ColliderActive { get; set; }

    public bool IsConsuming { get; set; }

    public GameObject EatingFromGameObject { get; set; }

    private void OnEnable()
    {
      resourceBar.SetMaxValue(maxHunger);
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
        Hunger -= Scaler * Time.deltaTime;
        _consumed += Scaler * Time.deltaTime;
        if (Hunger <= 0 || _consumed > _limit)
        {
          _consumed = 0;
          _limit = Random.Range(20, 40);
          IsConsuming = false;
        }
        resourceBar.SetValue(Hunger);
        return;
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