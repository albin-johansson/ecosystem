using UnityEngine;

public sealed class WaterConsumer : MonoBehaviour
{
  [SerializeField] private double rate = 0.02;
  [SerializeField] private double threshold = 0.1;
  [SerializeField] private ThirstBar thirstBar;

  private double Thirst { get; set; }

  private void Start()
  {
    thirstBar.SetMaxThirst(1f);
  }

  private void Update()
  {
    Thirst += rate * Time.deltaTime;
    thirstBar.SetThirst((float) Thirst);
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
    return Thirst > threshold;
  }
}