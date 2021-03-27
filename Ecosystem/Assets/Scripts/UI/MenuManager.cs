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
    [SerializeField] private GameObject genomeTypeMenu;
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
    [SerializeField] private Toggle dearSet;
    [SerializeField] private Toggle dearMutate;
    [SerializeField] private Toggle bearSet;
    [SerializeField] private Toggle bearMutate;

    private Dictionary<GeneType, Preset> rabbitPreset;
    private float rabbitMutateChance = 0.05f;
    private Dictionary<GeneType, Preset> wolfPreset;
    private float wolfMutateChance = 0.05f;
    private Dictionary<GeneType, Preset> dearPreset;
    private float dearMutateChance = 0.05f;
    private Dictionary<GeneType, Preset> bearPreset;
    private float bearMutateChance = 0.05f;

    private void Start()
    {
      //Set up listeners for Genome settings. 
      rabbitPreset = RabbitGenome.defaultSet;
      //TODO: give own defaults
      wolfPreset = RabbitGenome.defaultSet;
      dearPreset = RabbitGenome.defaultSet;
      bearPreset = RabbitGenome.defaultSet;

      rabbitSet.onValueChanged.AddListener(delegate { ToggleSetRabbit(rabbitSet); });
      rabbitMutate.onValueChanged.AddListener(delegate { ToggleMutateRabbit(rabbitMutate); });
      wolfSet.onValueChanged.AddListener(delegate { ToggleSetWolf(wolfSet); });
      wolfMutate.onValueChanged.AddListener(delegate { ToggleMutateWolf(wolfMutate); });
      dearSet.onValueChanged.AddListener(delegate { ToggleSetDear(dearSet); });
      dearMutate.onValueChanged.AddListener(delegate { ToggleMutateDear(dearMutate); });
      bearSet.onValueChanged.AddListener(delegate { ToggleSetBear(bearSet); });
      bearMutate.onValueChanged.AddListener(delegate { ToggleMutateBear(bearMutate); });
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
      //TODO: remember to remove/clean
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
      //TODO: set all presets. 
      RabbitGenome.SetPreset(rabbitPreset, rabbitMutateChance);

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

    /**
     * -------------------------------------------------------
     * presets below
     * -------------------------------------------------------
     */

    //TODO: check if "ref" is needed.
    private void ToggleSet(bool isOn, ref Dictionary<GeneType, Preset> preset, Dictionary<GeneType, Preset> single,
      Dictionary<GeneType, Preset> multi)
    {
      Debug.Log("Animal set: " + isOn);
      preset = isOn ? multi : single;
    }

    public void ToggleSetRabbit(Toggle toggle)
    {
      ToggleSet(toggle.isOn, ref rabbitPreset, RabbitGenome.defaultSingular, RabbitGenome.defaultSet);
    }


    //TODO: give own defaults!
    public void ToggleSetBear(Toggle toggle)
    {
      ToggleSet(toggle.isOn, ref bearPreset, RabbitGenome.defaultSingular, RabbitGenome.defaultSet);
    }

    //TODO: give own defaults!
    public void ToggleSetWolf(Toggle toggle)
    {
      ToggleSet(toggle.isOn, ref wolfPreset, RabbitGenome.defaultSingular, RabbitGenome.defaultSet);
    }

    //TODO: give own defaults!
    public void ToggleSetDear(Toggle toggle)
    {
      ToggleSet(toggle.isOn, ref dearPreset, RabbitGenome.defaultSingular, RabbitGenome.defaultSet);
    }

    /**
     * -------------------------------------------------------
     * mutates below
     * -------------------------------------------------------
     */
    public void ToggleMutateRabbit(Toggle toggle)
    {
      ToggleMutate(toggle.isOn, ref rabbitMutateChance);
    }

    public void ToggleMutateWolf(Toggle toggle)
    {
      ToggleMutate(toggle.isOn, ref wolfMutateChance);
    }

    public void ToggleMutateDear(Toggle toggle)
    {
      ToggleMutate(toggle.isOn, ref dearMutateChance);
    }

    public void ToggleMutateBear(Toggle toggle)
    {
      ToggleMutate(toggle.isOn, ref bearMutateChance);
    }

    private void ToggleMutate(bool isOn, ref float chance)
    {
      Debug.Log("animal mutate: " + isOn);
      chance = isOn ? 0.05f : 0f;
    }
  }
}