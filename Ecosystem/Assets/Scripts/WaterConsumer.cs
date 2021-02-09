using Stats;
using UnityEngine;
using UnityEngine.Serialization;

public sealed class WaterConsumer : MonoBehaviour
{
  [SerializeField] private Genome genome;

  private double Thirst { get; set; }

  private void Update()
  {
    Thirst += genome.GetThirstRate() * Time.deltaTime;
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