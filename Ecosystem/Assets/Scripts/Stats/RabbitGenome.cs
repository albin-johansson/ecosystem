public sealed class RabbitGenome : Genome
{
  private static double defaultMutateChance;
  private static GeneList defaultGenelist;

  /// <summary>
  /// Default values, constant ranges. 
  /// </summary>
  static RabbitGenome()
  {
    defaultMutateChance = 0.05;
    var hungerRateGene = new Gene(1, 10, 0.5);
    var hungerThresholdGene = new Gene(5, 10, 0);
    var thirstRateGene = new Gene(1, 10, 0.5);
    var thirstThresholdGene = new Gene(5, 10, 0);
    var visionGene = new Gene(25, 50, 1);
    var speedGene = new Gene(1.5, 2, 1);
    var sizeGene = new Gene(0.5, 1, 0.1);
    var desireGene = new Gene(1, 10, 1);
    defaultGenelist = new GeneList(hungerRateGene, hungerThresholdGene,
      thirstRateGene, thirstThresholdGene, visionGene, speedGene, sizeGene,
      desireGene);
  }

  public RabbitGenome() : base(defaultGenelist.Copy(), defaultMutateChance)
  {
  }
}