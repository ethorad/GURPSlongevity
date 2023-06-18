using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GurpsLongevity
{
    internal class StatBlock
    {
        internal int ST;
        internal int DX;
        internal int IQ;
        internal int HT;
        internal bool Longevity;
        internal int TL;

        public override string ToString()
        {
            string sep = "|";
            string res = "";
            res += ST.ToString() + sep;
            res += DX.ToString() + sep;
            res += IQ.ToString() + sep;
            res += HT.ToString() + sep;
            res += Longevity.ToString() + sep;
            res += TL.ToString();
            return res;
        }

        internal StatBlock(int ST, int DX, int IQ, int HT, bool Longevity, int TL)
        {
            this.ST = ST;
            this.DX = DX;
            this.IQ = IQ;
            this.HT = HT;
            this.Longevity = Longevity;
            this.TL = TL;
        }
        internal StatBlock(StatBlock parent, RollResult STres, RollResult DXres, RollResult IQres, RollResult HTres)
        {
            ST = parent.ST + GetAdjFromResult(STres); if (ST < 0) ST = 0;
            DX = parent.DX + GetAdjFromResult(DXres); if (DX < 0) DX = 0;
            IQ = parent.IQ + GetAdjFromResult(IQres); if (IQ < 0) IQ = 0;
            HT = parent.HT + GetAdjFromResult(HTres); if (HT < 0) HT = 0;
            Longevity = parent.Longevity;
            TL = parent.TL;
        }
        private int GetAdjFromResult(RollResult r)
        {
            switch(r)
            {
                case RollResult.Success:
                    return 0;
                case RollResult.Fail:
                    return -1;
                case RollResult.CriticalFail:
                    return -2;
            }
            return 0;
        }

        internal double GetProbability(RollResult result)
        {
            int modifiedHT = HT + TL;

            // get maximum value to roll for a success
            int successTarget;
            if (Longevity && modifiedHT >= 17)
                successTarget = 17;
            else if (Longevity) // i.e. and modifiedHT < 17
                successTarget = 16;
            else if (modifiedHT >= 17)
                successTarget = 16; // 17 and 18 are always failures
            else if (modifiedHT < 4)
                successTarget = 4; // 3 and 4 are always successes
            else
                successTarget = modifiedHT;

            // get minimum value to roll for a critical failure
            int critTarget;
            if (Longevity)
                critTarget = 20; // no crit fail if you have longevity
            else if (modifiedHT < 7)
                critTarget = modifiedHT + 9; // a roll of 10 more than target is a crit fail
            else if (modifiedHT <= 15)
                critTarget = 16; // 17 or 18 is crit fail with skill up to 15 (note low skills caught in the line above)
            else
                critTarget = 17; // so if skill is 16+, then crit only on an 18

            double successProb = RNG.GetProbability(successTarget);
            double critProb = 1 - RNG.GetProbability(critTarget);
            double failProb = 1 - successProb - critProb;

            switch(result)
            {
                case RollResult.Success:
                    return successProb;
                case RollResult.Fail:
                    return failProb;
                case RollResult.CriticalFail:
                    return critProb;
                default:
                    return 0;
            }
        }
    }
}
