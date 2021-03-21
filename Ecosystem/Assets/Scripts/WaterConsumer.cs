using Ecosystem.Genes;
using Ecosystem.Logging;
using Ecosystem.UI;
using UnityEngine;

namespace Ecosystem
{
  public sealed class WaterConsumer : MonoBehaviour
  {
    [SerializeField] private AbstractGenome genome;
    [SerializeField] private ResourceBar resourceBar;
    [SerializeField] private DeathHandler deathHandler;
    [SerializeField] private float maxThirst = 100;

    private bool _isDead;
    public bool IsDrinking { get; private set; }
    public bool CanDrink { get; private set; }
    public float Thirst { get; private set; }


    private void OnEnable()
    {
      resourceBar.SetMaxValue(maxThirst);
    }

    private void OnDisable()
    {
      Thirst = 0;
      IsDrinking = false;
      CanDrink = false;
    }

    private void Update()
    {
      if (_isDead)
      {
        return;
      }

      Thirst += genome.GetThirstRate().Value * Time.deltaTime;
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
        _isDead = true;
        deathHandler.Die(CauseOfDeath.Dehydration);
      }
    }

    private void OnTriggerEnter(Collider other) //TODO: fix bug: enter twice and exit once will yeld a false negative, only possible if watersources are overlapping
    {
      if (other.gameObject.CompareTag("Water"))
      {
        CanDrink = true;
      }
    }

    private void OnTriggerExit(Collider other)
    {
      if (other.gameObject.CompareTag("Water"))
      {
        CanDrink = false;
      }
    }

    public void StopDrinking()
    {
      IsDrinking = false;
    }

    public bool StartDrinking()
    {
      if (CanDrink)
      {
        IsDrinking = true;
      }
      return IsDrinking;
    }

    internal bool IsThirsty()
    {
      return Thirst > genome.GetThirstThreshold().Value;
    }
  }
}