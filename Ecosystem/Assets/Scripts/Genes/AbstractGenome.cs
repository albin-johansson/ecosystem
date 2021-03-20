using System.Collections.Generic;
using UnityEngine;

namespace Ecosystem.Genes
{
  public abstract class AbstractGenome : MonoBehaviour, IGenome
  {
    internal GenomeData Data;
    //internal float chance;

    internal static Dictionary<GeneType, Preset> _defaultPresets = new Dictionary<GeneType, Preset>()
    {
      //TODO: change to real values.
      {GeneType.HungerRate, new Preset(0, 1, new[] {0f, 1f})},
      {GeneType.HungerThreshold, new Preset(0, 1, new[] {0f, 1f})},
      {GeneType.ThirstRate, new Preset(0, 1, new[] {0f, 1f})},
      {GeneType.ThirstThreshold, new Preset(0, 1, new[] {0f, 1f})},
      {GeneType.Vision, new Preset(0, 1, new[] {0f, 1f})},
      {GeneType.SpeedFactor, new Preset(0, 1, new[] {0f, 1f})},
      {GeneType.SizeFactor, new Preset(0, 1, new[] {0f, 1f})},
      {GeneType.DesirabilityScore, new Preset(0, 1, new[] {0f, 1f})},
      {GeneType.GestationPeriod, new Preset(0, 1, new[] {0f, 1f})},
      {GeneType.SexualMaturityTime, new Preset(0, 1, new[] {0f, 1f})}
    };

    private void Awake()
    {
      Initialize();
    }


    protected Dictionary<GeneType, Gene> CreateGenes()
    {
      return CreateGenes(_defaultPresets);
    }

    protected Dictionary<GeneType, Gene> CreateGenes(Dictionary<GeneType, Preset> presets)
    {
      //Take the genes from the dictionary presets. 
      Gene HungerRate = GeneUtil.NewGeneFromList(presets[GeneType.HungerRate].min,
        presets[GeneType.HungerRate].max, presets[GeneType.HungerRate].values);
      Gene HungerThreshold = GeneUtil.NewGeneFromList(presets[GeneType.HungerThreshold].min,
        presets[GeneType.HungerThreshold].max, presets[GeneType.HungerThreshold].values);
      Gene ThirstRate = GeneUtil.NewGeneFromList(presets[GeneType.ThirstRate].min,
        presets[GeneType.ThirstRate].max, presets[GeneType.ThirstRate].values);
      Gene ThirstThreshold = GeneUtil.NewGeneFromList(presets[GeneType.ThirstThreshold].min,
        presets[GeneType.ThirstThreshold].max, presets[GeneType.ThirstThreshold].values);
      Gene Vision = GeneUtil.NewGeneFromList(presets[GeneType.Vision].min, presets[GeneType.Vision].max,
        presets[GeneType.Vision].values);
      Gene SpeedFactor = GeneUtil.NewGeneFromList(presets[GeneType.SpeedFactor].min,
        presets[GeneType.SpeedFactor].max, presets[GeneType.SpeedFactor].values);
      Gene SizeFactor = GeneUtil.NewGeneFromList(presets[GeneType.SizeFactor].min,
        presets[GeneType.SizeFactor].max, presets[GeneType.SizeFactor].values);
      Gene DesirabilityFactor = GeneUtil.NewGeneFromList(presets[GeneType.DesirabilityScore].min,
        presets[GeneType.DesirabilityScore].max, presets[GeneType.DesirabilityScore].values);
      Gene GestationPeriod = GeneUtil.NewGeneFromList(presets[GeneType.GestationPeriod].min,
        presets[GeneType.GestationPeriod].max, presets[GeneType.GestationPeriod].values);
      Gene SexualMaturityTime = GeneUtil.NewGeneFromList(presets[GeneType.SexualMaturityTime].min,
        presets[GeneType.SexualMaturityTime].max, presets[GeneType.SexualMaturityTime].values);

      //Create the genes dictionary
      Dictionary<GeneType, Gene> genes = new Dictionary<GeneType, Gene>
      {
        {GeneType.HungerRate, HungerRate},
        {GeneType.HungerThreshold, HungerThreshold},
        {GeneType.ThirstRate, ThirstRate},
        {GeneType.ThirstThreshold, ThirstThreshold},
        {GeneType.Vision, Vision},
        {GeneType.SpeedFactor, SpeedFactor},
        {GeneType.SizeFactor, SizeFactor},
        {GeneType.DesirabilityScore, DesirabilityFactor},
        {GeneType.GestationPeriod, GestationPeriod},
        {GeneType.SexualMaturityTime, SexualMaturityTime}
      };
      return genes;
    }

    protected abstract void Initialize();
    protected abstract Dictionary<GeneType, Preset> GetPresets();
    protected abstract float GetClassMutateChance();

    public GenomeData CreateData()
    {
      return GenomeData.Create(CreateGenes(GetPresets()), GetClassMutateChance());
    }

    public void Initialize(IGenome first, IGenome second)
    {
      Data = Genomes.Merge(first, second);
    }

    public bool IsMale => Data.IsMale;

    public float Speed => GetHungerRate().Value *
                          GetSpeedFactor().Value *
                          GetSizeFactor().ValueAsDecimal();

    public float Metabolism => GetHungerRate().Value * GetSizeFactor().Value;

    public float Attractiveness => GetDesirabilityScore().Value;

    public Gene GetHungerRate() => Data.HungerRate;

    public Gene GetHungerThreshold() => Data.HungerThreshold;

    public Gene GetThirstRate() => Data.ThirstRate;

    public Gene GetThirstThreshold() => Data.ThirstThreshold;

    public Gene GetVision() => Data.Vision;

    public Gene GetSpeedFactor() => Data.SpeedFactor;

    public Gene GetSizeFactor() => Data.SizeFactor;

    public Gene GetDesirabilityScore() => Data.DesirabilityScore;

    public Gene GetGestationPeriod() => Data.GestationPeriod;

    public Gene GetSexualMaturityTime() => Data.SexualMaturityTime;

    public GenomeData GetGenes() => Data;
  }
}