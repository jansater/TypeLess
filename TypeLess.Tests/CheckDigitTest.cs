using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeLess.Algorithms;
using Xunit;

namespace TypeLess.Tests
{
    public class CheckDigitTest
    {

        [Fact]
        public void WhenNumberIsValidLuhnThenReturnTrue()
        {
            Assert.True(Luhn.IsValid(new int[] { 8, 1, 1, 2, 1, 8, 9, 8, 7, 6 }));
            Assert.True(Luhn.IsValid(new int[] { 4, 9, 9, 2, 7, 3, 9, 8, 7, 1, 6 }));
            Assert.True(Luhn.IsValid(new int[] { 4, 9, 9, 2, 7, 3, 9, 8, 7, 1, 6 }));
            Assert.True(Luhn.IsValid(new int[] { 4, 2, 7, 2, 7, 2, 7, 9, 5, 4, 9, 8, 8, 1, 0, 2 }));

            Assert.True(Luhn.IsValid(new int[] { 5, 5, 7, 0, 1, 4, 8, 4, 0, 1, 1, 2, 7, 8, 1, 2 }));

            Assert.True(Luhn.IsValid(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 0 }));
            //American Express
            Assert.True(Luhn.IsValid(new int[] { 3, 7, 8, 2, 8, 2, 2, 4, 6, 3, 1, 0, 0, 0, 5 }));
            //American Express
            Assert.True(Luhn.IsValid(new int[] { 3, 7, 1, 4, 4, 9, 6, 3, 5, 3, 9, 8, 4, 3, 1 }));
            //American Express Corporate
            Assert.True(Luhn.IsValid(new int[] { 3, 7, 8, 7, 3, 4, 4, 9, 3, 6, 7, 1, 0, 0, 0 }));
            //Australian BankCard
            Assert.True(Luhn.IsValid(new int[] { 5, 6, 1, 0, 5, 9, 1, 0, 8, 1, 0, 1, 8, 2, 5, 0 }));
            //Diners Club
            Assert.True(Luhn.IsValid(new int[] { 3, 0, 5, 6, 9, 3, 0, 9, 0, 2, 5, 9, 0, 4 }));
            //Diners Club
            Assert.True(Luhn.IsValid(new int[] { 3, 8, 5, 2, 0, 0, 0, 0, 0, 2, 3, 2, 3, 7 }));
            //Discover
            Assert.True(Luhn.IsValid(new int[] { 6, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 7 }));
            //Discover
            Assert.True(Luhn.IsValid(new int[] { 6, 0, 1, 1, 0, 0, 0, 9, 9, 0, 1, 3, 9, 4, 2, 4 }));
            //JCB
            Assert.True(Luhn.IsValid(new int[] { 3, 5, 3, 0, 1, 1, 1, 3, 3, 3, 3, 0, 0, 0, 0, 0 }));
            //JCB
            Assert.True(Luhn.IsValid(new int[] { 3, 5, 6, 6, 0, 0, 2, 0, 2, 0, 3, 6, 0, 5, 0, 5 }));
            //Mastercard
            Assert.True(Luhn.IsValid(new int[] { 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 4, 4, 4, 4 }));
            //Mastercard
            Assert.True(Luhn.IsValid(new int[] { 5, 1, 0, 5, 1, 0, 5, 1, 0, 5, 1, 0, 5, 1, 0, 0 }));
            //Visa
            Assert.True(Luhn.IsValid(new int[] { 4, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }));
            //Visa
            Assert.True(Luhn.IsValid(new int[] { 4, 0, 1, 2, 8, 8, 8, 8, 8, 8, 8, 8, 1, 8, 8, 1 }));
            //Visa
            Assert.True(Luhn.IsValid(new int[] { 4, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 }));

            //Dankort (PBS)
            Assert.True(Luhn.IsValid(new int[] { 5,0,1,9,7,1,7,0,1,0,1,0,3,7,4,2 }));
            
            //Switch/Solo (Paymentech)
            Assert.True(Luhn.IsValid(new int[] { 6,3,3,1,1,0,1,9,9,9,9,9,0,0,1,6 }));
        }

        [Fact]
        public void WhenNumberIsNotValidLuhnThenReturnFalse()
        {
            Assert.False(Luhn.IsValid(new int[] { 4, 9, 9, 2, 7, 3, 9, 8, 7, 1, 7 }));
            Assert.False(Luhn.IsValid(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8 }));


        }

    }
}
