using UnityEngine;

public sealed class FoodConsumer : MonoBehaviour
{
  [SerializeField] private double rate = 0.02;
  [SerializeField] private double threshold = 0.1;
  [SerializeField] private ResourceBar resourceBar;

  private double Hunger { get; set; }

  private void Start()
  {
    resourceBar.SetMaxValue(1f);   //Should be set to max hunger before death
  }

  private void Update()
  {
    Hunger += rate * Time.deltaTime;
    resourceBar.SetValue((float)Hunger);
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