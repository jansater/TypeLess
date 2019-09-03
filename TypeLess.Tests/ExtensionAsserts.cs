using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;
using TypeLess.Extensions.Shipping;
using TypeLess.Extensions.Usa;
using TypeLess.Extensions.Books;

namespace TypeLess.Tests
{
    public class ExtensionAsserts
    {
        [Fact]
        public void WhenNotValidIMOThenThrow()
        {
            Xunit.Assert.Throws<ArgumentNullException>(() =>
            {

                string s = "asdasd";
                s.If().IsNotValidImoNr().ThenThrow();

            });
        }

        [Fact]
        public void WhenIsNotValidSSNThenThrow()
        {
            var res = Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                string s = "457-55-546299";
                s.If("s").IsNotValidSocialSecurityNumber().ThenThrow();
            });

            Assert.StartsWith("s must be a valid social security number", res.Message);

            var s2 = "457-55-5462";
            s2.If().IsNotValidSocialSecurityNumber().ThenThrow();
        }

        [Fact]
        public void WhenIsNotValidPhoneNrThenThrow()
        {
            var res = Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                string s = "(425) 555-01233";
                s.If("s").IsNotValidPhoneNumber().ThenThrow();
            });

            Assert.StartsWith("s must be a valid phone number", res.Message);


            var s2 = "(425) 555-0123";
            s2.If().IsNotValidPhoneNumber().ThenThrow();


        }

        [Fact]
        public void WhenIsNotValidZipCodeThenThrow()
        {
            var res = Xunit.Assert.Throws<ArgumentNullException>(() =>
            {
                string s = "95124444444";
                s.If("s").IsNotValidZipCode().ThenThrow();
            });

            Assert.StartsWith("s must be a valid zip code", res.Message);


            var s2 = "95124";
            s2.If().IsNotValidZipCode().ThenThrow();


        }

        //[Fact]
        //public void WhenIsNotValidISBNThenThrow()
        //{
        //    //var res = Xunit.Assert.Throws<ArgumentNullException>(() =>
        //    //{
        //    //    string s = "95124444444";
        //    //    s.If("s").IsNotValidISBN10().ThenThrow();
        //    //});

        //    //Assert.True(res.Message.StartsWith("s must be a valid ISBN"));

        //    Xunit.Assert.DoesNotThrow(() =>
        //    {
        //        string s = "ISBN-10:1449320104";
        //        s.If().IsNotValidISBN10().ThenThrow();
        //    });

        //}


    }
}
