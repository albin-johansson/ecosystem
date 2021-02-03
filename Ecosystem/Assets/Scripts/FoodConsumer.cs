using UnityEngine;

public sealed class FoodConsumer : MonoBehaviour
{
  [SerializeField] private Genom genom;

  private double Hunger { get; set; }

  private void Update()
  {
    Hunger += genom.getHungerRate() * Time.deltaTime;
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
    return Hunger > genom.getHungerThreshold();
  }
}