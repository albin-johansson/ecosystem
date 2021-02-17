public sealed class Gene
{
  private double Max { get; }
  private double Min { get; }
  public double Value { get; }

  public Gene(double value, double min, double max)
  {
    Max = max;
    Min = min;
    Value = GeneUtil.GetValidVar(value, min, max);
  }

  public Gene(Gene other) : this(other.Value, other.Min, other.Max)
  {
  }

  //Returns value as a number in [0,1]. 
  public double ValueAsDecimal()
  {
    return (Value - Min) / (Max - Min);
  }

  // Returns a new mutated gene based on the current Genes range. 
  public Gene Mutate()
  {
    return new Gene(GeneUtil.MutatedInRange(Min, Max), Min, Max);
  }
}