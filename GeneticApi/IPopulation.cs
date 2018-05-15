using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticApi
{
    public interface IPopulation
    {
        void AddCreature(ICreature creature);
        List<ICreature> GetCreaturesList();
        void SetCreaturesList(List<ICreature> creatures);
    }
}
