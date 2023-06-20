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
        private Dictionary<string, double> longevityRollsValues;

        internal LongevityTable()
        {
            longevityRollsValues = new Dictionary<string, double>();
        }

        private void AddLongevityRolls(StatBlock stats, double longevity)
        {
            string key = GetSortedStatBlock(stats).ToString();
            if (!longevityRollsValues.ContainsKey(key))
            {
                longevityRollsValues.Add(key, longevity);
            }
        }

        public double GetLongevity(CharacterSheet character)
        {
            double rolls = GetLongevityRolls(character.Stats);
            return character.Traits.AgeFromRolls(rolls);
        }

        private double GetLongevityRolls(StatBlock stats)
        {
            StatBlock sortedBlock = GetSortedStatBlock(stats);

            // if any of the stats are zero, then we are dead
            // want a value of 0, however as we're adding 1 for the die roll
            // need to return -1 to cancel that out and have death occurr immediately
            if (sortedBlock.IQ <= 0 || sortedBlock.HT <= 0)
                return -1;

            string key = sortedBlock.ToString();
            if (longevityRollsValues.ContainsKey(key))
                return longevityRollsValues[key];
            else
            {
                double ans = CalculateLongevityRolls(sortedBlock);
                AddLongevityRolls(sortedBlock, ans);
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
                STprob = start.GetProbability(rST);
                if (STprob == 0)
                    continue; // i.e. can't get to this state, so move to next

                foreach (eRollResult rDX in Enum.GetValues(typeof(eRollResult)))
                {
                    DXprob = start.GetProbability(rDX);
                    if (DXprob == 0)
                        continue;

                    foreach (eRollResult rIQ in Enum.GetValues(typeof(eRollResult)))
                    {
                        IQprob = start.GetProbability(rIQ);
                        if (IQprob == 0)
                            continue;

                        foreach (eRollResult rHT in Enum.GetValues(typeof(eRollResult)))
                        {
                            HTprob = start.GetProbability(rHT);
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

        private StatBlock GetSortedStatBlock(StatBlock stats)
        {
            int maxStat = 0;
            int midStat = 0;
            int minStat = 0;
            if (stats.ST >= stats.DX && stats.ST >= stats.IQ)
            {
                // ST is highest
                maxStat = stats.ST;
                midStat = Math.Max(stats.DX, stats.IQ);
                minStat = Math.Min(stats.DX, stats.IQ);
            }
            else if (stats.DX >= stats.ST && stats.DX >= stats.IQ)
            {
                // DX is highest
                maxStat = stats.DX;
                midStat = Math.Max(stats.ST, stats.IQ);
                minStat = Math.Min(stats.ST, stats.IQ);
            }
            else // IQ must be highest
            {
                maxStat = stats.IQ;
                midStat = Math.Max(stats.ST, stats.DX);
                minStat = Math.Min(stats.ST, stats.DX);
            }

            return new StatBlock(maxStat, midStat, minStat, stats.HT, stats.Longevity, stats.TL, stats.Fitness);
        }

    }
}
