using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticApi
{
    public abstract class AbstractPopulation<T> where T: ICreature
    {
        public List<T> CreaturePopulation { get; set; }

        public AbstractPopulation()
        {
            CreaturePopulation = new List<T>();
        }
    }
}
