﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticApi
{
    public interface ICreature
    {
        IFitness GetIndividualFitness(IEnvironmentParameters environmentParameters);
    }
}
