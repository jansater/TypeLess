using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeLess;

namespace TypeLess.DataTypes
{
    public class Range
    {
        public static bool Within(double expected, double value, double delta)
        {
            if (double.IsNaN(expected) && double.IsNaN(value))
                return true;
            return value >= expected - delta && value <= expected + delta;
        }
        public static bool Within(double? expected, double? value, double delta)
        {
            if (expected.HasValue && value.HasValue)
                return Range.Within(expected.Value, value.Value, delta);
            else
                return expected.HasValue == value.HasValue;
        }
    }

    public class Range<T> where T : IComparable<T>
    {
        public T Min { get; set; }
        public T Max { get; set; }

        public Range()
        {

        }

        public Range(T min, T max)
        {
            if (max.CompareTo(min) >= 0) {
                throw new ArgumentException("Min must be smaller than max");
            }

            this.Min = min;
            this.Max = max;
        }

        public bool IsWithinRange(T value, bool checkLowerRange = true, bool checkUpperRange = true)
        {
            return (!checkLowerRange || value.CompareTo(Min) >= 0) && (!checkUpperRange || value.CompareTo(Max) <= 0);
        }

       
    }
}
