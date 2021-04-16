using System.Collections.Generic;
using Ecosystem.Genes;
using UnityEngine;
using UnityEngine.UI;

namespace Ecosystem.UI
{
  public class GeneSettingsMenu : MonoBehaviour

  {
    [SerializeField] private Toggle rabbitSet;
    [SerializeField] private Toggle rabbitMutate;
    [SerializeField] private Toggle wolfSet;
    [SerializeField] private Toggle wolfMutate;
    [SerializeField] private Toggle deerSet;
    [SerializeField] private Toggle deerMutate;
    [SerializeField] private Toggle bearSet;
    [SerializeField] private Toggle bearMutate;

    private Dictionary<GeneType, Preset> _rabbitPreset;
    private Dictionary<GeneType, Preset> _wolfPreset;
    private Dictionary<GeneType, Preset> _deerPreset;
    private Dictionary<GeneType, Preset> _bearPreset;
    private float _rabbitMutateChance = 0.05f;
    private float _wolfMutateChance = 0.05f;
    private float _deerMutateChance = 0.05f;
    private float _bearMutateChance = 0.05f;


    private void Start()
    {
      _rabbitPreset = RabbitGenome.DefaultSet;
      _wolfPreset = WolfGenome.DefaultSet;
      _deerPreset = DeerGenome.DefaultSet;
      _bearPreset = BearGenome.DefaultSet;
      rabbitSet.onValueChanged.AddListener(delegate { ToggleSetRabbit(rabbitSet); AssignChanges();});
      rabbitMutate.onValueChanged.AddListener(delegate { ToggleMutateRabbit(rabbitMutate); AssignChanges();});
      wolfSet.onValueChanged.AddListener(delegate { ToggleSetWolf(wolfSet); AssignChanges();});
      wolfMutate.onValueChanged.AddListener(delegate { ToggleMutateWolf(wolfMutate); AssignChanges();});
      deerSet.onValueChanged.AddListener(delegate { ToggleSetDear(deerSet); AssignChanges();});
      deerMutate.onValueChanged.AddListener(delegate { ToggleMutateDear(deerMutate); AssignChanges();});
      bearSet.onValueChanged.AddListener(delegate { ToggleSetBear(bearSet); AssignChanges();});
      bearMutate.onValueChanged.AddListener(delegate { ToggleMutateBear(bearMutate); AssignChanges();});
    }


    /// <summary>
    /// Call when the presets should be assigned for all the animals. 
    /// </summary>
    public void AssignChanges()
    {
      RabbitGenome.SetPreset(_rabbitPreset, _rabbitMutateChance);
      WolfGenome.SetPreset(_wolfPreset, _wolfMutateChance);
      DeerGenome.SetPreset(_deerPreset, _deerMutateChance);
      BearGenome.SetPreset(_bearPreset, _bearMutateChance);
    }


    private static void ToggleSet(bool isOn, out Dictionary<GeneType, Preset> preset,
      Dictionary<GeneType, Preset> single, Dictionary<GeneType, Preset> multi)
    {
      preset = isOn ? multi : single;
    }

    private void ToggleSetRabbit(Toggle toggle)
    {
      ToggleSet(toggle.isOn, out _rabbitPreset, RabbitGenome.DefaultSingular, RabbitGenome.DefaultSet);
    }

    private void ToggleSetWolf(Toggle toggle)
    {
      ToggleSet(toggle.isOn, out _wolfPreset, WolfGenome.DefaultSingular, WolfGenome.DefaultSet);
    }

    private void ToggleSetDear(Toggle toggle)
    {
      ToggleSet(toggle.isOn, out _deerPreset, DeerGenome.DefaultSingular, DeerGenome.DefaultSet);
    }

    private void ToggleSetBear(Toggle toggle)
    {
      ToggleSet(toggle.isOn, out _bearPreset, BearGenome.DefaultSingular, BearGenome.DefaultSet);
    }

    private void ToggleMutateRabbit(Toggle toggle)
    {
      _rabbitMutateChance = GetMutateChance(toggle);
    }

    private void ToggleMutateWolf(Toggle toggle)
    {
      _wolfMutateChance = GetMutateChance(toggle);
    }

    private void ToggleMutateDear(Toggle toggle)
    {
      _deerMutateChance = GetMutateChance(toggle);
    }

    private void ToggleMutateBear(Toggle toggle)
    {
      _bearMutateChance = GetMutateChance(toggle);
    }

    private static float GetMutateChance(bool isOn)
    {
      return isOn ? 0.05f : 0f;
    }
  }
}