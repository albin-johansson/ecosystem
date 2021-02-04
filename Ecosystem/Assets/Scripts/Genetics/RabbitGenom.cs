using System.Collections;
using System.Collections.Generic;
using Genetics;
using UnityEngine;

public class RabbitGenom : Genom
{
    public RabbitGenom()
    {
        hungerGene = new HungerGene();
        thirstGene = new ThirstGene();
        visionGene = new VisionGene();
        speedGene = new SpeedGene();
        sizeGene = new SizeGene();
    }
}