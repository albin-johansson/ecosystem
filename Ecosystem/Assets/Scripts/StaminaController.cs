using Ecosystem.Genes;
using Ecosystem.UI;
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

    public bool IsRunning { get; set; }

    private float Exhaustion { get; set; }

    private void Start()
    {
      Exhaustion = 50;
      staminaBar.SetMaxValue(maxStamina);
      staminaBar.SetValue(Exhaustion);
    }

    private void Update()
    {
      if (IsRunning)
      {
        Exhaustion += ExhaustionMultiplier * genome.GetSpeed().Value * Time.deltaTime;
        staminaBar.SetValue(Exhaustion);
      }
      else if (Exhaustion > 0)
      {
        Exhaustion -= genome.Metabolism * Time.deltaTime;
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