using Ecosystem.Genes;
using Ecosystem.UI;
using Ecosystem.Util;
using UnityEngine;

namespace Ecosystem
{
  public sealed class StaminaController : MonoBehaviour
  {
    [SerializeField] private AbstractGenome genome;
    [SerializeField] private ResourceBar staminaBar;
    [SerializeField] private float maxStamina = 100;

    private const float ExhaustionMultiplier = 5;
    private const float BaseRunningLevel = 20;
    private float _multiplier = 1;

    public bool IsRunning { get; set; }

    private float Exhaustion { get; set; }

    private void Start()
    {
      Exhaustion = 50;
      staminaBar.SetMaxValue(maxStamina);
      staminaBar.SetValue(Exhaustion);
      if (genome.GetTag().Equals("Rabbit"))
      {
        _multiplier = 4;
      }
      else if (genome.GetTag().Equals("Deer"))
      {
        _multiplier = 2;
      }
    }

    private void Update()
    {
      if (IsRunning)
      {
        Exhaustion += ExhaustionMultiplier * genome.GetSpeed().Value * Time.deltaTime / _multiplier;
        staminaBar.SetValue(Exhaustion);
      }
      else if (Exhaustion > 0)
      {
        Exhaustion -= genome.Metabolism * Time.deltaTime * _multiplier;
        staminaBar.SetValue(Exhaustion);
      }

      if (Exhaustion >= maxStamina)
      {
        IsRunning = false;
        Exhaustion = maxStamina;
      }
    }

    public bool CanRun() => Exhaustion < maxStamina - BaseRunningLevel;
  }
}