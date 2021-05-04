using UnityEngine;

namespace Ecosystem.UI
{
    public class ExitMenu : MonoBehaviour
    {
        public void CloseExitMenu()
        {
            gameObject.SetActive(false);
        }

        public void Exit()
        {
            Application.Quit();
        }
    }
}
