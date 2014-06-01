using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeLess.Helpers
{
    public static class SpreadFunctions
    {

        public static readonly Func<double, double> QuadraticSpread = x => 1 - (1 - x) * (1 - x);
        public static readonly Func<double, double> SineSpread = x => Math.Sin(x * Math.PI / 2);
        public static readonly Func<double, double> LinearSpread = x => 1 / x;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a">The height of the curve</param>
        /// <param name="b">Position of center</param>
        /// <param name="c">Width of bell</param>
        /// <param name="d">offset (normally 0)</param>
        /// <returns></returns>
        public static Func<double, double> GaussianSpread(double a, double b, double c, double d)
        {
            return x => a * Math.Exp(-(Math.Pow(x - b, 2) / (2 * Math.Pow(c, 2)))) + d;
        }

        public static IEnumerable<double> Spread(double min, double max, int count, Func<double, double> distribution)
        {
            double start = min; 
            double scale = max - min; 
            foreach (double offset in Redistribute(count, distribution))
                yield return start + offset * scale; //offset gives values between 0 and 1
        }

        private static IEnumerable<double> Redistribute(int count, Func<double, double> distribution)
        {
            double step = 1.0 / (count - 1); // split n to return into a percentage step
            for (int i = 0; i < count; i++)
                yield return distribution(i * step); 
        }

    }
}
