namespace Ecosystem.Genes
{
  public struct Preset
  {
    public float min { get; private set; }
    public float max { get; private set; }
    public float[] values { get; private set; }

    public Preset(float min, float max, float[] values)
    {
      this.min = min;
      this.max = max;
      this.values = values;
    }
  }
}