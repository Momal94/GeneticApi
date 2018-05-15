using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticApi
{
    internal interface IEvolutionEnvironment<T> where T: ICreature
    {
        T CreateRandomCreature();
        AbstractPopulation<T> CreateRandomPopulation();
        float AveragePopulationGrade(AbstractPopulation<T> population);
        List<Tuple<T, IFitness>> GradePopulation(AbstractPopulation<T> population);
        Tuple<AbstractPopulation<T>, float, AbstractPopulation<T>> EvolvePopulation(AbstractPopulation<T> population);
    }
}