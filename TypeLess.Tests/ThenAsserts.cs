using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace TypeLess.Tests
{
    public class ThenAsserts
    {

        [Fact]
        public void ThenCallsActionOnTrue()
        {
            double d = 5;
            bool val = false;
            d.If().IsEqualTo(5).Then(x => val = true);
            Xunit.Assert.True(val);
        }

        [Fact]
        public void ThenCallsFuncOnTrue()
        {
            double d = 5;
            var res = d.If().IsEqualTo(5).ThenReturn(x => "Somestring");
            Xunit.Assert.Equal("Somestring", res);
        }

        class A
        {
            public int SomeProp { get; set; }
        }

        [Fact]
        public void ThrowWorksOnMultipleIfs()
        {
            A a = new A() { SomeProp = 6 };
            Assert.Throws<ArgumentNullException>(() =>
            {
                a.If().IsNotNull.ThenReturn(x => x.SomeProp).If("Some prop in a").IsLargerThan(5).ThenThrow();
            });

        }

        [Fact]
        public void WhenRegexMatchThenThrow()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                string s = "some 2 string";
                s.If().Match("\\d").ThenThrow("s must contain digits");
            });

            Assert.DoesNotThrow(() =>
            {
                string s = "some 2 string";
                s.If().Match("anotherString").ThenThrow();
            });

        }

        [Fact]
        public void WhenRegexDoesNotMatchThenThrow()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                string s = "some 2 string";
                s.If().DoesNotMatch("another string").ThenThrow();
            });

            Assert.DoesNotThrow(() =>
            {
                string s = "some string";
                s.If().DoesNotMatch("some").ThenThrow();
            });

        }

        [Fact]
        public void WhenRegexMatchReturnValIsCorrectThrow()
        {
            string s = "some 2 string";
            var res =  s.If().Match("\\d").ThenReturnResultOf("$0");
            Xunit.Assert.Equal("2", res);
        }

        [Fact]
        public void WhenRegexMatchReturnValIsCorrectForNameThrow()
        {
            string s = "some 2 string";
            var res =  s.If().Match("(?<val>\\d)").ThenReturnResultOf("${val}");
            Xunit.Assert.Equal("2", res);
        }

    }
}
