namespace Ecosystem.Genes
{
  /// <summary>
  ///   An interface for all animal genomes.
  /// </summary>
  public interface IGenome
  {
    /// <summary>
    ///   Initializes the genome based on two parent genomes.
    /// </summary>
    /// <param name="first">the first parent.</param>
    /// <param name="second">the second parent.</param>
    void Initialize(IGenome first, IGenome second);

    /// <summary>
    ///   Returns the speed value. This value depends on the hunger rate, speed factor and size
    ///   genes.
    /// </summary>
    /// <returns>the speed value, based on the associated genes.</returns>
    float WalkingSpeed { get; }

    /// <summary>
    ///   Returns the metabolism value. This value depends on the hunger rate, and size genes.
    /// </summary>
    /// <returns>the metabolism value, based on the associated genes.</returns>
    float Metabolism { get; }

    /// <summary>
    ///   Indicates whether or not the specimen is male or not.
    /// </summary>
    /// <returns><c>true</c> if the specimen is male; <c>false</c> otherwise.</returns>
    bool IsMale { get; }

    Gene GetHungerRate();

    Gene GetHungerThreshold();

    Gene GetThirstRate();

    Gene GetThirstThreshold();

    Gene GetVision();

    Gene GetSpeed();

    Gene GetGestationPeriod();

    Gene GetSexualMaturityTime();

    GenomeData GetGenes();
  }
}