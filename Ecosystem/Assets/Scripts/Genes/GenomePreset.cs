namespace Ecosystem.Genes
{
  public sealed class GenomePreset
  {
    /*
    Gene HungerRate = new Gene(1, 0.5f, 10);
    Gene HungerThreshold = new Gene(5, 0, 10);
    Gene ThirstRate = new Gene(1, 0.5f, 10);
    Gene ThirstThreshold = new Gene(5, 0, 10);
    Gene Vision = new Gene(25, 1, 50);
    Gene SpeedFactor = new Gene(1.5f, 1, 2);
    Gene SizeFactor = new Gene(0.5f, 0.1f, 1);
    Gene DesirabilityFactor = new Gene(1, 1, 10);
    Gene GestationPeriod = new Gene(12, 10, 120);
    Gene SexualMaturityTime = new Gene(20, 10, 150);
    */

    public (float min, float max) HR { get; set; }
    public (float min, float max) HT { get; set; }
    public (float min, float max) TR { get; set; }
    public (float min, float max) TT { get; set; }
    public (float min, float max) V { get; set; }
    public (float min, float max) Sp { get; set; }
    public (float min, float max) Si { get; set; }
    public (float min, float max) DF { get; set; }
    public (float min, float max) GP { get; set; }
    public (float min, float max) SM { get; set; }
    public float[] Hrs { get; set; }
    public float[] Hts { get; set; }
    public float[] Trs { get; set; }
    public float[] Tts { get; set; }
    public float[] Vs { get; set; }
    public float[] Sps { get; set; }
    public float[] Sis { get; set; }
    public float[] DFs { get; set; }
    public float[] GPs { get; set; }
    public float[] SMs { get; set; }
  }
}