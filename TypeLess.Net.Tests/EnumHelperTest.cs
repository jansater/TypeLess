using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;
using TypeLess.Extensions.Shipping;
using TypeLess.Extensions.Usa;
using TypeLess.Extensions.Books;
using TypeLess.Net.Helpers;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TypeLess.Net.Tests
{

    public enum Types { 
        [Description("Much better than type B")]
        [Display(Name="TYPE A")]
        TypeA,
        [Description("Much worse than type A")]
        [Display(Name = "TYPE B")]
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
    }
}
