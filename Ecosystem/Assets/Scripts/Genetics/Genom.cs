using System.Collections.Generic;
using Genetics;
using NUnit.Framework;
using UnityEngine;

public class Genom : MonoBehaviour
{
    protected double mutateChance;

    protected GeneList genes;

    public Genom(Genom g1, Genom g2)
    {
        List<Gene> list = new List<Gene>();
        for (int i = 0; i < g1.genes.GetSize(); i++)
        {
            Gene g;
            if (GeneUtil.RandomWithChance(g1.mutateChance))
            {
                //mutate
                g = new Gene(g1.genes.GetGene(i).GetMax(), g1.genes.GetGene(i).GetMin());
            }
            else
            {
                //pick parent
                if (GeneUtil.RandomWithChance(50))
                {
                    g = g1.genes.GetGene(i);
                }
                else
                {
                    g = g2.genes.GetGene(i);
                }
            }

            list.Add(g);
        }
    }

    protected Genom()
    {
        mutateChance = 0.05;
        Gene hungerRateGene = new Gene(1, 10, 0.5);
        Gene hungerThresholdGene = new Gene(5, 10, 0);
        Gene thirstRateGene = new Gene(1, 10, 0.5);
        Gene thirstThresholdGene = new Gene(5, 10, 0);
        Gene visionGene = new Gene(25, 50, 1);
        Gene speedGene = new Gene(1.5, 2, 1);
        Gene sizeGene = new Gene(0.5, 1, 0.1);
        Gene desireGene = new Gene(1, 10, 1);
        genes = new GeneList(hungerRateGene, hungerThresholdGene,
            thirstRateGene, thirstThresholdGene, visionGene, speedGene, sizeGene,
            desireGene);
    }

    public Genom(GeneList geneList, double percentage)
    {
        genes = geneList;
        mutateChance = percentage;
    }

    public double GetSpeed()
    {
        double speed = genes.GetGene(GeneList.HungerRate).GetValue() * genes.GetGene(GeneList.SpeedFactor).GetValue() *
                       ((genes.GetGene(GeneList.SizeFactor).GetMax() - genes.GetGene(GeneList.SizeFactor).GetMin()) -
                        genes.GetGene(GeneList.SizeFactor).GetValue());
        return speed;
    }

    public double GetVisionRange()
    {
        return genes.GetGene(GeneList.Vision).GetValue();
    }

    public double GetHungerRate()
    {
        return genes.GetGene(GeneList.HungerRate).GetValue() * genes.GetGene(GeneList.SizeFactor).GetValue();
    }

    public double GetHungerThreshold()
    {
        return genes.GetGene(GeneList.HungerThreshold).GetValue();
    }

    public double GetThirstRate()
    {
        return genes.GetGene(GeneList.ThirstRate).GetValue();
    }

    public double GetThirstThreshold()
    {
        return genes.GetGene(GeneList.ThirstThreshold).GetValue();
    }

    public double GetDesirability()
    {
        return genes.GetGene(GeneList.DesireFactor).GetValue();
    }
}