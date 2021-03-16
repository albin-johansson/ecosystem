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
    public float Thirst { get; private set; }


    private void OnEnable()
    {
      resourceBar.SetMaxValue(maxThirst);
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
      return Thirst > genome.GetThirstThreshold().Value;
    }
  }
}