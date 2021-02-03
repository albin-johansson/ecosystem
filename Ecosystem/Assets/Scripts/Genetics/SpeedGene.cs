using UnityEngine;

public class SpeedGene
{
    private double factor;

    public SpeedGene()
    {
        factor = 1;
    }

    public SpeedGene(double speedFactor)
    {
        this.factor = speedFactor;
    }

    public double getSpeedFactor()
    {
        return factor;
    }
}