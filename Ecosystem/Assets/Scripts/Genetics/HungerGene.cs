using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HungerGene
{
    private double hungerRate;
    private double hungerThreshold;

    public HungerGene()
    {
        hungerRate = 1;
        hungerThreshold = 1;
    }

    public HungerGene(double rate, double threshold)
    {
        this.hungerRate = rate;
        this.hungerThreshold = threshold;
    }

    public double getRate()
    {
        return hungerRate;
    }

    public double getThreshold()
    {
        return hungerThreshold;
    }
}