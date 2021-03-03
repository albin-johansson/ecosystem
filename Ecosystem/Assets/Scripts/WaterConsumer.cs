using Ecosystem.Genes;
using Ecosystem.Logging;
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
    private bool _isDead = false;

    public float Thirst { get; private set; }

    private void Start()
    {
      resourceBar.SetMaxValue(maxThirst);
    }

    private void Update()
    {
      if (_isDead)
      {
        return;
      }

      Thirst += genome.GetThirstRate() * Time.deltaTime;
      resourceBar.SetValue(Thirst);

      if (!(Thirst > maxThirst)) return;
      _isDead = true;
      deathHandler.Die(CauseOfDeath.Dehydration);
    }

    private void OnTriggerEnter(Collider other)
    {
      if (other.gameObject.CompareTag("Water"))
      {
        Thirst = 0;
      }
    }

    internal bool IsThirsty()
    {
      return Thirst > genome.GetThirstThreshold();
    }
  }
}