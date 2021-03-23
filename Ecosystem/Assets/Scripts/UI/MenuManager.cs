using System.Collections.Generic;
using Ecosystem.Genes;
using Ecosystem.Util;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Ecosystem.UI
{
  public sealed class MenuManager : MonoBehaviour
  {
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject dynamicSceneMenu;
    [SerializeField] private GameObject genomeTypeMenu;
    [SerializeField] private Button dynamicSceneStartButton;
    [SerializeField] private Text dynamicRabbitCount;
    [SerializeField] private Text dynamicDeerCount;
    [SerializeField] private Text dynamicWolfCount;
    [SerializeField] private Text dynamicBearCount;

    [SerializeField] private Text dynamicCarrotCount;
    //[SerializeField] private Text setOrRandom;

    [SerializeField] private Text rabbitGenome;
    [SerializeField] private Text deerGenome;
    [SerializeField] private Text wolfGenome;
    [SerializeField] private Text bearGenome;


    private void Start()
    {
      SceneManager.activeSceneChanged += OnSceneChanged;
      EnterMainMenu();
    }

    public void EnterMainMenu()
    {
      dynamicSceneMenu.SetActive(false);
      genomeTypeMenu.SetActive(false);
      mainMenu.SetActive(true);
    }

    public void EnterDynamicSceneMenu()
    {
      mainMenu.SetActive(false);
      dynamicSceneMenu.SetActive(true);
    }

    public void EnterGenomeTypeMenu()
    {
      Debug.Log("Yo bro");
      mainMenu.SetActive(false);
      genomeTypeMenu.SetActive(true);
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

    private void OnSceneChanged(Scene current, Scene next)
    {
      if (next.name == "DynamicScene")
      {
        var rabbit = Resources.Load("Prefabs/Animals/EcoRabbit");
        var deer = Resources.Load("Prefabs/Animals/EcoDeer");
        var wolf = Resources.Load("Prefabs/Animals/EcoWolf");
        var bear = Resources.Load("Prefabs/Animals/EcoBear");
        var carrot = Resources.Load("Prefabs/Food/EcoCarrot");

        var nRabbits = int.Parse(dynamicRabbitCount.text);
        var nDeer = int.Parse(dynamicDeerCount.text);
        var nWolves = int.Parse(dynamicWolfCount.text);
        var nBears = int.Parse(dynamicBearCount.text);
        var nCarrots = int.Parse(dynamicCarrotCount.text);
        var mutateOrSet = "y"; //setOrRandom.text;

        var terrain = Terrain.activeTerrain;

        if (mutateOrSet.Equals("n"))
        {
          RabbitGenome.SetPreset(new Dictionary<GeneType, Preset>()
          {
            {GeneType.HungerRate, new Preset(0, 2, new[] {0.5f, 1f})},
            {GeneType.HungerThreshold, new Preset(0, 2, new[] {0.5f, 1f})},
            {GeneType.ThirstRate, new Preset(0, 2, new[] {0.5f, 1f})},
            {GeneType.ThirstThreshold, new Preset(0, 2, new[] {0.5f, 1f})},
            {GeneType.Vision, new Preset(0, 2, new[] {0.5f, 1f})},
            {GeneType.SpeedFactor, new Preset(0, 2, new[] {0.5f, 1f})},
            {GeneType.SizeFactor, new Preset(0, 2, new[] {0.5f, 1f})},
            {GeneType.DesirabilityScore, new Preset(0, 2, new[] {0.5f, 1f})},
            {GeneType.GestationPeriod, new Preset(0, 2, new[] {0.5f, 1f})},
            {GeneType.SexualMaturityTime, new Preset(0, 2, new[] {0.5f, 1f})}
          }, 0f);
        }
        else if (mutateOrSet.Equals("y"))
        {
          RabbitGenome.SetPreset(new Dictionary<GeneType, Preset>()
          {
            {GeneType.HungerRate, new Preset(0, 2, new[] {0.7f})},
            {GeneType.HungerThreshold, new Preset(0, 2, new[] {0.7f})},
            {GeneType.ThirstRate, new Preset(0, 2, new[] {0.7f})},
            {GeneType.ThirstThreshold, new Preset(0, 2, new[] {0.7f})},
            {GeneType.Vision, new Preset(0, 2, new[] {0.7f})},
            {GeneType.SpeedFactor, new Preset(0, 2, new[] {0.7f})},
            {GeneType.SizeFactor, new Preset(0, 2, new[] {0.7f})},
            {GeneType.DesirabilityScore, new Preset(0, 2, new[] {0.7f})},
            {GeneType.GestationPeriod, new Preset(0, 2, new[] {0.7f})},
            {GeneType.SexualMaturityTime, new Preset(0, 2, new[] {0.7f})}
          }, 0.1f);
        }

        Instantiate(nRabbits, rabbit, terrain);
        Instantiate(nWolves, wolf, terrain);
        Instantiate(nDeer, deer, terrain);
        Instantiate(nBears, bear, terrain);
        Instantiate(nCarrots, carrot, terrain);
      }
    }

    public void ValidateDynamicSceneInputs()
    {
      dynamicSceneStartButton.interactable = dynamicRabbitCount.text != "" &&
                                             dynamicDeerCount.text != "" &&
                                             dynamicWolfCount.text != "" &&
                                             dynamicBearCount.text != "" &&
                                             dynamicCarrotCount.text != "";
    }

    /// <summary>
    ///   Spawns the specified amount of prefabs on random walkable positions on the terrain.
    /// </summary>
    /// <param name="count">the amount of prefabs that will be spawned</param>
    /// <param name="prefab">the prefab that will be spawned</param>
    /// <param name="terrain">the terrain on which the prefab will be spawned</param>
    private static void Instantiate(int count, Object prefab, Terrain terrain)
    {
      for (var i = 0; i < count; ++i)
      {
        // There is a small risk that we fail to find a walkable position, but it's fine
        if (Terrains.RandomWalkablePosition(terrain, out var position))
        {
          Instantiate(prefab, position, Quaternion.identity);
        }
      }
    }
  }
}