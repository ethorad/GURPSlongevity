using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GurpsLongevity
{
    internal class RNG
    {
        static private Random rng;
        static private double[] cdfProbabilities;

        static RNG()
        {
            rng = new Random();
            cdfProbabilities = new double[19]; // so we have indices 3 to 18 
            CalculateProbabilities();
        }

        static private void CalculateProbabilities()
        {
            // first of all, count how many ways there are to make each number
            for (int d1 = 1; d1 <= 6; d1++)
                for (int d2 = 1; d2 <= 6; d2++)
                    for (int d3 = 1; d3 <= 6; d3++)
                        cdfProbabilities[d1 + d2 + d3]++;

            // then divide by the total number of ways there are
            for (int i = 0; i < cdfProbabilities.Length; i++)
                cdfProbabilities[i] /= (6 * 6 * 6);

            // this gives us the PDF and we want the CDF
            // so replace each with the sum up to that value
            // start from the end so each value we overwrite is not used in future CDF calcs
            for (int i=cdfProbabilities.Length-1; i >= 0;i--)
            {
                // going to calculate the pdf for value i
                for (int j=0;j<i;j++)
                {
                    // increment the probability of rolling i by probability of rolling j
                    cdfProbabilities[i] += cdfProbabilities[j];
                }
            }
        }
        static internal double GetProbability(int roll)
        {
            if (roll >= cdfProbabilities.Length)
                return 1;
            if (roll < 0)
                return 0;
            else
                return cdfProbabilities[roll];
        }

        static internal int Rand(int minVal, int maxVal)
        {
            // return a random number between minVal and maxVal inclusive
            return rng.Next(minVal, maxVal + 1);
        }
        static internal int Roll(int numDice, int sides)
        {
            // roll the specified number of dice with the given sides
            int result = 0;
            for (int i = 0; i < numDice; i++)
                result += Rand(1, sides);
            return result;
        }
        static internal int Roll(int numDice)
        {
            // roll a specified number of 6-sided dice
            return Roll(numDice, 6);
        }
    }
}
