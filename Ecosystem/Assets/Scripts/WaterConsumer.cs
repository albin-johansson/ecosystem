using UnityEngine;

public sealed class WaterConsumer : MonoBehaviour
{
  [SerializeField] private double rate = 0.02;
  [SerializeField] private double threshold = 0.1;
  [SerializeField] private ResourceBar resourceBar;
  [SerializeField] private double maxThirst = 100;


  [SerializeField] private DeathHandler deathHandler;
  private double Thirst { get; set; }

  private void Start()
  {
    resourceBar.SetMaxValue(1f);   //Should be set to max thirst before death
  }

  private void Update()
  {
    Thirst += rate * Time.deltaTime;
    resourceBar.SetValue((float)Thirst);

    if (Thirst > maxThirst)
    {
      deathHandler.Die(CauseOfDeath.Dehydration);
    }
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
