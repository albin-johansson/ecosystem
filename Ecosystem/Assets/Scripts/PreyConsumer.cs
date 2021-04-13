using Ecosystem.Genes;
using Ecosystem.Logging;
using Ecosystem.UI;
using Ecosystem.Util;
using UnityEngine;

namespace Ecosystem
{
  public sealed class PreyConsumer : MonoBehaviour, IConsumer
  {
    public delegate void PreyConsumedEvent();

    /// <summary>
    /// This event is emitted every time a prey is consumed.
    /// </summary>
    public static event PreyConsumedEvent OnPreyConsumed;

    [SerializeField] private AbstractGenome genome;
    [SerializeField] private ResourceBar resourceBar;
    [SerializeField] private DeathHandler deathHandler;
    [SerializeField] private Reproducer reproducer;
    [SerializeField] private float maxHunger = 100;

    private bool _isDead;

    public float Hunger { get; private set; }

    public bool ColliderActive { get; set; }

    public bool IsAttacking { get; set; }

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
      if (!ColliderActive || IsAttacking)
      {
        return;
      }

      var otherObject = other.gameObject;
      if (Tags.IsPrey(otherObject))
      {
        var otherDeathHandler = otherObject.GetComponentInParent<DeathHandler>();
        if (!otherDeathHandler.isDead)
        {
          otherDeathHandler.Die(CauseOfDeath.Eaten);

          var otherNutritionController = otherObject.GetComponent<NutritionController>();
          Hunger -= otherNutritionController.Consume(Hunger);

          IsAttacking = true;
          Hunger = 0;

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