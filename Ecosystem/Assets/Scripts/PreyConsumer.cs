using Ecosystem.Genes;
using Ecosystem.Logging;
using Ecosystem.UI;
using UnityEngine;

namespace Ecosystem
{
  public sealed class PreyConsumer : MonoBehaviour
  {
    [SerializeField] private Genome genome;
    [SerializeField] private ResourceBar resourceBar;
    [SerializeField] private DeathHandler deathHandler;
    [SerializeField] private double maxHunger = 100;
    [SerializeField] private EcoAnimationController animationController;

    public double Hunger { get; private set; }

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
      Hunger += genome.GetHungerRate() * Time.deltaTime;
      resourceBar.SetValue((float) Hunger);
      if (Hunger > maxHunger)
      {
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

    internal bool IsHungry()
    {
      return Hunger > genome.GetHungerThreshold();
    }
  }
}