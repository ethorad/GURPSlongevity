using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GurpsLongevity
{
    class CharacterSheet
    {

        // traits
        private int st;
        internal int ST { get { return st; } set { st = value; } }
        private int dx;
        internal int DX { get { return dx; } set { dx = value; } }
        private int iq;
        internal int IQ { get { return iq; } set { iq = value; } }
        private int ht;
        internal int HT { get { return ht; } set { ht = value; } }
        private bool longevity;
        internal bool Longevity { get { return longevity; } set { longevity = value; } }
        private int tl;
        internal int TL { get { return tl; } set { tl = value; } }
        private eFitness fitness;
        internal eFitness Fitness { get { return fitness; } set { fitness = value; } }
        private double extendedLifespan;
        public double ExtendedLifespan
        {
            get { return extendedLifespan; }
            set
            {
                if (value < 0)
                    ShortLifespan = -value;
                else
                    extendedLifespan = value;
                if (extendedLifespan != 0)
                    ShortLifespan = 0;
            }
        }
        private double shortLifespan;
        public double ShortLifespan
        {
            get { return shortLifespan; }
            set
            {
                if (value < 0)
                    ExtendedLifespan = -value;
                else
                    shortLifespan = value;
                if (shortLifespan != 0)
                    ExtendedLifespan = 0;
            }
        }

    }
}
