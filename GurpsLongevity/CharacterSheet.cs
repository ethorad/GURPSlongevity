using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GurpsLongevity
{
    class CharacterSheet
    {
        internal StatBlock Stats;
        internal AgingTraits Traits;

        public CharacterSheet(StatBlock Stats, AgingTraits Traits)
        {
            this.Stats = Stats;
            this.Traits = Traits;
        }
        public CharacterSheet()
        {
            Stats = new StatBlock();
            Traits = new AgingTraits();
        }
    }
}
