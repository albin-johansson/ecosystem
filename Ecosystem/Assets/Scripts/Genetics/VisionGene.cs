using System.Collections;
using System.Collections.Generic;
using Genetics;
using UnityEngine;

public class VisionGene
{
    private double max;
    private double min;
    private double radius;

    public VisionGene() : this(5)
    {
    }

    public VisionGene(double r)
    {
        max = 10;
        min = 1;
        radius = GeneUtil.getValidVar(r, max, min);
    }

    public double getRadius()
    {
        return radius;
    }
}