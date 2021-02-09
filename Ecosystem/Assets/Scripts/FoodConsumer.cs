using Stats;
using UnityEngine;
using UnityEngine.Serialization;

public sealed class FoodConsumer : MonoBehaviour
{
  [SerializeField] private Genome genome;

  private double Hunger { get; set; }

  private void Update()
  {
    Hunger += genome.GetHungerRate() * Time.deltaTime;
  }

  private void OnTriggerEnter(Collider other)
  {
    if (other.GetComponent<Food>() != null)
    {
      Destroy(other);
      Hunger = 0;
    }
  }

  internal bool IsHungry()
  {
    return Hunger > genome.GetHungerThreshold();
  }
}