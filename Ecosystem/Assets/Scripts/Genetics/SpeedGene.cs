using Genetics;
using UnityEngine;

public class SpeedGene
{
    private double max;
    private double min;
    private double factor;

    public SpeedGene() : this(1)
    {
    }

    public SpeedGene(double speedFactor)
    {
        max = 2;
        min = 1;
        factor = GeneUtil.getValidVar(speedFactor, max, min);
    }

    public double getSpeedFactor()
    {
        return factor;
    }
}