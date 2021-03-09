using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ecosystem.UI
{
  public sealed class SceneSelector : MonoBehaviour
  {
    [SerializeField] private string scene;

    public void SelectScene()
    {
      SceneManager.LoadScene($"Scenes/{scene}/{scene}");
    }
  }
}