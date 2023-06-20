using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GurpsLongevity
{
    class AgingTraits
    {
        private const double baseAgingLevel1 = 50;
        private const double baseAgingLevel2 = 70;
        private const double baseAgingLevel3 = 90;

        private const double agingPerRollLevel1 = 1;
        private const double agingPerRollLevel2 = 0.5;
        private const double agingPerRollLevel3 = 0.25;

        private double extendedLifespan;
        private double shortLifespan;

        private double agingLevel1 { get { return baseAgingLevel1 * Math.Pow(2, extendedLifespan - shortLifespan); } }
        private double agingLevel2 { get { return baseAgingLevel2 * Math.Pow(2, extendedLifespan - shortLifespan); } }
        private double agingLevel3 { get { return baseAgingLevel3 * Math.Pow(2, extendedLifespan - shortLifespan); } }

        public AgingTraits()
        {
            extendedLifespan = 0;
            shortLifespan = 0;
        }

        public double AgeFromRolls(double rolls)
        {
            double tier1rolls = Math.Min(rolls, (agingLevel2 - agingLevel1) / agingPerRollLevel1);
            rolls -= tier1rolls;

            double tier2rolls = Math.Min(rolls, (agingLevel3 - agingLevel2) / agingPerRollLevel2);
            rolls -= tier2rolls;

            double tier3rolls = rolls;

            return agingLevel1 + tier1rolls * agingPerRollLevel1 + tier2rolls * agingPerRollLevel2 + tier3rolls * agingPerRollLevel3;
        }

    }
}
