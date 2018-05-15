using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticApi
{
    public struct PopulationParameters
    {
        public float ChanceToMutate;
        public float GradedRetainPercent;
        public float ChanceRetainNonGraded;
        public int PopulationCount;
        public int GenerationCountMax;
        public int GradedIndividualRetainCount;
    }
}
