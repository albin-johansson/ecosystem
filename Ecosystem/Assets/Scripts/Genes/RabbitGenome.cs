using System.Collections.Generic;

namespace Ecosystem.Genes
{
  public sealed class RabbitGenome : AbstractGenome
  {
    public static Dictionary<GeneType, Gene> DefaultGenes = GenerateNewDictionary();

    private static Dictionary<GeneType, Gene> GenerateNewDictionary()
    {
      return null;
    }

    protected override void Initialize()
    {
      Data = GenomeData.Create(CreateGenes());
      //Data = GenomeData.Create(DefaultGenes);
    }
  }
}