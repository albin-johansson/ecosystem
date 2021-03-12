using UnityEngine;
using UnityEngine.UI;

namespace Ecosystem.UI
{
    public sealed class GraphicsSettings : MonoBehaviour
    {
        [SerializeField] private Image settingsWindow;

        private bool _windowIsOpen;
        public static int _postProcessingActive;
        
        public void SettingsMenu()
        {
            if (_windowIsOpen)
            {
                _windowIsOpen = false;
                settingsWindow.gameObject.SetActive(false);
            }
            else
            {
                _windowIsOpen = true;
                settingsWindow.gameObject.SetActive(true);
            }

            _postProcessingActive = 10;
        }
    }
}
