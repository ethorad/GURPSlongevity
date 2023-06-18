using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GurpsLongevity
{
    internal class Transition
    {
        internal double Weight;
        internal StatBlock Stats;

        internal Transition(double Weight, StatBlock Stats)
        {
            this.Weight = Weight;
            this.Stats = Stats;
        }
    }
}
