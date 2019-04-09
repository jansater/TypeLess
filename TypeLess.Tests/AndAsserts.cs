using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using TypeLess;

namespace TypeLess.Tests
{
    public class AndAsserts
    {
        [Fact]
        public void WhenAllOperatorsFailThenExceptionIsThrown() {

            int x = 1;
            int y = 2;
            int z = 3;
           
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                x.If("x").IsEqualTo(1).And(y, "y").IsEqualTo(2).And(z, "z").IsEqualTo(3).ThenThrow<ArgumentException>();
            });

            Assert.StartsWith("x must not be equal to 1 when y is equal to 2 and z is equal to 3", ex.Message);
            
        }

        [Fact]
        public void WhenAllOperatorsFailInNegativeStatementThenExceptionIsThrown()
        {

            int x = 1;
            int y = 2;
            int z = 3;

            var ex = Assert.Throws<ArgumentException>(() =>
            {
                x.If("x").IsNotEqualTo(0).And(y, "y").IsNotEqualTo(0).And(z, "z").IsNotEqualTo(0).ThenThrow<ArgumentException>();
            });

            Assert.StartsWith("x must be equal to 0 when y is not equal to 0 and z is not equal to 0", ex.Message);

        }

        [Fact]
        public void WhenFirstOperatorSucceedsThenNoExceptionIsThrown() {

            int x = 1;
            int y = 2;
            int z = 3;
            
            x.If("x").IsEqualTo(2).And(y, "y").IsEqualTo(2).And(z, "z").IsEqualTo(3).ThenThrow<ArgumentException>();
        }

        [Fact]
        public void WhenSecondOperatorSucceedsThenNoExceptionIsThrown()
        {

            int x = 1;
            int y = 2;
            int z = 3;

            x.If("x").IsEqualTo(1).And(y, "y").IsEqualTo(1).And(z, "z").IsEqualTo(3).ThenThrow<ArgumentException>();
        }

        [Fact]
        public void WhenThirdOperatorSucceedsThenNoExceptionIsThrown()
        {

            int x = 1;
            int y = 2;
            int z = 3;

            x.If("x").IsEqualTo(1).And(y, "y").IsEqualTo(2).And(z, "z").IsEqualTo(2).ThenThrow<ArgumentException>();
        }

        [Fact]
        public void WhenAllOperatorsSucceedsThenNoExceptionIsThrown()
        {

            int x = 1;
            int y = 2;
            int z = 3;

            x.If("x").IsEqualTo(0).And(y, "y").IsEqualTo(0).And(z, "z").IsEqualTo(0).ThenThrow<ArgumentException>();
        }

        [Fact]
        public void ThrowsCorrectExceptionType()
        {
            int x = 1;
            Assert.Throws<ArgumentException>(() =>
            {
                x.If("x").IsEqualTo(1).ThenThrow<ArgumentException>();
            });
            
        }

        [Fact]
        public void WhenContainOperatorIsUsedErrorMessageMakesSense()
        {

            int x = 1;
            int[] y = new int[] { 1,4,5 };
            int z = 4;
            
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                x.If("x").IsEqualTo(1).And(y, "y").DoesNotContain(3,4,5).And(z, "z").IsNotEqualTo(3).ThenThrow<ArgumentException>();
            });

            
            Assert.StartsWith("x must not be equal to 1 when y does not contain 3,4,5 and z is not equal to 3", ex.Message);
        }

        [Fact]
        public void WhenStringOperatorIsUsedErrorMessageMakesSense()
        {

            int x = 1;
            string y = "hello";
            int z = 4;

            var ex = Assert.Throws<ArgumentException>(() =>
            {
                x.If("x").IsEqualTo(1).And(y, "y").DoesNotStartWith("hmm").And(z, "z").IsNotEqualTo(3).ThenThrow<ArgumentException>();
            });


            Assert.StartsWith("x must not be equal to 1 when y does not start with text hmm and z is not equal to 3", ex.Message);
        }

    }
}
