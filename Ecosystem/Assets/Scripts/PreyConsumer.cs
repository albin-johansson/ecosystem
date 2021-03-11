using Ecosystem.Genes;
using Ecosystem.Logging;
using Ecosystem.UI;
using UnityEngine;

namespace Ecosystem
{
  public sealed class PreyConsumer : MonoBehaviour, IConsumer
  {
    [SerializeField] private Genome genome;
    [SerializeField] private ResourceBar resourceBar;
    [SerializeField] private DeathHandler deathHandler;
    [SerializeField] private double maxHunger = 100;
    [SerializeField] private EcoAnimationController animationController;
    private bool _isDead;

    public double Hunger { get; set; }

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
      if (other.CompareTag("Prey"))
      {
        animationController.EnterAttackAnimation();
        OnPreyConsumed?.Invoke();
        other.gameObject.GetComponent<DeathHandler>().Die(CauseOfDeath.Eaten);
        Hunger = 0;
      }
    }

    public bool IsHungry()
    {
      return Hunger > genome.GetHungerThreshold();
    }
    
    public void SetSaturation(float value)
    {
      Hunger = maxHunger - value;
      resourceBar.SetSaturationValue(value);
    }
    
  }
}