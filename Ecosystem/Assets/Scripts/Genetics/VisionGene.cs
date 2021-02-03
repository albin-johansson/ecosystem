using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionGene
{
    private double radius;

    public VisionGene()
    {
        radius = 1;
    }

    public VisionGene(double radius)
    {
        this.radius = radius;
    }

    public double getRadius()
    {
        return radius;
    }
}