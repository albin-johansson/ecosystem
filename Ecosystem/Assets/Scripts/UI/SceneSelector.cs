using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ecosystem.UI
{
  public sealed class SceneSelector : MonoBehaviour
  {
    public void SelectScene()
    {
      switch (gameObject.name)
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