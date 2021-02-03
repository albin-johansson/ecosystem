using UnityEngine;

public abstract class Genom
{
    protected HungerGene hungerGene;
    protected ThirstGene thirstGene;
    protected VisionGene visionGene;
    protected SpeedGene speedGene;


    public double getSpeed()
    {
        double speed = hungerGene.getRate() * speedGene.getSpeedFactor();
        return speed;
    }

    public double getVisionRange()
    {
        return visionGene.getRadius();
    }
    public double getHungerRate()
    {
        return hungerGene.getRate();
    }

    public double getHungerThreshold()
    {
        return hungerGene.getThreshold();
    }

    public double getThirstRate()
    {
        return thirstGene.getRate();
    }

    public double getThirstThreshold()
    {
        return thirstGene.getThreshold();
    }
}