using Ecosystem.Genes;
using Ecosystem.Logging;
using Ecosystem.UI;
using UnityEngine;

namespace Ecosystem
{
  public sealed class FoodConsumer : MonoBehaviour, IConsumer
  {
    [SerializeField] private Genome genome;
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

      Hunger += genome.GetHungerRate() * Time.deltaTime;
      resourceBar.SetValue((float) Hunger);
      if (Hunger > maxHunger)
      {
        _isDead = true;
        deathHandler.Die(CauseOfDeath.Starvation);
      }
    }

    private void OnTriggerEnter(Collider other)
    {
      if (other.CompareTag("Food"))
      {
        OnFoodEaten?.Invoke(other.gameObject);
        Destroy(other.gameObject);
        Hunger = 0;
      }
    }

    public bool IsHungry()
    {
      return Hunger > genome.GetHungerThreshold();
    }
  }
}