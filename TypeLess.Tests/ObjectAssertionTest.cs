using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;
using TypeLess;

namespace TypeLess.Tests
{
    public class ObjectAssertionTest
    {

        class A
        {
            public int B { get; set; }
            public int C { get; set; }
        }

        ObjectAssertion CanDelete(A a)
        {
            return ObjectAssertion.New(
                a.If().IsTrue(x => x.B == 1, "B must not be 1"),
                a.If().IsTrue(x => x.C == 2, "C must not be 2")
            );
        }


        [Fact]
        public void WhenRulesAreValidThenReturnTrue()
        {
            var a = new A() { B = 1, C = 2 }; //values are correct according to the rules of CanDelete

            Assert.True(CanDelete(a).True);
            int errorCount = 0;
            Assert.Equal(0, errorCount);

            a.B = 2;
            Assert.False(CanDelete(a).True);

            a.C = 3;
            Assert.False(CanDelete(a).True);
        }

        [Fact]
        public void WhenRulesAreInvalidThenReturnFalse()
        {
            var a = new A() { B = 1, C = 2 };
            Assert.False(CanDelete(a).False);

            //Check current error message
            Assert.Equal("B must not be 1 and C must not be 2", CanDelete(a).ToString());

            a.B = 2;
            Assert.True(CanDelete(a).False);
            Assert.Equal("C must not be 2", CanDelete(a).ToString());

            a.C = 3;
            Assert.True(CanDelete(a).False);
        }

        ObjectAssertion CanUpdate(A a)
        {
            //this is not testing to see if something is invalid ... its testing validity
            return ObjectAssertion.New(
                a.If().EvalPositive.IsTrue(x => x.B == 1, "B must be 1"),
                a.If().EvalPositive.IsTrue(x => x.C == 2, "C must be 2")
            );
        }

        [Fact]
        public void RulesNotValidCanThrow()
        {
            var a = new A() { B = 2, C = 2 };

            var ex = Assert.Throws<ArgumentException>(() =>
            {
                CanUpdate(a).ThrowIfFalse<ArgumentException>();
            });

            var msg = ex.Message;

            Assert.Equal("B must be 1", msg);

            a.B = 1;

            CanUpdate(a).ThrowIfFalse<ArgumentNullException>();


            a.C = 3;
            ex = Assert.Throws<ArgumentException>(() =>
            {
                CanUpdate(a).ThrowIfFalse<ArgumentException>();
            });

            msg = ex.Message;

            Assert.Equal("C must be 2", msg);

            a.B = 2;
            ex = Assert.Throws<ArgumentException>(() =>
            {
                CanUpdate(a).ThrowIfFalse<ArgumentException>();
            });

            msg = ex.Message;

            Assert.Equal("B must be 1 and C must be 2", msg);

        }

        [Fact]
        public void RulesValidCanNotThrow()
        {

            var a = new A() { B = 1, C = 2 };
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                CanDelete(a).ThrowIfTrue<ArgumentException>();
            });

            var msg = ex.Message;

            Assert.Equal("B must not be 1 and C must not be 2", msg);


            a.B = 2;

            CanDelete(a).ThrowIfTrue<ArgumentNullException>();

        }
    }
}
