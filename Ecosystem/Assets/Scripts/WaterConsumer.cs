using Ecosystem.Genes;
using Ecosystem.Logging;
using Ecosystem.UI;
using Ecosystem.Util;
using UnityEngine;

namespace Ecosystem
{
  public sealed class WaterConsumer : MonoBehaviour
  {
    [SerializeField] private AbstractGenome genome;
    [SerializeField] private ResourceBar resourceBar;
    [SerializeField] private DeathHandler deathHandler;
    [SerializeField] private float maxThirst = 100;

    private int _waterSourcesAvailable;
    private bool _isDead;

    public bool IsDrinking { get; private set; }

    public float Thirst { get; private set; }

    public bool CanDrink => _waterSourcesAvailable > 0;

    private void OnEnable()
    {
      resourceBar.SetMaxValue(maxThirst);
      _waterSourcesAvailable = 0;
    }

    private void OnDisable()
    {
      Thirst = 0;
      IsDrinking = false;
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
        Thirst -= 10f * Time.deltaTime; //TODO Add drink rate
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

    private void OnTriggerEnter(Collider other)
    {
      if (Tags.IsWater(other.gameObject))
      {
        ++_waterSourcesAvailable;
      }
    }

    private void OnTriggerExit(Collider other)
    {
      if (Tags.IsWater(other.gameObject))
      {
        --_waterSourcesAvailable;
      }
    }

    public void StopDrinking()
    {
      IsDrinking = false;
    }

    public void SetHydration(float hydration)
    {
      Thirst = maxThirst - hydration;
      if (Thirst < 0)
      {
        Thirst = 0;
      }
      resourceBar.SetValue(Thirst);
      _isDead = false;
    }

    public void StartDrinking()
    {
      if (CanDrink)
      {
        IsDrinking = true;
      }
    }

    internal bool IsThirsty()
    {
      return Thirst > genome.GetThirstThreshold().Value;
    }
  }
}
