using Ecosystem.Systems;
using Ecosystem.Util;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Ecosystem.UI
{
  public sealed class MenuManager : MonoBehaviour
  {
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject dynamicSceneMenu;
    [SerializeField] private GameObject ecsDemoSceneMenu;

    [SerializeField] private Button dynamicSceneStartButton;
    [SerializeField] private Text dynamicRabbitCount;
    [SerializeField] private Text dynamicDeerCount;
    [SerializeField] private Text dynamicWolfCount;
    [SerializeField] private Text dynamicBearCount;
    [SerializeField] private Text dynamicCarrotCount;
    [SerializeField] private Text dynamicBerryBushCount;

    [SerializeField] private Button ecsSceneStartButton;
    [SerializeField] private Text ecsRabbitCount;
    [SerializeField] private Text ecsDeerCount;
    [SerializeField] private Text ecsWolfCount;
    [SerializeField] private Text ecsBearCount;
    [SerializeField] private Text ecsCarrotCount;
    [SerializeField] private Text ecsWaterCount;

    private void Start()
    {
      SceneManager.activeSceneChanged += OnSceneChanged;
      EnterMainMenu();
    }

    public void EnterMainMenu()
    {
      settingsMenu.SetActive(false);
      dynamicSceneMenu.SetActive(false);
      ecsDemoSceneMenu.SetActive(false);

      mainMenu.SetActive(true);
    }

    public void EnterSettingsMenu()
    {
      mainMenu.SetActive(false);
      settingsMenu.SetActive(true);
    }

    public void EnterDynamicSceneMenu()
    {
      mainMenu.SetActive(false);
      dynamicSceneMenu.SetActive(true);
    }

    public void EnterEcsDemoSceneMenu()
    {
      mainMenu.SetActive(false);
      ecsDemoSceneMenu.SetActive(true);
    }

    public void StartForestScene()
    {
      SceneManager.LoadScene("ForestScene");
    }

    public void StartDynamicScene()
    {
      SceneManager.LoadScene("DynamicScene");
    }

    public void StartTestScene()
    {
      SceneManager.LoadScene("PrototypeScene");
    }

    public void StartEcsDemo()
    {
      StartupSpawnSystem.InitialRabbitCount = int.Parse(ecsRabbitCount.text);
      StartupSpawnSystem.InitialDeerCount = int.Parse(ecsDeerCount.text);
      StartupSpawnSystem.InitialWolfCount = int.Parse(ecsWolfCount.text);
      StartupSpawnSystem.InitialBearCount = int.Parse(ecsBearCount.text);
      StartupSpawnSystem.InitialCarrotCount = int.Parse(ecsCarrotCount.text);
      StartupSpawnSystem.InitialWaterCount = int.Parse(ecsWaterCount.text);
      SceneManager.LoadScene("ECSDemo");
    }

    private void OnSceneChanged(Scene current, Scene next)
    {
      if (next.name == "DynamicScene")
      {
        var rabbit = Resources.Load("Prefabs/Animals/EcoRabbit");
        var deer = Resources.Load("Prefabs/Animals/EcoDeer");
        var wolf = Resources.Load("Prefabs/Animals/EcoWolf");
        var bear = Resources.Load("Prefabs/Animals/EcoBear");
        var carrot = Resources.Load("Prefabs/Food/EcoCarrot");
        var berrybush = Resources.Load("Prefabs/Food/EcoBerryBush");

        var nRabbits = int.Parse(dynamicRabbitCount.text);
        var nDeer = int.Parse(dynamicDeerCount.text);
        var nWolves = int.Parse(dynamicWolfCount.text);
        var nBears = int.Parse(dynamicBearCount.text);
        var nCarrots = int.Parse(dynamicCarrotCount.text);
        var nBerrybush = int.Parse(dynamicBerryBushCount.text);

        var terrain = Terrain.activeTerrain;

        Instantiate(nRabbits, rabbit, terrain);
        Instantiate(nWolves, wolf, terrain);
        Instantiate(nDeer, deer, terrain);
        Instantiate(nBears, bear, terrain);
        Instantiate(nCarrots, carrot, terrain);
        Instantiate(nBerrybush, berrybush, terrain, 0.3f);
      }
    }

    public void ValidateDynamicSceneInputs()
    {
      dynamicSceneStartButton.interactable = dynamicRabbitCount.text != "" &&
                                             dynamicDeerCount.text != "" &&
                                             dynamicWolfCount.text != "" &&
                                             dynamicBearCount.text != "" &&
                                             dynamicCarrotCount.text != "" &&
                                             dynamicBerryBushCount.text != "";
    }

    public void ValidateEcsDemoSceneInputs()
    {
      ecsSceneStartButton.interactable = ecsRabbitCount.text != "" &&
                                         ecsDeerCount.text != "" &&
                                         ecsWolfCount.text != "" &&
                                         ecsBearCount.text != "" &&
                                         ecsCarrotCount.text != "" &&
                                         ecsWaterCount.text != "";
    }

    /// <summary>
    ///   Spawns the specified amount of prefabs on random walkable positions on the terrain.
    /// </summary>
    /// <param name="count">the amount of prefabs that will be spawned</param>
    /// <param name="prefab">the prefab that will be spawned</param>
    /// <param name="terrain">the terrain on which the prefab will be spawned</param>
    /// <param name="offset">the offset to the terrain, this is zero by default</param>
    private static void Instantiate(int count, Object prefab, Terrain terrain, float offset = 0)
    {
      var offsetVector = new Vector3(0, offset, 0);
      for (var i = 0; i < count; ++i)
      {
        // There is a small risk that we fail to find a walkable position, but it's fine
        if (Terrains.RandomWalkablePosition(terrain, out var position))
        {
          position += offsetVector;
          Instantiate(prefab, position, Quaternion.identity);
        }
      }
    }
  }
}