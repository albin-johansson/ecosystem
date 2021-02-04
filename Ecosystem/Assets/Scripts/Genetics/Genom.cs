using Genetics;
using UnityEditor;
using UnityEngine;

public class Genom : MonoBehaviour
{
    protected HungerGene hungerGene;
    protected ThirstGene thirstGene;
    protected VisionGene visionGene;
    protected SpeedGene speedGene;
    protected SizeGene sizeGene;

    public static Genom childGenom(Genom g1, Genom g2)
    {
        return new Genom(new HungerGene(), new ThirstGene(), new VisionGene(), new SpeedGene(), new SizeGene());
    }

    protected Genom()
    {
    }

    protected Genom(HungerGene hg, ThirstGene tg, VisionGene vg, SpeedGene sg, SizeGene sig)
    {
        hungerGene = hg;
        thirstGene = tg;
        visionGene = vg;
        speedGene = sg;
        sizeGene = sig;
    }

    public double getSpeed()
    {
        double speed = hungerGene.getRate() * speedGene.getSpeedFactor() * (1.1 - sizeGene.getSize());
        return speed;
    }

    public double getVisionRange()
    {
        return visionGene.getRadius();
    }

    public double getHungerRate()
    {
        return hungerGene.getRate() * sizeGene.getSize();
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