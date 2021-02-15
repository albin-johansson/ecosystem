using UnityEngine;

public sealed class FoodConsumer : MonoBehaviour
{
  [SerializeField] private double rate = 0.02;
  [SerializeField] private double threshold = 0.1;
  [SerializeField] private DeathHandler deathHandler;
  [SerializeField] private double maxHunger = 100;
  private double Hunger { get; set; }

  private void Update()
  {
    Hunger += rate * Time.deltaTime;
    if (Hunger > maxHunger)
    {
      deathHandler.Die(CauseOfDeath.Starvation);
    }
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
    return Hunger > threshold;
  }
}