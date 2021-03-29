using Ecosystem.Genes;
using Ecosystem.UI;
using UnityEngine;

namespace Ecosystem
{
    public class StaminaController : MonoBehaviour
    {
        [SerializeField] private AbstractGenome genome;
        [SerializeField] private ResourceBar staminaBar;
        [SerializeField] private double maxStamina = 100;
        public bool IsRunning { get; set; }
        
        public double Exhaustion { get; private set; }

        private void Start()
        {
            Exhaustion = 20;
            staminaBar.SetMaxValue((float) maxStamina);
            staminaBar.SetValue((float) Exhaustion);
        }

        private void Update()
        {
            if (IsRunning)
            {
                Exhaustion += 2 * Time.deltaTime; //todo add genome
                staminaBar.SetValue((float) Exhaustion);
            } else if (Exhaustion > 0)
            {
                Exhaustion -= 1 * Time.deltaTime;
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
            return Exhaustion < maxStamina - 5;
        }
    }
}
