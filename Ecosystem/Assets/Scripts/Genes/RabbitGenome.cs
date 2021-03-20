using System.Collections.Generic;

namespace Ecosystem.Genes
{
  public sealed class RabbitGenome : AbstractGenome
  {
    protected override void Initialize()
    {
      Data = GenomeData.Create(CreateGenes(), chance);
      //Data = GenomeData.Create(DefaultGenes);
    }
  }
}