using System.Collections.Generic;

namespace Ecosystem.Genes
{
  public sealed class RabbitGenome : AbstractGenome
  {
    /*
    private static Gene HungerRate; // = new Gene(1, 0.5f, 10);
    private static Gene HungerThreshold; // = new Gene(5, 0, 10);
    private static Gene ThirstRate; // = new Gene(1, 0.5f, 10);
    private static Gene ThirstThreshold; //= new Gene(5, 0, 10);
    private static Gene Vision; // = new Gene(25, 1, 50);
    private static Gene SpeedFactor; // = new Gene(1.5f, 1, 2);
    private static Gene SizeFactor; // = new Gene(0.5f, 0.1f, 1);
    private static Gene DesirabilityFactor; // = new Gene(1, 1, 10);
    private static Gene GestationPeriod; // = new Gene(10, 10, 120);
    private static Gene SexualMaturityTime; // = new Gene(10, 10, 150);
    */
    private static GenomePreset _preset;

    static RabbitGenome()
    {
      _preset.HR = (0.5f, 10);
      _preset.HT = (0, 110);
      _preset.TR = (0.5f, 10);
      _preset.TT = (0, 10);
      _preset.V = (1, 50);
      _preset.Sp = (1, 2);
      _preset.Si = (0.1f, 1);
      _preset.DF = (1, 10);
      _preset.GP = (10, 120);
      _preset.SM = (10, 150);
      _preset.Hrs = new float[] {1, 0.5f, 10};
      _preset.Hts = new float[] {5, 0, 110};
      _preset.Trs = new float[] {1, 0.5f, 10};
      _preset.Tts = new float[] {5, 0, 10};
      _preset.Vs = new float[] {25, 1, 50};
      _preset.Sps = new float[] {1.5f, 1, 2};
      _preset.Sis = new float[] {0.5f, 0.1f, 1};
      _preset.DFs = new float[] {5, 1, 10};
      _preset.GPs = new float[] {65, 10, 120};
      _preset.SMs = new float[] {80, 10, 150};
    }

    public static Dictionary<GeneType, Gene> DefaultGenes;

    private static Dictionary<GeneType, Gene> GenerateNewDictionary()
    {
      Gene HungerRate = GeneUtil.NewGeneFromList(_preset.HR.min, _preset.HR.max, _preset.Hrs);
      Gene HungerThreshold = GeneUtil.NewGeneFromList(_preset.HT.min, _preset.HT.max, _preset.Hts);
      Gene ThirstRate = GeneUtil.NewGeneFromList(_preset.TR.min, _preset.TR.max, _preset.Trs);
      Gene ThirstThreshold = GeneUtil.NewGeneFromList(_preset.TT.min, _preset.TT.max, _preset.Tts);
      Gene Vision = GeneUtil.NewGeneFromList(_preset.V.min, _preset.V.max, _preset.Vs);
      Gene SpeedFactor = GeneUtil.NewGeneFromList(_preset.Sp.min, _preset.Sp.max, _preset.Sps);
      Gene SizeFactor = GeneUtil.NewGeneFromList(_preset.Si.min, _preset.Si.max, _preset.Sis);
      Gene DesirabilityFactor = GeneUtil.NewGeneFromList(_preset.DF.min, _preset.DF.max, _preset.DFs);
      Gene GestationPeriod = GeneUtil.NewGeneFromList(_preset.GP.min, _preset.GP.max, _preset.GPs);
      Gene SexualMaturityTime = GeneUtil.NewGeneFromList(_preset.SM.min, _preset.SM.max, _preset.SMs);

      return DefaultGenes = new Dictionary<GeneType, Gene>
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
        {GeneType.SexualMaturityTime, SexualMaturityTime},
      };
    }

    protected override void Initialize()
    {
      Data = GenomeData.Create(GenerateNewDictionary());
    }
  }
}