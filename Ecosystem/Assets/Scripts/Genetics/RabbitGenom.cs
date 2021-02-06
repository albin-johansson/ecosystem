using Genetics;

public class RabbitGenom : Genom
{
    public RabbitGenom()
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
}