using Ecosystem.Genes;
using Ecosystem.UI;
using UnityEngine;

namespace Ecosystem
{
  public sealed class StaminaController : MonoBehaviour
  {
    [SerializeField] private AbstractGenome genome;
    [SerializeField] private ResourceBar staminaBar;
    [SerializeField] private double maxStamina = 100;

    private static int _exhaustionMultiplier = 5;
    private static int _baseRunningLevel = 20;
    public bool IsRunning { get; set; }

    private double Exhaustion { get; set; }

    private void Start()
    {
      Exhaustion = 50;
      staminaBar.SetMaxValue((float) maxStamina);
      staminaBar.SetValue((float) Exhaustion);
    }

    private void Update()
    {
      if (IsRunning)
      {
        Exhaustion += _exhaustionMultiplier * genome.GetSpeedFactor().Value * Time.deltaTime;
        staminaBar.SetValue((float) Exhaustion);
      }
      else if (Exhaustion > 0)
      {
        Exhaustion -= genome.Metabolism * Time.deltaTime;
        staminaBar.SetValue((float) Exhaustion);
      }

      if (Exhaustion >= maxStamina)
      {
        IsRunning = false;
        Exhaustion = maxStamina;
      }
    }

    public bool CanRun()
    {
      return Exhaustion < maxStamina - _baseRunningLevel;
    }
  }
}