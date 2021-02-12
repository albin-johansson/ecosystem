using UnityEngine;

public sealed class PreyConsumer : MonoBehaviour
{
  [SerializeField] private double rate = 0.02;
  [SerializeField] private double threshold = 0.02;
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
    if (other.GetComponent<Prey>() != null)
    {
      Destroy(other.gameObject);
      Hunger = 0;
      }
  }

  internal bool IsHungry()
  {
    return Hunger > threshold;
  }
}
