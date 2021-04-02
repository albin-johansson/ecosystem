using System.Collections.Generic;
using Ecosystem.Genes;
using Ecosystem.Util;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Ecosystem.UI
{
  public sealed class MenuManager : MonoBehaviour
  {
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject dynamicSceneMenu;
    [SerializeField] private GameObject genomeSettingsMenu;
    [SerializeField] private GameObject settingsButton;
    [SerializeField] private Button dynamicSceneStartButton;
    [SerializeField] private Text dynamicRabbitCount;
    [SerializeField] private Text dynamicDeerCount;
    [SerializeField] private Text dynamicWolfCount;
    [SerializeField] private Text dynamicBearCount;
    [SerializeField] private Text dynamicCarrotCount;
    [SerializeField] private Toggle rabbitSet;
    [SerializeField] private Toggle rabbitMutate;
    [SerializeField] private Toggle wolfSet;
    [SerializeField] private Toggle wolfMutate;
    [SerializeField] private Toggle deerSet;
    [SerializeField] private Toggle deerMutate;
    [SerializeField] private Toggle bearSet;
    [SerializeField] private Toggle bearMutate;

    private Dictionary<GeneType, Preset> rabbitPreset;
    private Dictionary<GeneType, Preset> wolfPreset;
    private Dictionary<GeneType, Preset> deerPreset;
    private Dictionary<GeneType, Preset> bearPreset;
    private float rabbitMutateChance = 0.05f;
    private float wolfMutateChance = 0.05f;
    private float deerMutateChance = 0.05f;
    private float bearMutateChance = 0.05f;

    private void Start()
    {
      rabbitPreset = RabbitGenome.DefaultSet;
      wolfPreset = WolfGenome.DefaultSet;
      deerPreset = DeerGenome.DefaultSet;
      bearPreset = BearGenome.DefaultSet;
      rabbitSet.onValueChanged.AddListener(delegate { ToggleSetRabbit(rabbitSet); });
      rabbitMutate.onValueChanged.AddListener(delegate { ToggleMutateRabbit(rabbitMutate); });
      wolfSet.onValueChanged.AddListener(delegate { ToggleSetWolf(wolfSet); });
      wolfMutate.onValueChanged.AddListener(delegate { ToggleMutateWolf(wolfMutate); });
      deerSet.onValueChanged.AddListener(delegate { ToggleSetDear(deerSet); });
      deerMutate.onValueChanged.AddListener(delegate { ToggleMutateDear(deerMutate); });
      bearSet.onValueChanged.AddListener(delegate { ToggleSetBear(bearSet); });
      bearMutate.onValueChanged.AddListener(delegate { ToggleMutateBear(bearMutate); });

      SceneManager.activeSceneChanged += OnSceneChanged;
      EnterMainMenu();
    }

    public void EnterMainMenu()
    {
      dynamicSceneMenu.SetActive(false);
      genomeSettingsMenu.SetActive(false);
      mainMenu.SetActive(true);
      settingsButton.SetActive(true);
    }

    public void EnterDynamicSceneMenu()
    {
      mainMenu.SetActive(false);
      settingsButton.SetActive(false);
      dynamicSceneMenu.SetActive(true);
    }

    public void EnterGenomeTypeMenu()
    {
      mainMenu.SetActive(false);
      settingsButton.SetActive(false);
      genomeSettingsMenu.SetActive(true);
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
      RabbitGenome.SetPreset(rabbitPreset, rabbitMutateChance);
      WolfGenome.SetPreset(wolfPreset, wolfMutateChance);
      DeerGenome.SetPreset(deerPreset, deerMutateChance);
      BearGenome.SetPreset(bearPreset, bearMutateChance);

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

        var terrain = Terrain.activeTerrain;

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


    private static void ToggleSet(bool isOn, out Dictionary<GeneType, Preset> preset,
            Dictionary<GeneType, Preset> single, Dictionary<GeneType, Preset> multi)
    {
      preset = isOn ? multi : single;
    }

    private void ToggleSetRabbit(Toggle toggle)
    {
      ToggleSet(toggle.isOn, ref rabbitPreset, RabbitGenome.DefaultSingular, RabbitGenome.DefaultSet);
    }

    private void ToggleSetWolf(Toggle toggle)
    {
      ToggleSet(toggle.isOn, ref wolfPreset, WolfGenome.DefaultSingular, WolfGenome.DefaultSet);
    }

    private void ToggleSetDear(Toggle toggle)
    {
      ToggleSet(toggle.isOn, ref deerPreset, DeerGenome.DefaultSingular, DeerGenome.DefaultSet);
    }

    private void ToggleSetBear(Toggle toggle)
    {
      ToggleSet(toggle.isOn, ref bearPreset, BearGenome.DefaultSingular, BearGenome.DefaultSet);
    }

    private void ToggleMutateRabbit(Toggle toggle)
    {
      ToggleMutate(toggle.isOn, ref rabbitMutateChance);
    }

    private void ToggleMutateWolf(Toggle toggle)
    {
      ToggleMutate(toggle.isOn, ref wolfMutateChance);
    }

    private void ToggleMutateDear(Toggle toggle)
    {
      ToggleMutate(toggle.isOn, ref deerMutateChance);
    }

    private void ToggleMutateBear(Toggle toggle)
    {
      ToggleMutate(toggle.isOn, ref bearMutateChance);
    }

    private void ToggleMutate(bool isOn, ref float chance)
    {
      chance = isOn ? 0.05f : 0f;
    }
  }
}
