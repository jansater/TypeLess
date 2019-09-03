using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;
using TypeLess.Extensions.Sweden;
using System.Threading;
using System.Globalization;

namespace TypeLess.Tests
{
    public class Asserts
    {

        [Fact]
        public void QuickSyntaxThrows()
        {
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                (1 == 1).ThenThrow<ArgumentException>("1 should not equal 2");
            });
            Assert.Equal("1 should not equal 2", ex.Message);

            (1 == 2).ThenThrow<ArgumentException>("1 should not equal 2");
        }

        [Fact]
        public void QuickSyntaxOtherwiseGetCalled() {
            int a = 1;
            int b = 2;
            var hello = (a == b).ThenThrow<ArgumentException>("1 should not equal 2").OtherwiseReturn(x => "Hello");
            Assert.Equal("Hello", hello);
        }
        

        [Fact]
        public void WhenNullThenDontThrowIfJustToString()
        {


            string s = null;
            var msg = s.If("s").IsNull.ToString();

            Xunit.Assert.Contains("required", msg);


        }

        [Fact]
        public void ChainingAlerts()
        {
            var res = Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                object o2 = null;
                object o1 = new object();
                int i1 = 1;

                o1.If("o1").IsNull
                    .Or(i1.If("i1").IsEqualTo(1), "<br>")
                    .Or(o2.If("o2").IsNull, "<br>")
                    .ThenThrow();
            });

            Assert.StartsWith("i1 must not be equal to 1<br>o2 is required", res.Message);
        }

        [Fact]
        public void WhenOneOfMultipleObjectsIsNullThenOnlyMessageForOneIsReturned()
        {
            var res = Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                SomeClass o2 = null;
                object o1 = new object();

                o2.If("o2").Or(o1, "o1").IsNull.ThenThrow();
            });

            Assert.Equal("o2 is required", res.Message);
        }

        [Fact]
        public void WhenNullThenThrow()
        {
            var res = Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                string s = null;
                s.If("s").IsNull.ThenThrow();

            });

            Assert.StartsWith("s is required", res.Message);

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


            int? s2 = 1;
            s2.If().IsNull.ThenThrow();



            object o1 = new object();
            object o2 = new object();
            object o3 = new object();

            o1.If("o1").Or(o2, "o2").Or(o3, "o3").IsNull.ThenThrow();


            o1 = 1;
            o2 = 1;
            o3 = 1;

            o1.If("o1").Or(o2, "o2").Or(o3, "o3").IsNull.ThenThrow();


        }

        [Fact]
        public void WhenNullThenThrowWithSpecificException()
        {

            var res = Xunit.Assert.Throws<ArgumentException>(() =>
            {
                string s = null;
                s.If("s").IsNull.ThenThrow<ArgumentException>();
            });

            Assert.StartsWith("s is required", res.Message);

        }


        [Fact]
        public void WhenNullThenDontCheckFurther()
        {

            string s = null;
            Xunit.Assert.Equal(1, s.If("s").IsNull.IsEmpty.ErrorCount);


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
            var res = Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                string s = "";
                s.If("s").IsEmpty.ThenThrow();
            });

            Assert.StartsWith("s must not be empty", res.Message);


            string s2 = "d";
            s2.If().IsEmpty.ThenThrow();

        }

        [Fact]
        public void WhenStringDoesNotMatchPatternThenThrow()
        {
            var res = Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                string s = "here is a long string";
                s.If("s").DoesNotMatch("\\d").ThenThrow();
            });

            Assert.StartsWith("s must match pattern \\d", res.Message);


            string s2 = "asd 23123 asd";
            s2.If("s").DoesNotMatch("\\d").ThenThrow();

        }

        [Fact]
        public void WhenStringMatchPatternThenThrow()
        {
            var res = Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                string s = "here is 22 a long string";
                s.If("s").Match("\\d").ThenThrow();
            });

            Assert.StartsWith("s must not match pattern \\d", res.Message);


            string s2 = "asd asd";
            s2.If("s").Match("\\d").ThenThrow();

        }

        [Fact]
        public void WhenIsTrueThenThrow()
        {
            var res = Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                string s = "";
                s.If("s").IsTrue(x => true, "<name> must be false i guess").ThenThrow();
            });

            Assert.StartsWith("s must be false i guess", res.Message);


            string s2 = "d";
            s2.If().IsTrue(x => false, "").ThenThrow();

        }

        [Fact]
        public void WhenIsFalseThenThrow()
        {
            var res = Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                string s = "";
                s.If("s").IsFalse(x => false, "{0} must be true i guess").ThenThrow();
            });

            Assert.StartsWith("s must be true i guess", res.Message);


            string s2 = "d";
            s2.If().IsFalse(x => true, "").ThenThrow();

        }

        [Fact]
        public void WhenIsFalseNoStringThenThrow()
        {
            var res = Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                string s = "";
                s.If("s").IsFalse(x => false).ThenThrow("s must be true i guess");
            });

            Assert.StartsWith("s must be true i guess", res.Message);


            string s2 = "d";
            s2.If().IsFalse(x => true).ThenThrow();

        }

        [Fact]
        public void WhenIsEmptyThenThrow()
        {
            var res = Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                var l = new List<int>();
                l.If("s").IsEmpty.ThenThrow();
            });

            Assert.StartsWith("s must not be empty", res.Message);


            var l2 = new List<int>() { 1 };
            l2.If().IsEmpty.ThenThrow();

        }

        [Fact]
        public void WhenEnumerationIsEmptyThenThrow()
        {
            var res = Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                var l = new List<int>() as IEnumerable;
                l.If("s").IsEmpty.ThenThrow();
            });

            Assert.StartsWith("s must not be empty", res.Message);


            var l2 = new List<int>() { 1 } as IEnumerable;
            l2.If().IsEmpty.ThenThrow();

        }

        [Fact]
        public void WhenContainsLessThanThenThrow()
        {

            var res = Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                var l = new List<int> { 1, 2 };
                l.If("s").ContainsLessThan(3).ThenThrow();
            });

            Assert.StartsWith("s must contain more than 3 items", res.Message);


            var l2 = new List<int> { 1, 2 };
            l2.If().ContainsLessThan(2).ThenThrow();

        }

        [Fact]
        public void WhenContainsMoreThanThenThrow()
        {
            var res = Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                var l = new List<int> { 1, 2, 3, 4 };
                l.If("s").ContainsMoreThan(3).ThenThrow();
            });

            Assert.StartsWith("s must contain less than 3 items", res.Message);


            var l2 = new List<int> { 1, 2 };
            l2.If().ContainsMoreThan(2).ThenThrow();

        }

        [Fact]
        public void WhenZeroThenThrow()
        {
            var res = Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                int i = 0;
                i.If("s").IsZero.ThenThrow();
            });

            Assert.StartsWith("s must be non zero", res.Message);


            int i2 = 1;
            i2.If().IsZero.ThenThrow();


            Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                double d = 0.0;
                d.If().IsZero.ThenThrow();
            });


            double d2 = 0.0001;
            d2.If().IsZero.ThenThrow();


        }

        [Fact]
        public void WhenNotEqualToThenThrow()
        {
            var res = Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                int i = 0;
                i.If("s").IsNotEqualTo(1).ThenThrow();
            });

            Assert.StartsWith("s must be equal to 1", res.Message);


            int i2 = 1;
            i2.If().IsNotEqualTo(1).ThenThrow();


        }

        [Fact]
        public void WhenEqualToThenThrow()
        {
            var res = Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                int i = 1;
                i.If("s").IsEqualTo(1).ThenThrow();
            });

            Assert.StartsWith("s must not be equal to 1", res.Message);


            int i2 = 0;
            i2.If().IsEqualTo(1).ThenThrow();


        }

        [Fact]
        public void WhenSmallerThanThenThrow()
        {
            var res = Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                int i = 1;
                i.If("s").IsLessThan(2).ThenThrow();
            });

            Assert.StartsWith("s must be greater than 2", res.Message);


            int i2 = 1;
            i2.If().IsLessThan(0).ThenThrow();


            int requestId = 1;
            int userId = 3;


            requestId.If("request id").Or(userId, "user id").IsLessThan(1).ThenThrow();

        }

        //[Fact]
        //public void WhenSmallerThanOrEqualToThenThrow()
        //{
        //    var res = Xunit.Assert.Throws<ArgumentNullException>(() =>
        //    {
        //        int i = 1;
        //        i.If("s").IsSmallerThan(2).ThenThrow();
        //    });

        //    Assert.True(res.Message.StartsWith("s must be greater than 2"));

        //    Xunit.Assert.DoesNotThrow(() =>
        //    {
        //        int i = 1;
        //        i.If().IsSmallerThan(0).ThenThrow();
        //    });

        //    int requestId = 1;
        //    int userId = 3;

        //    Xunit.Assert.DoesNotThrow(() =>
        //    {
        //        requestId.If("request id").Or(userId, "user id").IsSmallerThan(1).ThenThrow();
        //    });
        //}

        [Fact]
        public void WhenLargerThanThenThrow()
        {
            var res = Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                int i = 3;
                i.If("s").IsGreaterThan(2).ThenThrow();
            });

            Assert.StartsWith("s must be smaller than 2", res.Message);


            int i2 = 1;
            i2.If().IsGreaterThan(2).ThenThrow();


        }

        [Fact]
        public void WhenPositiveThenThrow()
        {
            var res = Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                int i = 3;
                i.If("s").IsPositive.ThenThrow();
            });

            Assert.StartsWith("s must be zero or negative", res.Message);


            int i2 = 0;
            i2.If().IsPositive.ThenThrow();

            int i3 = -1;
            i3.If().IsPositive.ThenThrow();


        }

        [Fact]
        public void WhenNegativeThenThrow()
        {
            var res = Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                int i = -1;
                i.If("s").IsNegative.ThenThrow();
            });

            Assert.StartsWith("s must be zero or positive", res.Message);


            int i2 = 0;
            i2.If().IsNegative.ThenThrow();

            int i3 = 1;
            i3.If().IsNegative.ThenThrow();


        }

        [Fact]
        public void WhenNullableThenThrow()
        {
            var res = Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                int i = -1;
                i.If("s").IsNegative.ThenThrow();
            });

            Assert.StartsWith("s must be zero or positive", res.Message);


            int i2 = 0;
            i2.If().IsNegative.ThenThrow();

            int i3 = 1;
            i3.If().IsNegative.ThenThrow();


        }

        [Fact]
        public void WhenIsEmptyOrContainsWhitespaceThenThrow()
        {
            var res = Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                string s = "   ";
                s.If("s").IsEmptyOrWhitespace.ThenThrow();
            });

            Assert.StartsWith("s must not be empty", res.Message);

            Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                string s = "";
                s.If().IsEmptyOrWhitespace.ThenThrow();
            });


            string s2 = "sasd";
            s2.If().IsEmptyOrWhitespace.ThenThrow();


        }

        [Fact]
        public void WhenIsInvalidEmailThenThrow()
        {
            var res = Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                string s = "asdad@";
                s.If("s").IsNotValidEmail.ThenThrow();
            });

            Assert.StartsWith("s must be a valid email address", res.Message);

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


            string s2 = "asdasd@asdad.se";
            s2.If().IsEmptyOrWhitespace.ThenThrow();


            string s3 = "asdasd.asdad@asdad.se";
            s3.If().IsEmptyOrWhitespace.ThenThrow();

            string s4 = "asdasd.asdad@asdad..asdad.se";
            s4.If().IsEmptyOrWhitespace.ThenThrow();


        }



        [Fact]
        public void WhenIsShorterThanThenThrow()
        {
            var res = Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                string s = "123";
                s.If("s").IsShorterThan(4).ThenThrow();
            });

            Assert.StartsWith("s must be longer than 3 characters", res.Message);



            string s2 = "1234";
            s2.If().IsShorterThan(4).ThenThrow();


            Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                var span = TimeSpan.FromHours(2);
                span.If().IsShorterThan(TimeSpan.FromHours(3)).ThenThrow();
            });


            var span2 = TimeSpan.FromHours(2);
            span2.If().IsShorterThan(TimeSpan.FromHours(1)).ThenThrow();


        }

        [Fact]
        public void WhenIsLongerThanThenThrow()
        {
            var res = Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                string s = "123";
                s.If("s").IsLongerThan(2).ThenThrow();
            });

            Assert.StartsWith("s must be shorter than 3 characters", res.Message);



            string s2 = "123";
            s2.If().IsLongerThan(3).ThenThrow();


            Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                var span = TimeSpan.FromHours(2);
                span.If().IsLongerThan(TimeSpan.FromHours(1)).ThenThrow();
            });


            var span2 = TimeSpan.FromHours(2);
            span2.If().IsLongerThan(TimeSpan.FromHours(3)).ThenThrow();


        }

        [Fact]
        public void WhenDoesNotContainNonAlphaCharsThenThrow()
        {
            var res = Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                string s = "asd123";
                s.If("s").DoesNotContainAlphaChars.ThenThrow();
            });

            Assert.StartsWith("s must contain alpha numeric characters", res.Message);


            string s2 = "123@";
            s2.If().DoesNotContainAlphaChars.ThenThrow();


        }

        [Fact]
        public void WhenDoesNotContainDigitThenThrow()
        {
            var res = Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                string s = "asd";
                s.If("s").DoesNotContainDigit.ThenThrow();
            });

            Assert.StartsWith("s must contain at least 1 digit", res.Message);


            string s2 = "asd123";
            s2.If().DoesNotContainDigit.ThenThrow();


        }

        [Fact]
        public void WhenDoesNotContainTextThenThrow()
        {
            var res = Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                string s = "asd";
                s.If("s").DoesNotContain("e").ThenThrow();
            });

            Assert.StartsWith("s must contain text e", res.Message);


            string s2 = "asd123";
            s2.If().DoesNotContain("asd").ThenThrow();


        }

        [Fact]
        public void WhenDoesNotStartWithTextThenThrow()
        {
            var res = Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                string s = "asd";
                s.If("s").DoesNotStartWith("sd").ThenThrow();
            });

            Assert.StartsWith("s must start with text sd", res.Message);


            string s2 = "asd123";
            s2.If().DoesNotStartWith("asd").ThenThrow();


        }

        [Fact]
        public void WhenDoesNotEndWithTextThenThrow()
        {
            var res = Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                string s = "asd";
                s.If("s").DoesNotEndWith("as").ThenThrow();
            });

            Assert.StartsWith("s must end with text as", res.Message);


            string s2 = "asd123";
            s2.If().DoesNotEndWith("123").ThenThrow();


        }

        [Fact]
        public void WhenNotWithinThenThrow()
        {
            var res = Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                int i = 5;
                i.If("s").IsNotWithin(1, 4).ThenThrow();
            });

            Assert.StartsWith("s must be within 1 and 4", res.Message);


            int i2 = 5;
            i2.If().IsNotWithin(3, 6).ThenThrow();


            DateTime dMin = new DateTime(2014, 05, 10);
            DateTime dMax = new DateTime(2014, 05, 23);
            Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                DateTime d = new DateTime(2014, 05, 24);
                d.If().IsNotWithin(dMin, dMax).ThenThrow();
            });


            DateTime d2 = new DateTime(2014, 05, 14);
            d2.If().IsNotWithin(dMin, dMax).ThenThrow();



        }

        [Fact]
        public void WhenIsWithinThenThrow()
        {
            var res = Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                int i = 3;
                i.If("s").IsWithin(1, 4).ThenThrow();
            });

            Assert.StartsWith("s must not be within 1 and 4", res.Message);


            int i2 = 5;
            i2.If().IsWithin(6, 9).ThenThrow();


            DateTime dMin = new DateTime(2014, 05, 10);
            DateTime dMax = new DateTime(2014, 05, 23);
            Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                DateTime d = new DateTime(2014, 05, 15);
                d.If().IsWithin(dMin, dMax).ThenThrow();
            });


            DateTime d2 = new DateTime(2014, 05, 25);
            d2.If().IsWithin(dMin, dMax).ThenThrow();


            int i3 = 3;
            Assert.True(i3.If().IsWithin(1, 4).True);

        }

        public class SomeClassWithoutValidate
        {


        }

        public class SomeClassWithValidateB
        {
            public string Name { get; set; }
            public int Number { get; set; }

            public SomeClassWithValidateB()
            {
                Name = "Some long string";
            }

            public ObjectAssertion IsInvalid()
            {
                return ObjectAssertion.New(
                    Name.If("Name2").IsLongerThan(5),
                    Number.If("Number2").IsEqualTo(0)
                    );
            }
        }

        public class SomeClassWithValidate
        {
            public string Name { get; set; }
            public int Number { get; set; }
            public SomeClassWithValidateB Prop { get; set; }

            public SomeClassWithValidate()
            {
                Name = "Some long string";
                Prop = new SomeClassWithValidateB();
            }

            public ObjectAssertion IsInvalid()
            {
                return ObjectAssertion.New(
                    Name.If("Name1").IsLongerThan(6),
                    Number.If("Number1").IsEqualTo(0),
                    Prop.If("Prop").IsInvalid
                    );
            }
        }

        [Fact]
        public void WhenCombiningAssertsErrorMessageIsCorrect()
        {

            double d = 1;
            double d2 = 10;

            var errMsg = d.If("d").IsEqualTo(1).IsGreaterThan(0).Or(d2.If("d2").IsEqualTo(10)).ToString();
            Assert.StartsWith("d must not be equal to 1 and d must be smaller than 0. d2 must not be equal to 10", errMsg);

        }

        [Fact]
        public void EnumerableContains()
        {

            var list = new List<string>() { "1", "2", "3", "4", "5" };


            list.If().DoesNotContain("1").ThenThrow();


            var res = Assert.Throws<ArgumentNullException>(() =>
            {
                list.If("list").DoesNotContain("6").ThenThrow();
            });

            Assert.Equal("list must contain 6", res.Message);


            list.If().Contains("6").ThenThrow();


            res = Assert.Throws<ArgumentNullException>(() =>
            {
                list.If("list").Contains("1").ThenThrow();
            });

            Assert.Equal("list must not contain 1", res.Message);

            //subset

            list.If().DoesNotContain("1", "2", "5").ThenThrow();


            res = Assert.Throws<ArgumentNullException>(() =>
            {
                list.If("list").DoesNotContain("1", "2", "6").ThenThrow();
            });

            Assert.Equal("list must contain 1,2,6", res.Message);


            list.If().Contains("0", "6", "7").ThenThrow();

            res = Assert.Throws<ArgumentNullException>(() =>
            {
                list.If("list").Contains("1", "2", "5").ThenThrow();
            });

            Assert.Equal("list must not contain 1,2,5", res.Message);

        }

        [Fact]
        public void WhenDTOIsInValidThenThrow()
        {

            //Xunit.Assert.Throws<MissingMemberException>(() =>
            //{
            //    var x = new SomeClassWithoutValidate();
            //    x.If().IsInvalid.ThenThrow();
            //});

            var res = Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                var x = new SomeClassWithValidate();
                x.If("s").IsInvalid.ThenThrow();
            });

            Assert.StartsWith("Name1 must be shorter than 7 characters and Number1 must not be equal to 0 and Name2 must be shorter than 6 characters and Number2 must not be equal to 0", res.Message);
        }

        [Fact]
        public void WhenInOneOfMultipleRangesThenReturnIsValid()
        {
            double heading = 345;

            var isTrue = heading.If()
                .IsWithin(315, 360)
                .Or(heading).IsWithin(0, 45)
                .Or(heading).IsWithin(135, 225);

            Assert.True(isTrue.True);

            isTrue = heading.If()
                .IsWithin(315, 360)
                .IsWithin(0, 45)
                .IsWithin(135, 225);

            Assert.True(isTrue.True);

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

            Xunit.Assert.True(errMsg.Contains("s1") && errMsg.Contains("s2") && !errMsg.Contains("s3"));

        }

        [Fact]
        public void ErrorMsgCanProcessArgs()
        {
            var res = Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                (1 == 0).If("expr").IsFalse.ThenThrow("1 must be equal to {0}", 0);
            });

            Assert.StartsWith("1 must be equal to 0", res.Message);

            var res2 = Xunit.Assert.Throws<SomeException>(() =>
            {
                (1 == 0).If("expr").IsFalse.ThenThrow<SomeException>("1 must be equal to {0}", 2);
            });

            Assert.StartsWith("1 must be equal to 2", res2.Message);

        }

        [Fact]
        public void AnyAssertWorksForDoubles()
        {

            double d1 = 1;
            double d2 = 3;
            double d3 = 4;

            var errMsg = d1.If("d1").Or(d2, "d2").Or(d3, "d3").IsLessThan(5).IsGreaterThan(0).ToString();

            Xunit.Assert.StartsWith("d1 must be greater than 5. d2 must be greater than 5. d3 must be greater than 5 and d1 must be smaller than 0. d2 must be smaller than 0. d3 must be smaller than 0", errMsg);

        }

        [Fact]
        public void WhenBoolIsFalseThenThrow()
        {

            var res = Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                (1 == 0).If("expr").IsFalse.ThenThrow();
            });

            Assert.StartsWith("expr must be true", res.Message);


            (1 == 1).If("expr").IsFalse.ThenThrow();


        }

        [Fact]
        public void WhenBoolIsTrueThenThrow()
        {

            var res = Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                (1 == 1).If("expr").IsTrue.ThenThrow();
            });

            Assert.StartsWith("expr must be false", res.Message);


            (1 == 0).If("expr").IsTrue.ThenThrow();


        }

        [Fact]
        public void WhenOnSameDayThenThrow()
        {
            DateTime d = DateTime.Now;
            DateTime d2 = DateTime.Now.Date.AddHours(2);

            var res = Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                d.If("s").SameDayAs(d2).ThenThrow();
            });

            Assert.StartsWith("s must not be on same day as " + d2.ToString("yyyy-MM-dd"), res.Message);


            d.If().SameDayAs(d2.AddDays(1)).ThenThrow();

        }

        [Fact]
        public void WhenNotOnSameDayThenThrow()
        {

            DateTime d = DateTime.Now;
            DateTime d2 = DateTime.Now.Date.AddHours(2).AddDays(1);

            var res = Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                d.If("s").NotSameDayAs(d2).ThenThrow();
            });

            Assert.StartsWith("s must be on same day as " + d2.ToString("yyyy-MM-dd"), res.Message);


            d.If().NotSameDayAs(d2.AddDays(-1)).ThenThrow();

        }

        [Fact]
        public void WhenDoesNotContainKeyThenThrow()
        {
            var res = Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                Dictionary<string, int> d = new Dictionary<string, int>();
                d.If("s").DoesNotContainKey("some key").ThenThrow();
            });

            Assert.StartsWith("s must contain key some key", res.Message);


            Dictionary<string, int> d2 = new Dictionary<string, int>();
            d2.Add("some key", 1);
            d2.If().DoesNotContainKey("some key").ThenThrow();


        }

        [Fact]
        public void WhenContainKeyThenThrow()
        {
            var res = Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                Dictionary<string, int> d = new Dictionary<string, int>();
                d.Add("some key", 1);
                d.If("s").ContainsKey("some key").ThenThrow();
            });

            Assert.StartsWith("s must not contain key some key", res.Message);


            Dictionary<string, int> d2 = new Dictionary<string, int>();
            d2.If().ContainsKey("some key").ThenThrow();


        }

        [Fact]
        public void WhenIsNotValidPersonalNumberThenThrow()
        {
            var res = Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                string s = "123987";
                s.If("s").IsNotValidPersonalNumber().ThenThrow();
            });

            Assert.StartsWith("s must be a valid personal number", res.Message);


            string s2 = "6708021586";
            s2.If().IsNotValidPersonalNumber().ThenThrow();

        }

        [Fact]
        public void WhenIsNotValidUrlThenThrow()
        {
            var res = Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                string s = "http://www.rapidsolutions].se";
                s.If("s").IsNotValidUrl.ThenThrow();
            });

            Assert.StartsWith("s must be a valid URL", res.Message);


            string s2 = "http://www.rapidsolutions.se";
            s2.If("s").IsNotValidUrl.ThenThrow();

        }


    }
}
