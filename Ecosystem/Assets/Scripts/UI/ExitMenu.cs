using Ecosystem.Logging;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ecosystem.UI
{
    public class ExitMenu : MonoBehaviour
    {
        [SerializeField] private LoggingManager loggingManager;
        public void CloseExitMenu()
        {
            gameObject.SetActive(false);
        }

        public void ReturnToMainMenu()
        {
            loggingManager.SceneExit();
            SceneManager.LoadScene("MainMenu");
        }

        public void RestartScene()
        {
            loggingManager.SceneExit();
            SceneManager.LoadScene("ForestScene");
        }

        public void Exit()
        {
            Application.Quit();
        }
    }
}
