using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;
using TypeLess.Extensions.Shipping;

namespace TypeLess.Tests
{
    public class ExtensionAsserts
    {
        [Fact]
        public void WhenNotValidIMOThenThrow()
        {
            Xunit.Assert.Throws<ArgumentNullException>(() => {

                string s = "asdasd";
                s.If().IsNotValidImoNr().ThenThrow();

            });
        }


    }
}
