public sealed class Gene
{
  public double Max { get; private set; }
  public double Min { get; private set; }
  public double Value { get; private set; }

  public Gene(double value, double max, double min)
  {
    Max = max;
    Min = min;
    Value = GeneUtil.GetValidVar(value, max, min);
  }

  /// <summary>
  /// Generate a mutated Gene in the given range. 
  /// </summary>
  /// <param name="max"></param>
  /// <param name="min"></param>
  public Gene(double max, double min)
  {
    Max = max;
    Min = min;
    GeneUtil.MutatedInRange(max, min);
  }

  public Gene Copy()
  {
    return new Gene(this.Value, this.Max, this.Min);
  }
}