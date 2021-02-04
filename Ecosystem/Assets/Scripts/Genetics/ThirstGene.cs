using System.Collections;
using System.Collections.Generic;
using Genetics;
using UnityEngine;

public class ThirstGene
{
    private double trMax;
    private double trMin;
    private double thirstRate;
    private double ttMax;
    private double ttMin;
    private double thirstThreshold;

    public ThirstGene() : this(1, 1)
    {
    }

    public ThirstGene(double rate, double threshold)
    {
        trMax = 2;
        trMin = 0.5;
        ttMax = 10;
        ttMin = 0;
        thirstRate = GeneUtil.getValidVar(rate, trMax, trMin);
        thirstThreshold = GeneUtil.getValidVar(threshold, ttMax, ttMin);
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