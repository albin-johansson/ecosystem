using System;
using System.Collections;
using System.Collections.Generic;
using Genetics;
using UnityEngine;

public class HungerGene
{
    private double hrMax;
    private double hrMin;
    private double hungerRate;
    private double htMax;
    private double htMin;
    private double hungerThreshold;

    public HungerGene()
    {
        hrMax = 2;
        hrMin = 0.5;
        hungerRate = GeneUtil.getValidVar(1, hrMax, hrMin);

        htMax = 10;
        htMin = 0;
        hungerThreshold = GeneUtil.getValidVar(1, htMax, htMin);
    }

    public HungerGene(double rate, double threshold)
    {
        hrMax = 2;
        hrMin = 0.5;
        this.hungerRate = GeneUtil.getValidVar(rate, hrMax, hrMin);

        htMax = 10;
        htMin = 0;
        this.hungerThreshold = GeneUtil.getValidVar(threshold, htMax, htMin);
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