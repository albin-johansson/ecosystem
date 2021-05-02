using Ecosystem.Logging;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ecosystem.UI
{
    public class ExitMenu : MonoBehaviour
    {
        [SerializeField] private LoggingManager loggingManager;
        [SerializeField] private string currentScene;
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
            SceneManager.LoadScene(currentScene);
        }

        public void Exit()
        {
            Application.Quit();
        }
    }
}
