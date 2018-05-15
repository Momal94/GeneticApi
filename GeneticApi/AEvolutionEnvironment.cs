using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GeneticApi
{
    public abstract class AEvolutionEnvironment<T> : IEvolutionEnvironment<T> where T: ICreature
    {
        protected readonly PopulationParameters _populationParameters;
        protected IEnvironmentParameters _environmentParameters;
        protected readonly Random _rand;
        protected AbstractPopulation<T> _gradedPopulation;
        protected AbstractPopulation<T> _solution;
        protected AbstractPopulation<T> _parents;
        protected AbstractPopulation<T> _children;

        protected AEvolutionEnvironment(PopulationParameters populationParameters, IEnvironmentParameters environmentParameters, AbstractPopulation<T> gradedPopulation, AbstractPopulation<T> solution, AbstractPopulation<T> parents, AbstractPopulation<T> children)
        {
            _populationParameters = populationParameters;
            _environmentParameters = environmentParameters;
            _gradedPopulation = gradedPopulation;
            _solution = solution;
            _parents = parents;
            _children = children;
            _rand = new Random(42);
        }

        public virtual T CreateRandomCreature()
        {
            throw new NotImplementedException();
        }

        public virtual AbstractPopulation<T> CreateRandomPopulation()
        {
            throw new NotImplementedException();
        }

        public virtual float AveragePopulationGrade(AbstractPopulation<T> population)
        {
            float total = population.CreaturePopulation.Sum(creature => creature.GetIndividualFitness(_environmentParameters).GetGrade());
            return total / _populationParameters.PopulationCount;
        }

        public virtual List<Tuple<T, IFitness>> GradePopulation(AbstractPopulation<T> population)
        {
            var tupleList = population.CreaturePopulation.Select(x => new Tuple<T, IFitness>(x, x.GetIndividualFitness(_environmentParameters))).ToList();
            return tupleList.OrderByDescending(x => x.Item2.GetGrade()).ToList();
        }

        public virtual bool IsEvolutionOptimal(IFitness fitness)
        {
            throw new NotImplementedException();
        }

        public float ComputeAverageGrade(List<Tuple<T,IFitness>> rawGradedPopulation)
        {
            float averageGrade = 0;
            foreach (var tuple in rawGradedPopulation)
            {
                averageGrade += tuple.Item2.GetGrade();
                _gradedPopulation.CreaturePopulation.Add(tuple.Item1);

                if (IsEvolutionOptimal(tuple.Item2))
                {
                    _solution.CreaturePopulation.Add(tuple.Item1);
                }

                averageGrade /= _populationParameters.PopulationCount;
            }
            return averageGrade;
        }

        public virtual AbstractPopulation<T> MutatePopulation()
        {
            throw new NotImplementedException();
        }

        public virtual AbstractPopulation<T> CrossOverPopulation()
        {
            throw new NotImplementedException();
        }

        public virtual List<T> FindTopCreatures(List<Tuple<T, IFitness>> rawGradedPopulation)
        {
            return rawGradedPopulation.Select(x => x.Item1).ToList().Take(_populationParameters.GradedIndividualRetainCount).ToList();
        }

        public Tuple<AbstractPopulation<T>, float, AbstractPopulation<T>> EvolvePopulation(AbstractPopulation<T> population)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var rawGradedPopulation = GradePopulation(population);
            _gradedPopulation.CreaturePopulation.Clear();
            float averageGrade = 0;

            averageGrade = ComputeAverageGrade(rawGradedPopulation);

            if (_solution.CreaturePopulation.Count > 0)
            {
                return new Tuple<AbstractPopulation<T>, float, AbstractPopulation<T>>(population, averageGrade, _solution);
            }
            _parents.CreaturePopulation.Clear();
            //TOP Creature
            _parents.CreaturePopulation = FindTopCreatures(rawGradedPopulation);

            //Genetic diversity 
            for (int i = _populationParameters.GradedIndividualRetainCount; i < _gradedPopulation.CreaturePopulation.Count; ++i)
            {
                if (_rand.Next(1, 100) < _populationParameters.ChanceRetainNonGraded)
                {
                    var creature = _gradedPopulation.CreaturePopulation.ToArray()[i];
                    _parents.CreaturePopulation.Add(creature);
                }
            }

            //Mutation
            _parents = MutatePopulation();

            //Crossover parents
            _children = CrossOverPopulation();

            _parents.CreaturePopulation.AddRange(_children.CreaturePopulation);
            return new Tuple<AbstractPopulation<T>, float, AbstractPopulation<T>>(_parents, averageGrade, _solution);
        }
    }
}
