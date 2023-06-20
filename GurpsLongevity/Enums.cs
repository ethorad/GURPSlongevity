using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GurpsLongevity
{
    internal enum eRollResult
    {
        Success,
        Fail,
        CriticalFail
    }

    internal enum eFitness
    {
        VeryUnfit = -2,
        Unfit = -1,
        Normal = 0,
        Fit = 1,
        VeryFit = 2
    }
}
