using UnityEngine;

public sealed class PreyConsumer : MonoBehaviour
{
  [SerializeField] private Genome genome;

  private double Hunger { get; set; }

  private void Update()
  {
    Hunger += genome.GetHungerRate() * Time.deltaTime;
  }

  private void OnTriggerEnter(Collider other)
  {
    if (other.GetComponent<Prey>() != null)
    {
      Destroy(other.gameObject);
      Hunger = 0;
    }
  }

  internal bool IsHungry()
  {
    return Hunger > genome.GetHungerThreshold();
  }
}