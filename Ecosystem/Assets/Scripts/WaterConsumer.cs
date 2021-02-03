using UnityEngine;

public sealed class WaterConsumer : MonoBehaviour
{
  [SerializeField] private Genom genom;
  private double Thirst { get; set; }

  private void Update()
  {
    Thirst += genom.getThirstRate() * Time.deltaTime;
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
    return Thirst > genom.getThirstThreshold();
  }
}