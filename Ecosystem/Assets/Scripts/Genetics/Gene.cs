using Genetics;

public class Gene
{
    protected double max;
    protected double min;
    protected double factor;

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


    public Gene(double max, double min)
    {
        this.max = max;
        this.min = min;
        GeneUtil.MutatedInRange(max, min);
    }
}