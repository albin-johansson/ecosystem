using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ecosystem.UI
{
  public sealed class MenuManager : MonoBehaviour
  {
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject dynamicSceneMenu;

    private void Start()
    {
      EnterMainMenu();
    }

    public void EnterMainMenu()
    {
      dynamicSceneMenu.SetActive(false);
      mainMenu.SetActive(true);
    }

    public void EnterDynamicSceneMenu()
    {
      mainMenu.SetActive(false);
      dynamicSceneMenu.SetActive(true);
    }

    public void StartForestScene()
    {
      SceneManager.LoadScene("ForestScene");
    }

    public void StartDynamicScene()
    {
    }

    public void StartTestScene()
    {
      SceneManager.LoadScene("PrototypeScene");
      
    }

    public void OnRabbitCountChange(string count)
    {
    }
  }
}