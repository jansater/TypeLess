﻿using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;
using TypeLess.Extensions.Sweden;

namespace TypeLess.Tests
{
    public class Asserts
    {
        [Fact]
        public void WhenNullThenDontThrowIfJustToString()
        {

            Xunit.Assert.DoesNotThrow(() =>
            {
                string s = null;
                var msg = s.If("s").IsNull.ToString();

                Xunit.Assert.True(msg.Contains("required"));
            });

        }

        [Fact]
        public void WhenNullThenThrow()
        {

            Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                string s = null;
                s.If("s").IsNull.ThenThrow();
            });

            Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                int? s = null;
                s.If().IsNull.ThenThrow();
            });

            Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                string s = null;
                s.If().IsNull.ThenThrow();
            });

            Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                IEnumerable<int> s = null;
                s.If().IsNull.ThenThrow();
            });

            Xunit.Assert.DoesNotThrow(() =>
            {
                int? s = 1;
                s.If().IsNull.ThenThrow();
            });

        }

        [Fact]
        public void WhenNullThenThrowWithSpecificException()
        {

            Xunit.Assert.Throws<ArgumentException>(() =>
            {
                string s = null;
                s.If("s").IsNull.ThenThrow<ArgumentException>();
            });

        }


        [Fact]
        public void WhenNullThenDontCheckFurther()
        {
            Xunit.Assert.DoesNotThrow(() =>
            {
                string s = null;
                Xunit.Assert.Equal(1, s.If("s").IsNull.IsEmpty.ErrorCount);
            });

        }

        [Fact]
        public void WhenStopSetThenDontCheckFurther()
        {
            string s = "";
            Xunit.Assert.Equal(1, s.If("s")
                .IsEmpty
                .StopIfNotValid
                .IsNotValidEmail.ErrorCount);
        }

        [Fact]
        public void WhenEmptyStringThenThrow()
        {
            Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                string s = "";
                s.If().IsEmpty.ThenThrow();
            });

            Xunit.Assert.DoesNotThrow(() =>
            {
                string s = "d";
                s.If().IsEmpty.ThenThrow();
            });
        }

        [Fact]
        public void WhenIsTrueThenThrow()
        {
            Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                string s = "";
                s.If().IsTrue(x => true, "").ThenThrow();
            });

            Xunit.Assert.DoesNotThrow(() =>
            {
                string s = "d";
                s.If().IsTrue(x => false, "").ThenThrow();

            });
        }

        [Fact]
        public void WhenIsFalseThenThrow()
        {
            Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                string s = "";
                s.If().IsFalse(x => false, "").ThenThrow();
            });

            Xunit.Assert.DoesNotThrow(() =>
            {
                string s = "d";
                s.If().IsFalse(x => true, "").ThenThrow();

            });
        }

        [Fact]
        public void WhenIsEmptyThenThrow()
        {
            Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                var l = new List<int>();
                l.If().IsEmpty.ThenThrow();
            });

            Xunit.Assert.DoesNotThrow(() =>
            {
                var l = new List<int>() { 1 };
                l.If().IsEmpty.ThenThrow();
            });
        }

        [Fact]
        public void WhenEnumerationIsEmptyThenThrow()
        {
            Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                var l = new List<int>() as IEnumerable;
                l.If().IsEmpty.ThenThrow();
            });

            Xunit.Assert.DoesNotThrow(() =>
            {
                var l = new List<int>() { 1 } as IEnumerable;
                l.If().IsEmpty.ThenThrow();
            });
        }

        [Fact]
        public void WhenContainsLessThanThenThrow()
        {

            Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                var l = new List<int> { 1, 2 };
                l.If().ContainsLessThan(3).ThenThrow();
            });

            Xunit.Assert.DoesNotThrow(() =>
            {
                var l = new List<int> { 1, 2 };
                l.If().ContainsLessThan(2).ThenThrow();
            });
        }

        [Fact]
        public void WhenContainsMoreThanThenThrow()
        {
            Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                var l = new List<int> { 1, 2, 3, 4 };
                l.If().ContainsMoreThan(3).ThenThrow();
            });

            Xunit.Assert.DoesNotThrow(() =>
            {
                var l = new List<int> { 1, 2 };
                l.If().ContainsMoreThan(2).ThenThrow();
            });
        }

        [Fact]
        public void WhenZeroThenThrow()
        {
            Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                int i = 0;
                i.If().IsZero.ThenThrow();
            });

            Xunit.Assert.DoesNotThrow(() =>
            {
                int i = 1;
                i.If().IsZero.ThenThrow();
            });

            Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                double d = 0.0;
                d.If().IsZero.ThenThrow();
            });

            Xunit.Assert.DoesNotThrow(() =>
            {
                double d = 0.0001;
                d.If().IsZero.ThenThrow();
            });

        }

        [Fact]
        public void WhenNotEqualToThenThrow()
        {
            Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                int i = 0;
                i.If().IsNotEqualTo(1).ThenThrow();
            });

            Xunit.Assert.DoesNotThrow(() =>
            {
                int i = 1;
                i.If().IsNotEqualTo(1).ThenThrow();
            });

        }

        [Fact]
        public void WhenEqualToThenThrow()
        {
            Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                int i = 1;
                i.If().IsEqualTo(1).ThenThrow();
            });

            Xunit.Assert.DoesNotThrow(() =>
            {
                int i = 0;
                i.If().IsEqualTo(1).ThenThrow();
            });

        }

        [Fact]
        public void WhenSmallerThanThenThrow()
        {
            Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                int i = 1;
                i.If().IsSmallerThan(2).ThenThrow();
            });

            Xunit.Assert.DoesNotThrow(() =>
            {
                int i = 1;
                i.If().IsSmallerThan(0).ThenThrow();
            });

        }

        [Fact]
        public void WhenLargerThanThenThrow()
        {
            Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                int i = 3;
                i.If().IsLargerThan(2).ThenThrow();
            });

            Xunit.Assert.DoesNotThrow(() =>
            {
                int i = 1;
                i.If().IsLargerThan(2).ThenThrow();
            });

        }

        [Fact]
        public void WhenPositiveThenThrow()
        {
            Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                int i = 3;
                i.If().IsPositive.ThenThrow();
            });

            Xunit.Assert.DoesNotThrow(() =>
            {
                int i = 0;
                i.If().IsPositive.ThenThrow();
            });

            Xunit.Assert.DoesNotThrow(() =>
            {
                int i = -1;
                i.If().IsPositive.ThenThrow();
            });

        }

        [Fact]
        public void WhenNegativeThenThrow()
        {
            Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                int i = -1;
                i.If().IsNegative.ThenThrow();
            });

            Xunit.Assert.DoesNotThrow(() =>
            {
                int i = 0;
                i.If().IsNegative.ThenThrow();
            });

            Xunit.Assert.DoesNotThrow(() =>
            {
                int i = 1;
                i.If().IsNegative.ThenThrow();
            });

        }

        [Fact]
        public void WhenNullableThenThrow()
        {
            Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                int i = -1;
                i.If().IsNegative.ThenThrow();
            });

            Xunit.Assert.DoesNotThrow(() =>
            {
                int i = 0;
                i.If().IsNegative.ThenThrow();
            });

            Xunit.Assert.DoesNotThrow(() =>
            {
                int i = 1;
                i.If().IsNegative.ThenThrow();
            });

        }

        [Fact]
        public void WhenIsEmptyOrContainsWhitespaceThenThrow()
        {
            Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                string s = "   ";
                s.If().IsEmptyOrWhitespace.ThenThrow();
            });

            Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                string s = "";
                s.If().IsEmptyOrWhitespace.ThenThrow();
            });

            Xunit.Assert.DoesNotThrow(() =>
            {
                string s = "sasd";
                s.If().IsEmptyOrWhitespace.ThenThrow();
            });

        }

        [Fact]
        public void WhenIsInvalidEmailThenThrow()
        {
            Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                string s = "asdad@";
                s.If().IsNotValidEmail.ThenThrow();
            });

            Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                string s = "asdad";
                s.If().IsNotValidEmail.ThenThrow();
            });

            Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                string s = "asdad@asd..se";
                s.If().IsNotValidEmail.ThenThrow();
            });

            Xunit.Assert.DoesNotThrow(() =>
            {
                string s = "asdasd@asdad.se";
                s.If().IsEmptyOrWhitespace.ThenThrow();
            });

            Xunit.Assert.DoesNotThrow(() =>
            {
                string s = "asdasd.asdad@asdad.se";
                s.If().IsEmptyOrWhitespace.ThenThrow();
            });

            Xunit.Assert.DoesNotThrow(() =>
            {
                string s = "asdasd.asdad@asdad..asdad.se";
                s.If().IsEmptyOrWhitespace.ThenThrow();
            });

        }



        [Fact]
        public void WhenIsShorterThanThenThrow()
        {
            Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                string s = "123";
                s.If().IsShorterThan(4).ThenThrow();
            });

            Xunit.Assert.DoesNotThrow(() =>
            {
                string s = "1234";
                s.If().IsShorterThan(4).ThenThrow();
            });

            Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                var span = TimeSpan.FromHours(2);
                span.If().IsShorterThan(TimeSpan.FromHours(3)).ThenThrow();
            });

            Xunit.Assert.DoesNotThrow(() =>
            {
                var span = TimeSpan.FromHours(2);
                span.If().IsShorterThan(TimeSpan.FromHours(1)).ThenThrow();
            });

        }

        [Fact]
        public void WhenIsLongerThanThenThrow()
        {
            Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                string s = "123";
                s.If().IsLongerThan(2).ThenThrow();
            });

            Xunit.Assert.DoesNotThrow(() =>
            {
                string s = "123";
                s.If().IsLongerThan(3).ThenThrow();
            });

            Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                var span = TimeSpan.FromHours(2);
                span.If().IsLongerThan(TimeSpan.FromHours(1)).ThenThrow();
            });

            Xunit.Assert.DoesNotThrow(() =>
            {
                var span = TimeSpan.FromHours(2);
                span.If().IsLongerThan(TimeSpan.FromHours(3)).ThenThrow();
            });

        }

        [Fact]
        public void WhenDoesNotContainNonAlphaCharsThenThrow()
        {
            Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                string s = "asd123";
                s.If().DoesNotContainAlphaChars.ThenThrow();
            });

            Xunit.Assert.DoesNotThrow(() =>
            {
                string s = "123@";
                s.If().DoesNotContainAlphaChars.ThenThrow();
            });

        }

        [Fact]
        public void WhenDoesNotContainDigitThenThrow()
        {
            Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                string s = "asd";
                s.If().DoesNotContainDigit.ThenThrow();
            });

            Xunit.Assert.DoesNotThrow(() =>
            {
                string s = "asd123";
                s.If().DoesNotContainDigit.ThenThrow();
            });

        }

        [Fact]
        public void WhenDoesNotContainTextThenThrow()
        {
            Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                string s = "asd";
                s.If().DoesNotContain("e").ThenThrow();
            });

            Xunit.Assert.DoesNotThrow(() =>
            {
                string s = "asd123";
                s.If().DoesNotContain("asd").ThenThrow();
            });

        }

        [Fact]
        public void WhenDoesNotStartWithTextThenThrow()
        {
            Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                string s = "asd";
                s.If().DoesNotStartWith("sd").ThenThrow();
            });

            Xunit.Assert.DoesNotThrow(() =>
            {
                string s = "asd123";
                s.If().DoesNotStartWith("asd").ThenThrow();
            });

        }

        [Fact]
        public void WhenDoesNotEndWithTextThenThrow()
        {
            Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                string s = "asd";
                s.If().DoesNotEndWith("as").ThenThrow();
            });

            Xunit.Assert.DoesNotThrow(() =>
            {
                string s = "asd123";
                s.If().DoesNotEndWith("123").ThenThrow();
            });

        }

        [Fact]
        public void WhenNotWithinThenThrow()
        {
            Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                int i = 5;
                i.If().IsNotWithin(1, 4).ThenThrow();
            });

            Xunit.Assert.DoesNotThrow(() =>
            {
                int i = 5;
                i.If().IsNotWithin(3, 6).ThenThrow();
            });

            DateTime dMin = new DateTime(2014, 05, 10);
            DateTime dMax = new DateTime(2014, 05, 23);
            Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                DateTime d = new DateTime(2014, 05, 24);
                d.If().IsNotWithin(dMin, dMax).ThenThrow();
            });

            Xunit.Assert.DoesNotThrow(() =>
            {
                DateTime d = new DateTime(2014, 05, 14);
                d.If().IsNotWithin(dMin, dMax).ThenThrow();
            });


        }

        [Fact]
        public void WhenIsWithinThenThrow()
        {
            Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                int i = 3;
                i.If().IsWithin(1, 4).ThenThrow();
            });

            Xunit.Assert.DoesNotThrow(() =>
            {
                int i = 5;
                i.If().IsWithin(6, 9).ThenThrow();
            });

            DateTime dMin = new DateTime(2014, 05, 10);
            DateTime dMax = new DateTime(2014, 05, 23);
            Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                DateTime d = new DateTime(2014, 05, 15);
                d.If().IsWithin(dMin, dMax).ThenThrow();
            });

            Xunit.Assert.DoesNotThrow(() =>
            {
                DateTime d = new DateTime(2014, 05, 25);
                d.If().IsWithin(dMin, dMax).ThenThrow();
            });

            int i2 = 3;
            Assert.True(i2.If().IsWithin(1, 4).IsValid);

        }

        public class SomeClassWithoutValidate
        {


        }

        public class SomeClassWithValidateB
        {
            public string Name { get; set; }
            public int Number { get; set; }

            public ObjectAssertion IsInvalid()
            {
                return ObjectAssertion.New(
                    Name.If().IsNull, 
                    Number.If().IsEqualTo(4));
            }
        }

        public class SomeClassWithValidate
        {
            public string Name { get; set; }
            public int Number { get; set; }
            public SomeClassWithValidateB Prop { get; set; }

            public SomeClassWithValidate()
            {
                Prop = new SomeClassWithValidateB();
            }

            public ObjectAssertion IsInvalid()
            {
                return ObjectAssertion.New(Prop.If().IsInvalid);
            }
        }

        [Fact]
        public void WhenDTOIsInValidThenThrow()
        {

            //Xunit.Assert.Throws<MissingMemberException>(() =>
            //{
            //    var x = new SomeClassWithoutValidate();

            //    x.If().IsInvalid.ThenThrow();
            //});

            Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                var x = new SomeClassWithValidate();
                x.If().IsInvalid.ThenThrow();
            });

        }

        [Fact]
        public void WhenInOneOfMultipleRangesThenReturnIsValid() {
            double heading = 345;

            var isTrue = heading.If()
                .IsWithin(315, 360).Or(heading).IsWithin(0, 45).Or(heading).IsWithin(135, 225).IsValid;

            Assert.True(isTrue);
        }

        public class SomeClass
        {

        }

        [Fact]
        public void WhenAssertingMultipleItemsInOneAssertThenYouGetXAssertsEvenIfShortCircuit()
        {
            SomeClass s1 = null;
            SomeClass s2 = null;
            string d = "";

            var errMsg = s1.If("s1")
                .Or(s2, "s2")
                .Or(d, "s3").IsNull.ToString();

            Xunit.Assert.True(errMsg.Contains("1") && errMsg.Contains("2") && errMsg.Contains("3"));

        }

        [Fact]
        public void AnyAssertWorksForDoubles()
        {
            
            double d1 = 1;
            double d2 = 3;
            double d3 = 4;

            var errMsg = d1.If("1").Or(d2, "2").Or(d3, "3").IsSmallerThan(5).IsLargerThan(0).ToString();
            Xunit.Assert.True(errMsg.Contains("1") && errMsg.Contains("2") && errMsg.Contains("3"));

        }

        [Fact]
        public void WhenBoolIsFalseThenThrow()
        {

            Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                (1 == 0).If("expr").IsFalse.ThenThrow();
            });

            Xunit.Assert.DoesNotThrow(() =>
            {
                (1 == 1).If("expr").IsFalse.ThenThrow();   
            });

        }

        [Fact]
        public void WhenBoolIsTrueThenThrow()
        {

            Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                (1 == 1).If("expr").IsTrue.ThenThrow();
            });

            Xunit.Assert.DoesNotThrow(() =>
            {
                (1 == 0).If("expr").IsTrue.ThenThrow();
            });

        }

        [Fact]
        public void WhenOnSameDayThenThrow()
        {

            DateTime d = DateTime.Now;
            DateTime d2 = DateTime.Now.Date.AddHours(2);

            Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                d.If().SameDayAs(d2).ThenThrow();
            });

            Xunit.Assert.DoesNotThrow(() =>
            {
                d.If().SameDayAs(d2.AddDays(1)).ThenThrow();
            });
        }

        [Fact]
        public void WhenNotOnSameDayThenThrow()
        {

            DateTime d = DateTime.Now;
            DateTime d2 = DateTime.Now.Date.AddHours(2).AddDays(1);

            Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                d.If().NotSameDayAs(d2).ThenThrow();
            });

            Xunit.Assert.DoesNotThrow(() =>
            {
                d.If().NotSameDayAs(d2.AddDays(-1)).ThenThrow();
            });
        }

        [Fact]
        public void WhenDoesNotContainKeyThenThrow()
        {
            Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                Dictionary<string, int> d = new Dictionary<string, int>();
                d.If().DoesNotContainKey("some key").ThenThrow();
            });

            Xunit.Assert.DoesNotThrow(() =>
            {
                Dictionary<string, int> d = new Dictionary<string, int>();
                d.Add("some key", 1);
                d.If().DoesNotContainKey("some key").ThenThrow();
            });

        }

        [Fact]
        public void WhenContainKeyThenThrow()
        {
            Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                Dictionary<string, int> d = new Dictionary<string, int>();
                d.Add("some key", 1);
                d.If().ContainsKey("some key").ThenThrow();
            });

            Xunit.Assert.DoesNotThrow(() =>
            {
                Dictionary<string, int> d = new Dictionary<string, int>();
                d.If().ContainsKey("some key").ThenThrow();
            });

        }

        [Fact]
        public void WhenIsNotValidPersonalNumberThenThrow()
        {
            Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                string s = "123987";
                s.If().IsNotValidPersonalNumber().ThenThrow();
            });

            Xunit.Assert.DoesNotThrow(() =>
            {
                string s = "6708021586";
                s.If().IsNotValidPersonalNumber().ThenThrow();
            });

        }
    }
}