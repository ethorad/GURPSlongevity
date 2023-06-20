using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GurpsLongevity
{
    class LongevityTable
    {
        // Constants from rulebook
        private const double baseAgingLevel1 = 50;
        private const double baseAgingLevel2 = 70;
        private const double baseAgingLevel3 = 90;

        private const double agingPerRollLevel1 = 1;
        private const double agingPerRollLevel2 = 0.5;
        private const double agingPerRollLevel3 = 0.25;

        // collection to hold calculated longevity stats
        private Dictionary<string, double> longevityRollsValues;

        internal LongevityTable()
        {
            longevityRollsValues = new Dictionary<string, double>();
        }

        private void AddLongevityRolls(StatBlock stats, double longevity)
        {
            stats.Sort();
            string key = stats.ToString();
            if (!longevityRollsValues.ContainsKey(key))
            {
                longevityRollsValues.Add(key, longevity);
            }
        }

        public double GetLongevity(CharacterSheet character)
        {
            double rolls = GetLongevityRolls(new StatBlock(character));

            double agingLevel1 = baseAgingLevel1 * Math.Pow(2, character.ExtendedLifespan - character.ShortLifespan);
            double agingLevel2 = baseAgingLevel2 * Math.Pow(2, character.ExtendedLifespan - character.ShortLifespan);
            double agingLevel3 =baseAgingLevel3 * Math.Pow(2, character.ExtendedLifespan - character.ShortLifespan);

            double tier1rolls = Math.Min(rolls, (agingLevel2 - agingLevel1) / agingPerRollLevel1);
            rolls -= tier1rolls;

            double tier2rolls = Math.Min(rolls, (agingLevel3 - agingLevel2) / agingPerRollLevel2);
            rolls -= tier2rolls;

            double tier3rolls = rolls;

            return agingLevel1 + tier1rolls * agingPerRollLevel1 + tier2rolls * agingPerRollLevel2 + tier3rolls * agingPerRollLevel3;
        }

        private double GetLongevityRolls(StatBlock stats)
        {
            stats.Sort();

            // if any of the stats are zero, then we are dead
            // want a value of 0, however as we're adding 1 for the die roll
            // need to return -1 to cancel that out and have death occurr immediately
            if (stats.IQ <= 0 || stats.HT <= 0)
                return -1;

            string key = stats.ToString();
            if (longevityRollsValues.ContainsKey(key))
                return longevityRollsValues[key];
            else
            {
                double ans = CalculateLongevityRolls(stats);
                AddLongevityRolls(stats, ans);
                return ans;
            }
        }

        private double CalculateLongevityRolls(StatBlock start)
        {
            List<Transition> children = GetChildren(start);
            string startStr = start.ToString();
            double numerator = 0;
            double denominator = 0;
            foreach (Transition child in children)
            {
                if (child.Stats.ToString() == startStr)
                    denominator = 1 - child.Weight;
                else
                    numerator += child.Weight * (1 + GetLongevityRolls(child.Stats));
            }
            return numerator / denominator;

        }

        private List<Transition> GetChildren(StatBlock start)
        {
            List<Transition> children = new List<Transition>();
            double STprob = 0;
            double DXprob = 0;
            double IQprob = 0;
            double HTprob = 0;

            foreach (eRollResult rST in Enum.GetValues(typeof(eRollResult)))
            {
                STprob = GetProbability(rST, start);
                if (STprob == 0)
                    continue; // i.e. can't get to this state, so move to next

                foreach (eRollResult rDX in Enum.GetValues(typeof(eRollResult)))
                {
                    DXprob = GetProbability(rDX, start);
                    if (DXprob == 0)
                        continue;

                    foreach (eRollResult rIQ in Enum.GetValues(typeof(eRollResult)))
                    {
                        IQprob = GetProbability(rIQ, start);
                        if (IQprob == 0)
                            continue;

                        foreach (eRollResult rHT in Enum.GetValues(typeof(eRollResult)))
                        {
                            HTprob = GetProbability(rHT, start);
                            if (HTprob == 0)
                                continue;

                            double prob = STprob * DXprob * IQprob * HTprob;
                            StatBlock child = new StatBlock(start, rST, rDX, rIQ, rHT);
                            children.Add(new Transition(prob, child));

                        } // HT
                    } // IQ
                } // DX
            } // ST
            return children;
        }

        private double GetProbability(eRollResult result, StatBlock stats)
        {
            double successProb = RNG.GetProbability(stats.SuccessTarget);
            double critProb = 1 - RNG.GetProbability(stats.CriticalTarget);
            double failProb = 1 - successProb - critProb;

            switch (result)
            {
                case eRollResult.Success:
                    return successProb;
                case eRollResult.Fail:
                    return failProb;
                case eRollResult.CriticalFail:
                    return critProb;
                default:
                    return 0;
            }
        }

    }
}
