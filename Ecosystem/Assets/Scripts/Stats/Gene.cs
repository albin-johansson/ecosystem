namespace Stats
{
  public class Gene
  {
    private double max;
    private double min;
    private double factor;

    public Gene(double f, double max, double min)
    {
      this.max = max;
      this.min = min;
      factor = GeneUtil.GetValidVar(f, max, min);
    }

    public double GetValue()
    {
      return factor;
    }

    public double GetMax()
    {
      return max;
    }

    public double GetMin()
    {
      return min;
    }


    /// <summary>
    /// Generate a mutated Gene in the given range. 
    /// </summary>
    /// <param name="max"></param>
    /// <param name="min"></param>
    public Gene(double max, double min)
    {
      this.max = max;
      this.min = min;
      GeneUtil.MutatedInRange(max, min);
    }

    public Gene Copy()
    {
      return new Gene(this.factor, this.max, this.min);
    }
  }
}