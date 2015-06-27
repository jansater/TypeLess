using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;
using TypeLess.Extensions.Shipping;
using TypeLess.Extensions.Usa;
using TypeLess.Extensions.Books;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TypeLess.Net.Tests
{

    public enum Types { 
        [Description("Much better than type B")]
        [Display(Name="TYPE A", Order=1)]
        TypeA,
        [Description("Much worse than type A")]
        [Display(Name = "TYPE B", Order = 2)]
        TypeB
    }

    public class EnumHelperTest
    {
        [Fact]
        public void ItsPossibleToGetDescriptionAndDisplayNameFromEnum() {

            var typeA = Types.TypeA.GetDisplayAttributes();

            Assert.Equal("Much better than type B", typeA.Description);
            Assert.Equal("TYPE A", typeA.DisplayName);
        }

        [Fact]
        public void ItsPossibleToGetOrder()
        {
            var typeA = Types.TypeA.GetDisplayAttributes();

            Assert.Equal("Much better than type B", typeA.Description);
            Assert.Equal("TYPE A", typeA.DisplayName);
        }
    }
}
