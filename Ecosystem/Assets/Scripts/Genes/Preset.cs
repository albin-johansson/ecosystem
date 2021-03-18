namespace Ecosystem.Genes
{
  public struct Preset
  {
    public float min;
    public float max;
    public float[] values;

    public Preset(float min, float max, float[] values)
    {
      this.min = min;
      this.max = max;
      this.values = values;
    }
  }
}