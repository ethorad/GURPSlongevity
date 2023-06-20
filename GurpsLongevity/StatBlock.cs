using System;
using System.Collections;
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
        internal eFitness Fitness;

        private int modifiedHT { get { return HT + TL - 3 + (int)Fitness; } }
        internal int SuccessTarget
        {
            get
            {
                // get maximum value to roll for a success
                if (Longevity && modifiedHT >= 17)
                    return 17;
                else if (Longevity) // i.e. and modifiedHT < 17
                    return 16;
                else if (modifiedHT >= 17)
                    return 16; // 17 and 18 are always failures
                else if (modifiedHT < 4)
                    return 4; // 3 and 4 are always successes
                else
                    return modifiedHT;
            }
        }
        internal int CriticalTarget
        {
            get
            {
                // get maximum value to roll to avoid a critical 
                if (Longevity)
                    return int.MaxValue; // no crit fail if you have longevity
                else if (modifiedHT < 7)
                    return modifiedHT + 9; // a roll of 10 more than target is a crit fail
                else if (modifiedHT <= 15)
                    return 16; // 17 or 18 is crit fail with skill up to 15 (note low skills caught in the line above)
                else
                    return 17; // so if skill is 16+, then crit only on an 18
            }
        }

        public override string ToString()
        {
            string sep = "|";
            string res = "";
            res += ST.ToString() + sep;
            res += DX.ToString() + sep;
            res += IQ.ToString() + sep;
            res += HT.ToString() + sep;
            res += Longevity.ToString() + sep;
            res += modifiedHT.ToString();
            return res;
        }

        internal StatBlock(CharacterSheet cs)
        {
            this.ST = cs.ST;
            this.DX = cs.DX;
            this.IQ = cs.IQ;
            this.HT = cs.HT;
            this.Longevity = cs.Longevity;
            this.TL = cs.TL;
            this.Fitness = cs.Fitness;
        }
        internal StatBlock(StatBlock parent, eRollResult STres, eRollResult DXres, eRollResult IQres, eRollResult HTres)
        {
            ST = parent.ST + GetAdjFromResult(STres); if (ST < 0) ST = 0;
            DX = parent.DX + GetAdjFromResult(DXres); if (DX < 0) DX = 0;
            IQ = parent.IQ + GetAdjFromResult(IQres); if (IQ < 0) IQ = 0;
            HT = parent.HT + GetAdjFromResult(HTres); if (HT < 0) HT = 0;
            Longevity = parent.Longevity;
            TL = parent.TL;
            Fitness = parent.Fitness;
        }
        private int GetAdjFromResult(eRollResult r)
        {
            switch(r)
            {
                case eRollResult.Success:
                    return 0;
                case eRollResult.Fail:
                    return -1;
                case eRollResult.CriticalFail:
                    return -2;
            }
            return 0;
        }

        internal void Sort()
        {
            int maxStat = 0;
            int midStat = 0;
            int minStat = 0;

            if (ST >= DX && ST >= IQ)
            {
                // ST is highest
                maxStat = ST;
                midStat = Math.Max(DX, IQ);
                minStat = Math.Min(DX, IQ);
            }
            else if (DX >= ST && DX >= IQ)
            {
                // DX is highest
                maxStat = DX;
                midStat = Math.Max(ST, IQ);
                minStat = Math.Min(ST, IQ);
            }
            else // IQ must be highest
            {
                maxStat = IQ;
                midStat = Math.Max(ST, DX);
                minStat = Math.Min(ST, DX);
            }

            ST = maxStat;
            DX = midStat;
            IQ = minStat;
        }

    }
}
