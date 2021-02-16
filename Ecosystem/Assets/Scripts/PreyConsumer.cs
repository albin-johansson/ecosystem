using UnityEngine;

public sealed class PreyConsumer : MonoBehaviour
{
  [SerializeField] private double rate = 0.02;
  [SerializeField] private double threshold = 0.02;
  [SerializeField] private double maxHunger = 100;


  [SerializeField] private DeathHandler deathHandler;
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
    if (other.GetComponent<Prey>() != null)
    {
      other.gameObject.GetComponent<DeathHandler>().Die(CauseOfDeath.Hunted);
      Hunger = 0;
    }
  }

  internal bool IsHungry()
  {
    return Hunger > threshold;
  }
}