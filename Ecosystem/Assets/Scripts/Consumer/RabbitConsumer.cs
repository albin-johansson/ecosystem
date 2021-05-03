using Ecosystem.Genes;
using Ecosystem.Logging;
using Ecosystem.Spawning;
using Ecosystem.UI;
using Ecosystem.Util;
using UnityEngine;

namespace Ecosystem.Consumer
{
  public sealed class RabbitConsumer : MonoBehaviour, IConsumer
  {
    [SerializeField] private AbstractGenome genome;
    [SerializeField] private ResourceBar resourceBar;
    [SerializeField] private DeathHandler deathHandler;
    [SerializeField] private Reproducer reproducer;
    [SerializeField] private float maxHunger = 100;

    private bool _isDead;

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

      if (EatingFromGameObject && EatingFromGameObject.activeInHierarchy)
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

      resourceBar.SetValue(Hunger);
      if (Hunger >= maxHunger)
      {
        _isDead = true;
        deathHandler.Die(CauseOfDeath.Starvation);
      }
    }

    private void OnTriggerEnter(Collider other)
    {
      var otherObject = other.gameObject;

      if (Tags.IsStaticFood(otherObject))
      {
        EatingFromGameObject = otherObject;
      }
      else if (Tags.IsFood(otherObject))
      {
        if (other.TryGetComponent(out NutritionController nutritionController))
        {
          Hunger -= nutritionController.Consume(Hunger);
        }

        ObjectPoolHandler.Instance.ReturnOrDestroy(otherObject.tag, otherObject);
      }
    }

    private void OnTriggerExit(Collider other)
    {
      if (other.gameObject == EatingFromGameObject)
      {
        EatingFromGameObject = null;
      }
    }

    public bool IsHungry()
    {
      return Hunger > genome.GetHungerThreshold().Value;
    }

    public void SetSaturation(float value)
    {
      Hunger = maxHunger - value;
      if (Hunger < 0)
      {
        Hunger = 0;
      }
      _isDead = false;
      resourceBar.SetSaturationValue(value);
    }
  }
}