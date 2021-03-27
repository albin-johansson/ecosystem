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

    private bool _isDead;
    public bool IsDrinking { get; private set; }
    public bool CanDrink { get; private set; }
    public float Thirst { get; private set; }
    private int _waterSourcesAvailable = 0;


    private void OnEnable()
    {
      resourceBar.SetMaxValue(maxThirst);
      _waterSourcesAvailable = 0;
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
      Debug.Log(genome.GetHungerRate().Value);

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

    private void OnTriggerEnter(Collider other)
    {
      if (Tags.IsWater(other.gameObject))
      {
        _waterSourcesAvailable++;
        CanDrink = _waterSourcesAvailable > 0;
      }
    }

    private void OnTriggerExit(Collider other)
    {
      if (Tags.IsWater(other.gameObject))
      {
        _waterSourcesAvailable--;
        CanDrink = _waterSourcesAvailable > 0;
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