using Ecosystem.Genes;
using UnityEngine;

namespace Ecosystem
{
  public sealed class WaterConsumer : MonoBehaviour
  {
    [SerializeField] private Genome genome;
    [SerializeField] private ResourceBar resourceBar;
    [SerializeField] private DeathHandler deathHandler;
    [SerializeField] private float maxThirst = 100;

    private float Thirst { get; set; }

    private void Start()
    {
      resourceBar.SetMaxValue(maxThirst);
    }

    private void Update()
    {
      Thirst += genome.GetThirstRate() * Time.deltaTime;
      resourceBar.SetValue(Thirst);

      if (Thirst > maxThirst)
      {
        deathHandler.Die(CauseOfDeath.Dehydration);
      }
    }

    private void OnTriggerEnter(Collider other)
    {
      if (other.CompareTag("Water"))
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