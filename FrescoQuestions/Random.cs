using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrescoQuestions
{
    public static class Random
    {
        private static System.Random Rnd { get; set; } = new System.Random();
        public static int GenerateInt(int first, int last) => Rnd.Next(first, last);
        public static int GenerateInt(int first, int last, int exception)
        {
            int rndVal;
            while ((rndVal = Rnd.Next(first, last)) == exception) ;
            return rndVal;
        }
        public static double GenerateFouble(double first, double last) => Rnd.NextDouble() * (last - first) + first;
        public static double GenerateFouble(double first, double last, double exception)
        {
            double rndVal;
            while ((rndVal = Rnd.NextDouble() * (last - first) + first) == exception) ;
            return rndVal;
        }
    }
}
