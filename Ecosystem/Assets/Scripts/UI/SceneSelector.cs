using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ecosystem.UI
{
  public sealed class SceneSelector : MonoBehaviour
  {
    [SerializeField] private string button;
    public void SelectScene()
    {
      switch (button)
      {
        case "ForestScene":
          SceneManager.LoadScene("Scenes/ForestScene/ForestScene");
          break;
        case "SampleScene":
          SceneManager.LoadScene("Scenes/PrototypeScene/PrototypeScene");
          break;
        default:
          break;
      }
    }
  }
}