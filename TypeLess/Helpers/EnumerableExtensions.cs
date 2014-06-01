using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeLess.Helpers
{

    public static class EnumerableExtensions
    {
        /// <summary>
        /// Remove the first and last item of an enumerable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">The items.</param>
        /// <returns></returns>
        public static IEnumerable<T> RemoveFirstLast<T>(this IEnumerable<T> items)
        {
            return items.Skip(1).Take(items.Count() - 2);
        }


        /// <summary>
        /// Get the standard deviation of the items in an enumerable
        /// </summary>
        /// <param name="range">The range.</param>
        /// <returns></returns>
        public static double StandardDeviation(this IEnumerable<double> range)
        {
            var count = (double)range.Count();
            var avg = range.Average();
            return Math.Sqrt(range.Sum(x => Math.Pow(x - avg, 2)) / count);
        }

        /// <summary>
        /// Create a string representation from an enumerable. Uses StringBuilder
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="func">The function.</param>
        /// <param name="onSameLine">if set to <c>true</c> [on same line].</param>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public static string ToString<T>(this IEnumerable<T> source, Func<T, string> func, bool onSameLine = false)
        {
            StringBuilder sb = new StringBuilder(source.Count() * 25);
            foreach (var item in source)
            {
                if (onSameLine)
                {
                    sb.Append(func(item));
                }
                else
                {
                    sb.AppendLine(func(item));
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Scale a range to fit within another range.
        /// </summary>
        /// <param name="range">The range.</param>
        /// <param name="newScaleMin">The new scale minimum.</param>
        /// <param name="newScaleMax">The new scale maximum.</param>
        /// <returns></returns>
        public static IEnumerable<double> Scale(this IEnumerable<double> range, double toScaleMin, double toScaleMax, double fromScaleMin = 0, double fromScaleMax = 100.0)
        {
            double scale = (double)(toScaleMax - toScaleMin) / (fromScaleMax - fromScaleMin);
            return range.Select(x => x * scale).ToList();
        }

        /// <summary>
        /// Fill an enumerable with n values using a distribution function. See SpreadFunctions for pre-defined distributions
        /// </summary>
        /// <param name="range">The source range</param>
        /// <param name="nNumbersToSpread">The number of values to return</param>
        /// <param name="distribution">The distribution function.</param>
        /// <returns></returns>
        public static IEnumerable<double> AsEnumerableRange(this IEnumerable<double> range, int nNumbersToSpread, Func<double, double> distribution)
        {
            return SpreadFunctions.Spread(range.Min(), range.Max(), nNumbersToSpread, distribution);
        }

    }

}
