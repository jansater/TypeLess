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
        public void ItsPossibleToRunThenAndThenThrowException()
        {
            double d = 5;
            bool val = false;
            bool exThrown = false;
            try
            {
                d.If().IsEqualTo(5).Then(x => val = true).ThenThrow<ArgumentException>("Just throwing an exception");
            }
            catch (Exception)
            {
                exThrown = true;
            }
            Xunit.Assert.True(exThrown);
            Xunit.Assert.True(val);
        }

        private void Log(string msg)
        {
        }

        [Fact]
        public void ItsPossibleToLogToAndThenThrowException()
        {
            double d = 5;
            bool exThrown = false;
            try
            {
                d.If("d").IsEqualTo(5).ThenLogTo(Log).ThenThrow<ArgumentException>("Just throwing an exception");
            }
            catch (Exception)
            {
                exThrown = true;
            }
            Xunit.Assert.True(exThrown);
            
        }

        [Fact]
        public void LogIsNotWrittenIfStatementIsFalse()
        {
            string s = "";
            double d = 5;
            d.If("d").IsEqualTo(4).ThenLogTo(x => s = "test").ThenThrow<ArgumentException>("Just throwing an exception");
            Assert.Equal("", s);
        }

        [Fact]
        public void LogIsWrittenIfStatementIsTrue()
        {
            string s = "";
            double d = 5;
            try
            {
                d.If("d").IsEqualTo(5).ThenLogTo(x => s = "test").ThenThrow<ArgumentException>("Just throwing an exception");
            }
            catch (Exception)
            {
                
            }
            
            Assert.Equal("test", s);
        }

        [Fact]
        public void ExceptionIsOnlyThrownAfterThenOnValidStatement()
        {
            double d = 4;
            bool val = false;
            bool exThrown = false;
            try
            {
                d.If().IsEqualTo(5).Then(x => val = true).ThenThrow<ArgumentException>("Just throwing an exception");
            }
            catch (Exception)
            {
                exThrown = true;
            }
            Xunit.Assert.False(exThrown);
            Xunit.Assert.False(val);
        }

        [Fact]
        public void CallTryCatchFinally()
        {
            object o = null;

            bool tryCalled = false;
            bool catchCalled = false;
            bool finallyCalled = false;

            o.If().IsNull.Try(x =>
            {
                tryCalled = true;
                var s = x.ToString();
            }, ex =>
            {
                catchCalled = true;
            }, x =>
            {
                finallyCalled = true;
            });

            Assert.True(tryCalled && catchCalled && finallyCalled);
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
                a.If().IsNotNull.ThenReturn(x => x.SomeProp).If("Some prop in a").IsGreaterThan(5).ThenThrow();
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

           
                string s2 = "some 2 string";
                s2.If().Match("anotherString").ThenThrow();
           

        }

        [Fact]
        public void WhenRegexDoesNotMatchThenThrow()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                string s = "some 2 string";
                s.If().DoesNotMatch("another string").ThenThrow();
            });

            
                var s2 = "some string";
                s2.If().DoesNotMatch("some").ThenThrow();
           

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
