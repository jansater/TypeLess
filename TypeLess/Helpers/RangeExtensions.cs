using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeLess.DataTypes;

namespace TypeLess.Helpers
{

    public static class RangeExtensions
    {

        /// <summary>
        /// Get the length between min and max value of range
        /// </summary>
        /// <param name="range">The range.</param>
        /// <returns></returns>
        public static TimeSpan Length(this Range<DateTime> range)
        {
            return range.Max - range.Min;
        }

        /// <summary>
        /// Get the length between min and max value of range
        /// </summary>
        /// <param name="range">The range.</param>
        /// <returns></returns>
        public static double Length(this Range<double> range)
        {
            return range.Max - range.Min;
        }

        /// <summary>
        /// Get the length between min and max value of range
        /// </summary>
        /// <param name="range">The range.</param>
        /// <returns></returns>
        public static int Length(this Range<int> range)
        {
            return range.Max - range.Min;
        }

        /// <summary>
        /// Create a range using the min and max values of an enumerable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="range">The range.</param>
        /// <returns></returns>
        public static Range<T> AsRange<T>(this IEnumerable<T> range) where T : IComparable<T>
        {
            return new Range<T>(range.Min(), range.Max());
        }

        /// <summary>
        /// Convert a range to an enumerable with n values using a distribution function. See SpreadFunctions for pre-defined distributions
        /// </summary>
        /// <param name="range">The source range</param>
        /// <param name="nNumbersToSpread">The number of values to return</param>
        /// <param name="distribution">The distribution function.</param>
        /// <returns></returns>
        public static IEnumerable<double> AsEnumerable(this Range<double> range, int nNumbersToSpread, Func<double, double> distribution)
        {
            return SpreadFunctions.Spread(range.Min, range.Max, nNumbersToSpread, distribution);
        }


        /// <summary>
        /// Convert a range to a list of values. The new list will try to fit as many increments as possible into the new list.
        /// </summary>
        /// <param name="range">The range.</param>
        /// <param name="increment">The increment.</param>
        /// <param name="allowValueSmallerThanIncrementBeforeMax">if set to <c>true</c> [allow value smaller than increment before maximum].
        /// Use this when you want to control if the second last value should be added if the length to the last value is less than 1 increment.
        /// </param>
        /// <returns></returns>
        public static List<double> AsEnumerable(this Range<double> range, double increment = 1, bool allowValueSmallerThanIncrementBeforeMax = true)
        {
            List<double> d = new List<double>((int)Math.Ceiling(range.Max - range.Min));
            d.Add(range.Min);

            for (double i = range.Min + increment; (i + (allowValueSmallerThanIncrementBeforeMax ? 0.0 : increment)) < range.Max; i += increment)
            {
                d.Add(i);
            }

            d.Add(range.Max);

            return d;
        }

        /// <summary>
        /// Convert a range to a list of values. The new list will try to fit as many increments as possible into the new list.
        /// </summary>
        /// <param name="range">The range.</param>
        /// <param name="increment">The increment.</param>
        /// <param name="allowValueSmallerThanIncrementBeforeMax">if set to <c>true</c> [allow value smaller than increment before maximum].
        /// Use this when you want to control if the second last value should be added if the length to the last value is less than 1 increment.
        /// </param>
        /// <returns></returns>
        public static List<int> AsEnumerable(this Range<int> range, int increment = 1, bool allowValueSmallerThanIncrementBeforeMax = true)
        {
            List<int> d = new List<int>(range.Max - range.Min);
            d.Add(range.Min);

            for (int i = range.Min + increment; (i + (allowValueSmallerThanIncrementBeforeMax ? 0 : increment)) < range.Max; i += increment)
            {
                d.Add(i);
            }

            d.Add(range.Max);

            return d;
        }

        /// <summary>
        /// Convert a range to a list of values. The new list will try to fit as many increments as possible into the new list.
        /// </summary>
        /// <param name="range">The range.</param>
        /// <param name="increment">The increment.</param>
        /// <param name="allowValueSmallerThanIncrementBeforeMax">if set to <c>true</c> [allow value smaller than increment before maximum].
        /// Use this when you want to control if the second last value should be added if the length to the last value is less than 1 increment.
        /// </param>
        /// <returns></returns>
        public static IEnumerable<DateTime> AsEnumerable(this Range<DateTime> range, TimeSpan increment, bool allowValueSmallerThanIncrementBeforeMax = true)
        {
            var timesIncrementInRange = (double)(range.Max.Ticks - range.Min.Ticks) / increment.Ticks;
            List<DateTime> d = new List<DateTime>((int)Math.Ceiling(timesIncrementInRange));
            d.Add(range.Min);

            for (DateTime i = range.Min + increment; (i + (allowValueSmallerThanIncrementBeforeMax ? TimeSpan.Zero : increment)) < range.Max; i += increment)
            {
                d.Add(i);
            }

            d.Add(range.Max);

            return d;
        }

    }

}
