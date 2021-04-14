using Ecosystem.Genes;
using Ecosystem.Logging;
using Ecosystem.UI;
using Ecosystem.Util;
using UnityEngine;

namespace Ecosystem.Consumers
{
  public sealed class BearConsumer : MonoBehaviour, IConsumer
  {
    [SerializeField] private AbstractGenome genome;
    [SerializeField] private ResourceBar resourceBar;
    [SerializeField] private DeathHandler deathHandler;
    [SerializeField] private float maxHunger = 100;
    [SerializeField] private EcoAnimationController animationController;
    [SerializeField] private Reproducer reproducer;
    private bool _isDead;

    public bool IsConsuming { get; set; }
    
    public GameObject EatingFromGameObject { get; set; }

    public float Hunger { get; set; }

    public bool ColliderActive { get; set; }

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
      
      if (EatingFromGameObject && EatingFromGameObject.activeSelf)
      {
        Hunger -= 4 * Time.deltaTime;
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

      resourceBar.SetValue((float) Hunger);
      if (Hunger > maxHunger)
      {
        _isDead = true;
        deathHandler.Die(CauseOfDeath.Starvation);
      }
    }

    private void OnTriggerEnter(Collider other)
    {
      if (!ColliderActive || IsConsuming) return;


      var otherObject = other.gameObject;
      if (Tags.IsPrey(otherObject))
      {
        var otherDeathHandler = otherObject.GetComponentInParent<DeathHandler>();
        if (!otherDeathHandler.isDead)
        {
          IsConsuming = true;
          otherDeathHandler.Die(CauseOfDeath.Eaten);
          var otherNutritionController = otherObject.GetComponentInParent<NutritionController>();
          Hunger -= otherNutritionController.Consume(Hunger);
          OnPreyConsumed?.Invoke();
        }
      }
      else if (Tags.IsMeat(other.gameObject))
      {
        if (other.TryGetComponent(out NutritionController nutritionController))
        {
          Hunger -= nutritionController.Consume(Hunger);
        }
      }
      else if (Tags.IsStaticFood(other.gameObject))
      {
        EatingFromGameObject = other.gameObject;
      }
    }
    
    private void OnTriggerStay(Collider other)
    {
      if (ColliderActive)
      {
        if (Tags.IsMeat(other.gameObject))
        {
          if (other.TryGetComponent(out NutritionController nutritionController))
          {
            Hunger -= nutritionController.Consume(Hunger);
            ColliderActive = false;
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