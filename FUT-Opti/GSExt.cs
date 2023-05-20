using GeneticSharp;

namespace FUT_Opti
{
    public class EliteReinsertion : ReinsertionBase
    {
        public EliteReinsertion() : base(false, true)
        {
        }

        protected override IList<IChromosome> PerformSelectChromosomes(IPopulation population, IList<IChromosome> offspring, IList<IChromosome> parents)
        {
            int diff = population.MinSize - offspring.Count;

            if (diff > 0)
            {
                int elitePick = diff / 2;
                IList<IChromosome> bestParents = parents.OrderByDescending(p => p.Fitness).Take(elitePick).ToList();

                offspring = offspring.Concat(bestParents).ToList();

                for (int i = 0; i < diff - elitePick; i++)
                {
                    offspring.Add(new OptimisationChromosome());
                }
            }

            return offspring;
        }
    }

    public class WheelPlusNewSelection : SelectionBase
    {
        private const double TopChromosomesPercentage = 0.1;
        private const double RouletteSelectionPercentage = 0.5;

        private List<IChromosome> _previousGenerationChromosomes;

        public WheelPlusNewSelection() : base(2)
        {
            _previousGenerationChromosomes = new List<IChromosome>();
        }

        protected override IList<IChromosome> PerformSelectChromosomes(int number, Generation generation)
        {
            _previousGenerationChromosomes.AddRange(generation.Chromosomes);

            var ordered = _previousGenerationChromosomes.OrderByDescending(c => c.Fitness);
            var result = ordered.Take((int)(number * TopChromosomesPercentage));

            var chromosomes = generation.Chromosomes;
            var rouletteWheel = new List<double>();
            var rnd = RandomizationProvider.Current;

            CalculateCumulativePercentFitness(chromosomes, rouletteWheel);

            var rouletteSelection = SelectFromWheel((int)(number * RouletteSelectionPercentage), chromosomes, rouletteWheel, () => rnd.GetDouble());

            return result.Concat(rouletteSelection).ToList();
        }

        protected static IList<IChromosome> SelectFromWheel(int number, IList<IChromosome> chromosomes, IList<double> rouletteWheel, Func<double> getPointer)
        {
            var selected = new List<IChromosome>();

            for (int i = 0; i < number; i++)
            {
                var pointer = getPointer();

                var chromosomeIndex = rouletteWheel
                                        .Select((value, index) => new { Value = value, Index = index })
                                        .FirstOrDefault(r => r.Value >= pointer)?.Index;

                if (chromosomeIndex.HasValue)
                    selected.Add(chromosomes[chromosomeIndex.Value].Clone());
            }

            return selected;
        }

        protected static void CalculateCumulativePercentFitness(IList<IChromosome> chromosomes, IList<double> rouletteWheel)
        {
            double sumFitness = chromosomes.Sum(c => c.Fitness.Value);

            double cumulativePercent = 0.0;

            foreach (var chromosome in chromosomes)
            {
                cumulativePercent += chromosome.Fitness.Value / sumFitness;
                rouletteWheel.Add(cumulativePercent);
            }
        }
    }

    public class OptimisationChromosome : ChromosomeBase
    {
        public static readonly int[] Choices = new int[11];
        public OptimisationChromosome() : base(11)
        {
            for (int i = 0; i < Length; i++)
            {
                ReplaceGene(i, GenerateGene(i));
            }
        }

        public int GetSlot(int x) => (int)GetGene(x).Value;

        public override IChromosome CreateNew() => new OptimisationChromosome();

        public override Gene GenerateGene(int geneIndex) => new Gene(RandomizationProvider.Current.GetInt(0, Choices[geneIndex]));
    }
}
