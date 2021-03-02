using Ecosystem.Genes;
using Ecosystem.UI;
using UnityEngine;

namespace Ecosystem
{
  public sealed class WaterConsumer : MonoBehaviour
  {
    [SerializeField] private Genome genome;
    [SerializeField] private ResourceBar resourceBar;
    [SerializeField] private DeathHandler deathHandler;
    [SerializeField] private float maxThirst = 100;
   
    public bool IsDrinking { get; private set; }

    public float Thirst { get; private set; }

    private void Start()
    {
      resourceBar.SetMaxValue(maxThirst);
    }

    private void Update()
    {
      Thirst += genome.GetThirstRate() * Time.deltaTime;
      resourceBar.SetValue(Thirst);
      if (IsDrinking)
      {
        Thirst -= 10f * Time.deltaTime; //TODO Add drinkrate
        if (Thirst <= 0)
        {
          Thirst = 0;
          IsDrinking = false;
        }
      }

      if (Thirst > maxThirst)
      {
        deathHandler.Die(CauseOfDeath.Dehydration);
      }
    }

    private void OnTriggerEnter(Collider other)
    {
      if (other.gameObject.CompareTag("Water") && IsThirsty())
      {
        IsDrinking = true;
      }
    }

    public void StopDrinking()
    {
      IsDrinking = false;
    }

    internal bool IsThirsty()
    {
      return Thirst > genome.GetThirstThreshold();
    }
  }
}
