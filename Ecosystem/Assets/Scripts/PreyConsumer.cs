using Ecosystem.Genes;
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

    private double Hunger { get; set; }

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
        //deathHandler.Die(CauseOfDeath.Hunted, other.gameObject);
        other.gameObject.GetComponent<DeathHandler>().Die(CauseOfDeath.Hunted);
        Hunger = 0;
      }
    }

    internal bool IsHungry()
    {
      return Hunger > genome.GetHungerThreshold();
    }
  }
}