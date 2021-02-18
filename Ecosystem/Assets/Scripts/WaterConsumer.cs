using Ecosystem.Stats;
using UnityEngine;

namespace Ecosystem
{
  public sealed class WaterConsumer : MonoBehaviour
  {
    [SerializeField] private Genome genome;
    [SerializeField] private ResourceBar resourceBar;
    [SerializeField] private DeathHandler deathHandler;
    [SerializeField] private double maxThirst = 100;

    private double Thirst { get; set; }

    private void Start()
    {
      resourceBar.SetMaxValue((float) maxThirst);
    }

    private void Update()
    {
      Thirst += genome.GetThirstRate() * Time.deltaTime;
      resourceBar.SetValue((float) Thirst);

      if (Thirst > maxThirst)
      {
        deathHandler.Die(CauseOfDeath.Dehydration);
      }
    }

    private void OnTriggerEnter(Collider other)
    {
      if (other.GetComponent<Water>() != null)
      {
        Thirst = 0;
      }
    }

    internal bool IsThirsty()
    {
      return Thirst > genome.GetThirstThreshold();
    }
  }
}