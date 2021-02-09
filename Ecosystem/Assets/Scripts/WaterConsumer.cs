using UnityEngine;
using UnityEngine.Serialization;

public sealed class WaterConsumer : MonoBehaviour
{
  //[SerializeField] private Genome genome;

  private double Thirst { get; set; }

  private void Update()
  {
    Thirst += 1 * Time.deltaTime; //genome.GetThirstRate()
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
    return Thirst > 10; //genome.GetThirstThreshold();
  }
}