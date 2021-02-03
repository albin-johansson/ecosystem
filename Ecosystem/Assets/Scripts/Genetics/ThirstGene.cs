using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirstGene
{
    private double thirstRate;
    private double thirstThreshold;

    public ThirstGene()
    {
        thirstRate = 1;
        thirstThreshold = 1;
    }

    public ThirstGene(double rate, double threshold)
    {
        this.thirstRate = rate;
        this.thirstThreshold = threshold;
    }

    public double getRate()
    {
        return thirstRate;
    }

    public double getThreshold()
    {
        return thirstThreshold;
    }
}