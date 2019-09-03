using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TypeLess;
using TypeLess.Extensions.Sweden;
using TypeLess.DataTypes;
using TypeLess.Helpers;
using Xunit;
using TypeLess.Extensions.Banks;

namespace TypeLess.Tests
{
    public class ThenThenReturnChecks
    {
        public ThenThenReturnChecks()
        {
            object s = null;
            s.If().IsNotNull.Then(x => x = null).ThenReturn(4);
        }
    }

    public class ComparableSyntaxChecks<T> where T : IComparable<T>
    {
        public ComparableSyntaxChecks()
        {
            Range<int> i = new Range<int>();
        }
    }

    public class ClassSyntaxChecks
    {
        public ClassSyntaxChecks()
        {
            ListSyntaxChecks o = new ListSyntaxChecks();
            DoubleSyntaxChecks o2 = new DoubleSyntaxChecks();
            DictionaryChecks o3 = new DictionaryChecks();

            o.If().IsFalse(x => true, "is not false").ThenThrow();
            o.If().IsTrue(x => false, "is not true").ThenThrow();
            o.If().Or(o2).Or(o3).IsNull.ThenThrow(); //this should not be possible
            o.If().IsEqualTo(o3).ThenThrow();

            o.If().IsNull.ThenThrow().Otherwise(x =>
            {

            });
        }
    }

    public class TraverserChecks
    {

        [Fact]
        public void TraverseReturnsCorrectSum()
        {
            "aaaaRSBG".If().IsNotValidSwiftCode().ThenThrow();

            var s = new { Sum = 0 };

            var total = EnumerableTraverser.Traverse(new int[] { 1, 2, 4 }, (i, current, next, state) =>
           {
               state = new { Sum = state.Sum + current };
               return state;
           }, s);

            Assert.True(total.Sum == 7);
        }
    }

    public class DoubleSyntaxChecks
    {
        public DoubleSyntaxChecks()
        {
            double d = 2;
            double e = 3;

            d.If().IsPositive.Or(d.If().IsPositive);
            d.If().IsPositive.ThenThrow();
            d.If().IsNegative.ThenThrow();
            d.If().IsZero.ThenThrow();
            d.If().IsLessThan(5).ThenThrow();
            d.If().IsGreaterThan(5).ThenThrow();
            d.If().IsEqualTo(5).ThenThrow();
            d.If().IsFalse(x => true, "is not false").ThenThrow();
            d.If().IsTrue(x => false, "is not true").ThenThrow();
            d.If().IsNotEqualTo(5).ThenThrow();
            d.If().IsNotWithin(3, 5).ThenThrow();
            d.If().IsWithin(3, 5).ThenThrow();
            d.If().Or(e).IsGreaterThan(5).ThenThrow(); //this should not be possible
        }
    }

    public class ListSyntaxChecks
    {
        public ListSyntaxChecks()
        {
            List<int> someList = new List<int>();
            List<int> someOtherList = new List<int>();

            someList.If().ContainsLessThan(5).ThenThrow();
            someList.If().ContainsMoreThan(5).ThenThrow();
            someList.If().IsEmpty.ThenThrow();
            someList.If().IsEqualTo(someList).ThenThrow(); //dont think we should have this
            someList.If().IsFalse(x => 1 == 0, "").ThenThrow();
            someList.If().IsTrue(x => 1 == 1, "").ThenThrow();
            someList.If().Or(someOtherList).ContainsLessThan(5).ThenThrow();

            IList<int> someIList = new List<int>();
            IList<int> someOtherIList = new List<int>();

            someIList.If().ContainsLessThan(5).ThenThrow();
            someIList.If().ContainsMoreThan(5).ThenThrow();
            someIList.If().IsEmpty.ThenThrow();
            someIList.If().IsEqualTo(someList).ThenThrow(); //dont think we should have this
            someIList.If().IsFalse(x => 1 == 0, "").ThenThrow();
            someIList.If().IsTrue(x => 1 == 1, "").ThenThrow();
            someIList.If().Or(someOtherList).ContainsLessThan(5).ThenThrow();

            int[] arr = new int[] { 1 };
            arr.If().ContainsLessThan(5).ThenThrow();
            arr.If().ContainsMoreThan(5).ThenThrow();
            arr.If().IsEmpty.ThenThrow();
            arr.If().IsEqualTo(someList).ThenThrow(); //dont think we should have this
            arr.If().IsFalse(x => 1 == 0, "").ThenThrow();
            arr.If().IsTrue(x => 1 == 1, "").ThenThrow();
            arr.If().Or(someOtherList).ContainsLessThan(5).ThenThrow();

            Queue<int> q = new Queue<int>();
            q.If().ContainsLessThan(5).ThenThrow();
            q.If().ContainsMoreThan(5).ThenThrow();
            q.If().IsEmpty.ThenThrow();
            q.If().IsEqualTo(someList).ThenThrow(); //dont think we should have this
            q.If().IsFalse(x => 1 == 0, "").ThenThrow();
            q.If().IsTrue(x => 1 == 1, "").ThenThrow();
            q.If().Or(someOtherList).ContainsLessThan(5).ThenThrow();

            Stack<int> s = new Stack<int>();
            s.If().ContainsLessThan(5).ThenThrow();
            s.If().ContainsMoreThan(5).ThenThrow();
            s.If().IsEmpty.ThenThrow();
            s.If().IsEqualTo(someList).ThenThrow(); //dont think we should have this
            s.If().IsFalse(x => 1 == 0, "").ThenThrow();
            s.If().IsTrue(x => 1 == 1, "").ThenThrow();
            s.If().Or(someOtherList).ContainsLessThan(5).ThenThrow();

            IEnumerable<int> i = new List<int>();
            i.If().ContainsLessThan(5).ThenThrow();
        }

    }

    public class AndChecks
    {

        public AndChecks()
        {
            var arr1 = new int[] { 1, 2, 3, 4 };
            var arr2 = new int[] { 1, 2 };

            arr1.If("Arr1").Contains(new int[] { 1, 2 }).And(arr2, "Arr2").Contains(new int[] { 1, 2 }).ThenThrow<ArgumentException>();
        }
    }

    public class DictionaryChecks
    {
        public DictionaryChecks()
        {
            Dictionary<string, int> someDict = new Dictionary<string, int>();

            someDict.If().IsEmpty.ThenThrow();
            someDict.If().IsEqualTo(someDict).ThenThrow(); //dont think we should have this
            someDict.If().IsFalse(x => 1 == 0, "").ThenThrow();
            someDict.If().IsTrue(x => 1 == 1, "").ThenThrow();

            IDictionary<string, int> someIDict = new Dictionary<string, int>();

            someIDict.If().IsEmpty.ThenThrow();
            someIDict.If().IsEqualTo(someDict).ThenThrow(); //dont think we should have this
            someIDict.If().IsFalse(x => 1 == 0, "").ThenThrow();
            someIDict.If().IsTrue(x => 1 == 1, "").ThenThrow();
        }

    }

    public class StringChecks
    {
        public StringChecks()
        {
            string s = "";

            s.If().IsEmpty.ThenThrow();
            s.If().IsFalse(x => 1 == 0, "").ThenThrow();
            s.If().IsTrue(x => 1 == 1, "").ThenThrow();
            s.If().DoesNotContainAlphaChars.ThenThrow();
            s.If().IsNotValidPersonalNumber().ThenThrow();
        }
    }

    public class NullableChecks
    {
        public NullableChecks()
        {
            int? i = null;

            i.If().IsNull.ThenThrow();
            i.If().IsNotNull.ThenThrow();

        }
    }

    public class DateTimeChecks
    {
        public DateTimeChecks()
        {
            DateTime dt = new DateTime();

            dt.If().NotSameYearAs(DateTime.UtcNow).ThenThrow();

        }
    }

    public class TimeSpanChecks
    {
        public TimeSpanChecks()
        {
            TimeSpan dt = new TimeSpan();
            dt.If().IsShorterThan(TimeSpan.FromHours(5)).ThenThrow();
        }
    }

    public class ThenOtherwiseTests
    {
        public ThenOtherwiseTests()
        {
            // How can we make a decision to call and return a value .. or another value without a execute function... 
            // We can't evaluate until the last function call ... so how do we know when the last function has been added ... 
            //object o = null;
            //o.If().IsNotNull.ThenReturn(x => "").Otherwise(x => x. "");
        }
    }
}
