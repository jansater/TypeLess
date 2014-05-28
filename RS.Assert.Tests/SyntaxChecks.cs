using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RS.Assert.Tests
{
    public class DoubleSyntaxChecks
    {
        public DoubleSyntaxChecks()
        {
            double d = 2;
            
            d.If().IsPositive.ThenThrow();
            d.If().IsNegative.ThenThrow();
            d.If().IsZero.ThenThrow();
            d.If().IsSmallerThan(5).ThenThrow();
            d.If().IsLargerThan(5).ThenThrow();
            d.If().Or(2.0, "").ThenThrow(); //this should not be available
            d.If().IsEqualTo(5).ThenThrow();
            d.If().IsFalse(x => true, "is not false").ThenThrow();
            d.If().IsTrue(x => false, "is not true").ThenThrow();
            d.If().IsNotEqualTo(5).ThenThrow();
            d.If().IsNotWithin(3, 5).ThenThrow();

            double d2 = 3;
            double d3 = 4;

            d.If().Or(d2).Or(d3).IsLargerThan(5).ThenThrow();

            (1 > 0).If("Some bool").IsTrue.ThenThrow();
            

        }

    }
}
