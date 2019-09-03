using System;
using Xunit;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TypeLess.Net.Tests
{
    [Flags]
    public enum Types
    {
        Undefined,
        [Description("Much better than type B")]
        [Display(Name = "TYPE A", Order = 1, Description = "Much better than type B")]
        TypeA,
        [Description("Much worse than type A")]
        [Display(Name = "TYPE B", Order = 2, Description = "Much better than type A")]
        TypeB,
        [Description("Much worse than type AB")]
        [Display(Order = 0)]
        TypeC = 4
    }

    public enum TypesEnum
    {
        Undefined,
        [Description("Much better than type B")]
        TypeA,
        [Description("Much better than type A")]
        TypeB,
        [Description("Much better than type AB")]
        TypeC
    }

    [Flags]
    public enum TypesShortEnum
    {
        Undefined = 0,
        [Description("Type A")]
        TypeA = 1,
        [Description("Type B")]
        TypeB = 2,
        [Description("Type C")]
        TypeC = 4,
        [Description("Type D")]
        TypeD = 8
    }

    public class EnumHelperTest
    {

        [Fact]
        public void ThreeFlagsCreateCorrectMessage()
        {
            var typeA = TypesShortEnum.TypeA | TypesShortEnum.TypeB | TypesShortEnum.TypeC | TypesShortEnum.TypeD;

            Assert.Equal("Type A, Type B, Type C & Type D", typeA.GetFlagDescription());
            Assert.Equal("Type A & Type B", (TypesShortEnum.TypeA | TypesShortEnum.TypeB).GetFlagDescription());
            Assert.Equal("Type A", (TypesShortEnum.TypeA).GetFlagDescription());
        }

        [Fact]
        public void ItsPossibleToGetDescriptionForEnum()
        {
            var typeA = TypesEnum.TypeC;

            Assert.Equal("Much better than type AB", typeA.GetDescription());
        }

        [Fact]
        public void ItsPossibleToGetDescriptionAndDisplayNameFromEnum()
        {
            var typeA = Types.TypeA;

            Assert.Equal("Much better than type B", typeA.GetFlagDescription());
        }

        [Fact]
        public void ItsPossibleToGetAMergedDescriptionForFlags()
        {
            var type = (Types.TypeA | Types.TypeB);
            Assert.Equal("Much better than type B & Much worse than type A", type.GetFlagDescription());
        }

        [Fact]
        public void ItsPossibleToGetADescriptionForFlags()
        {
            var type = (Types.TypeC);
            Assert.Equal("Much worse than type AB", type.GetFlagDescription());
        }

        [Fact]
        public void ItsPossibleToGetOrder()
        {
            var typeA = Types.TypeA.GetDisplayAttributes();
            var typeB = Types.TypeB.GetDisplayAttributes();
            Assert.Equal(1, typeA.Order);
            Assert.Equal(2, typeB.Order);
        }
    }
}
