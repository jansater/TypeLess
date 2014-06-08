using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TypeLess.DataTypes;
using TypeLess.Properties;

namespace TypeLess.Extensions.Books
{
    public static class Isbn
    {
        

        private static string ISBN10Or13 = @"ISBN(-1(?:(0)|3))?:?\x20*(?(1)(?(2)(?:(?=.{13}$)\d{1,5}([ -])\d{1,7}\3\d{1,6}\3(?:\d|x)$)|(?:(?=.{17}$)97(?:8|9)([ -])\d{1,5}\4\d{1,7}\4\d{1,6}\4\d$))|(?(.{13}$)(?:\d{1,5}([ -])\d{1,7}\5\d{1,6}\5(?:\d|x)$)|(?:(?=.{17}$)97(?:8|9)([ -])\d{1,5}\6\d{1,7}\6\d{1,6}\6\d$)))";

        private static IStringAssertion IsNotValidISBN10(this IStringAssertionU source)
        {
            source.Extend(x =>
            {
                return AssertResult.New(!ValidIsbn10(x), Resources.IsNotValidISBN);
            });
            return (IStringAssertion)source;
        }

        private static IStringAssertion IsValidISBN10(this IStringAssertionU source)
        {
            source.Extend(x =>
            {
                return AssertResult.New(ValidIsbn10(x), Resources.IsValidISBN);
            });
            return (IStringAssertion)source;
        }

        private static bool ValidIsbn10(string isbn)
        {
            if (isbn == null)
            {
                return false;
            }

            isbn = isbn.ToUpper();

            if (!Regex.IsMatch(isbn, ISBN10Or13))
            {
                return false;
            }

            isbn = isbn.Replace("ISBN-13", String.Empty).Replace("ISBN-10", String.Empty).Replace("ISBN", String.Empty);

            isbn = isbn.TrimStart(':').Trim();

            // Remove non ISBN digits, then split into an array
            var chars = Regex.Replace(isbn, "[^0-9X]", "").ToCharArray().ToList();
            // Remove the final ISBN digit from `chars`, and assign it to `last`
            var last  = chars.Last();
            chars.RemoveAt(chars.Count - 1);
            var sum   = 0;
            var digit = 10;
            int check;
            char charCheck = default(char);

            if (chars.Count == 9)
            {
                // Compute the ISBN-10 check digit
                for (var i = 0; i < chars.Count; i++)
                {
                    sum += digit * Convert.ToInt32(chars[i]);
                    digit -= 1;
                }
                check = 11 - (sum % 11);
                if (check == 10)
                {
                    charCheck = 'X';
                }
                else if (check == 11)
                {
                    charCheck = '0';
                }
                else {
                    return false;
                }
            }
            else {
                return false;
            }

            return charCheck == last;
        }


        /*
 
if (regex.test(subject)) {
  // Remove non ISBN digits, then split into an array
  var chars = subject.replace(/[^0-9X]/g, "").split("");
  // Remove the final ISBN digit from `chars`, and assign it to `last`
  var last  = chars.pop();
  var sum   = 0;
  var digit = 10;
  var check;

  if (chars.length == 9) {
    // Compute the ISBN-10 check digit
    for (var i = 0; i < chars.length; i++) {
      sum += digit * parseInt(chars[i], 10);
      digit -= 1;
    }
    check = 11 - (sum % 11);
    if (check == 10) {
      check = "X";
    } else if (check == 11) {
      check = "0";
    }
  } else {
    // Compute the ISBN-13 check digit
    for (var i = 0; i < chars.length; i++) {
      sum += (i % 2 * 2 + 1) * parseInt(chars[i], 10);
    }
    check = 10 - (sum % 10);
    if (check == 10) {
      check = "0";
    }
  }

  if (check == last) {
    alert("Valid ISBN");
  } else {
    alert("Invalid ISBN check digit");
  }
} else {
  alert("Invalid ISBN");
}
         */
    }
}
