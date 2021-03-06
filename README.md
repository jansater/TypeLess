TypeLess 
=========
##More code - less typing##
###No dependencies | Portal library | Easy to extend with your own validations###
###Licensed under MIT, feel free to use in commercial projects###

###Available on nuget: PM> Install-Package TypeLess###


**What problem/annoyance is this trying to solve**
``` c#
public void Login(SomeDTO data) {
    if (data == null) {
        throw new ArgumentNullException("Data is required");
    }
    
    if (data.Email == null) {
        throw new ArgumentNullException("Email is required");
    }
    
    if (NotValidEmail(data.Email)) {
        throw new ArgumentException("Email is not valid");
    }
    
    if (data.Email.Length > 5) {
        throw new ArgumentException("Email must be shorter than 6 characters")
    }
    
    ...
}
```

####If you are like me then writing that is tedious!####

**Would be easier to write this**
``` c#
using TypeLess;
...
public SomeMethod(SomeDTO input)
{
    input.If("Input").IsNull.ThenThrow();
    input.Email.If("Email").IsNull.IsNotValidEmail.ThenThrow();
    input.Name.If("Name").IsNull.IsEmptyOrWhitespace.ThenThrow();
    input.UserId.If("User id").IsSmallerThan(0).ThenThrow();
    
    ...
}
```

**Or if you prefer to put validation in the class where it belongs**
``` c#
using TypeLess;
...
public class SomeDTO {
    public string Name { get; set; }
    public string Email { get; set; }
    public int UserId { get; set; }

    public ObjectAssertion IsInvalid() {
        return ObjectAssertion.New(
            Name.If().IsNull.IsNotValidEmail,
            Email.If().IsNull.IsEmptyOrWhitespace,
            UserId.If().IsSmallerThan(0));
    }
}

...

public void Login(SomeDTO input)
{
    input.If().IsInvalid.ThenThrow();
}
```
####How about this case?####
```
s1.If("s1").IsNull.ThenThrow();
s2.If("s2").IsNull.ThenThrow();
s3.If("s3").IsNull.ThenThrow();
```
**If you need to check the same predicates on multiple objects like above then you can do this**
```
s1.If("s1").Or(s2, "s2").Or(s3, "s3").IsNull.ThenThrow().Otherwise(() => {...});
```

**If you need to you can also group different types like this**
```
string s1 = "some string";
double d1 = 0;
s1.If("s1").IsNull.Or(d1.If("d1").IsGreaterThan(0)).ThenThrow();
```

**And of course you can use multiple checks as in the very unreal example below**
```
d1.If("1").Or(d2, "2").Or(d3, "3").IsSmallerThan(5).IsGreaterThan(0).ThenThrow();
```

####And that is what you do with TypeLess! Nothing more, nothing (but hopefully) less####

**Example of output:**
``` c#
string email = "some text";
email.If("Email")
  .IsNull
  .IsNotValidEmail
  .IsLongerThan(5).ThenThrow();
```

- *Example exception debug: Email must be a valid email address and must be shorter than 6 characters at SomeMethod, line number 27 in file Asserts.cs*
- *Example exception not debug: Email must be a valid email address and must be shorter than 6 characters*

**or if the parameter name is not important in the exception output you can leave it out**
``` c#
email.If().IsNull.IsNotValidEmail.ThenThrow();
```

**here is how you use custom error message (you can use &lt;name&gt; anywhere in the text to include the parameter name)**
``` c#
email.If("email").IsNull.IsNotValidEmail.ThenThrow("<name> was not a valid email address");
```

**and of course you can throw custom exceptions**
``` c#
email.If("email").IsNull.IsNotValidEmail.ThenThrow<SomeException>("<name> was not a valid email address");
```

####Use with normal if statement####
``` c#
var precondition = email.If().IsNull.IsNotValidEmail;
if (!precondition.True) {
    //Get errors
    string errors = precondition.ToString();
}
```
Another if statement example

``` c#
var angle = 345;

if (angle.If()
 .IsWithin(315, 360)
 .Or(angle).IsWithin(0, 45)
 .Or(angle).IsWithin(135, 225).True) 
{
 ...
}
else {...}

or this would produce the same results

if (angle.If()
 .IsWithin(315, 360)
 .IsWithin(0, 45)
 .IsWithin(135, 225).True) 
{
 ...
}
else {...}

or

angle.If()
 .IsWithin(315, 360)
 .IsWithin(0, 45)
 .IsWithin(135, 225).Then(a => {...}).Otherwise(a => {...});

```
####Custom validation with lambda expresions####
``` c#
double a=1.0,b=2.0,c=3.0;
a.If().IsFalse(x => x > b && x < c, "a must be between b and c").ThenThrow();
```
- *Example output: Double a must be between b and c*

####Short circuit validation (default for isNull check)####
``` c#
string email = "some text";
  email.If("Email")
    .IsNotValidEmail.StopIfNotValid
    .IsLongerThan(5).ThenThrow();
```
- *Example output: Email must be a valid email address*

####Combine validations from multiple assertions with custom separator####
``` c#
DateTime d1 = new DateTime(2014,05,01);
DateTime d2 = new DateTime(2014,05,10);
string s1 = "abc";

var ifDateNotValid = new DateTime(2014, 05, 24).If().IsNotWithin(d1, d2);
var ifStringNotValid = s1.If("abc").IsShorterThan(4);

ifDateNotValid.Or(ifStringNotValid, "<br />").ThenThrow();
```
- *Example output: DateTime must be within 2014-05-01 00:00:00 and 2014-05-10 00:00:00 <br /> abc must be longer than 3 characters*

####How to add your own validation code on top of TypeLess####
Adding your own checks is easy. Just create an extension method for the assertion type you are interested in like this
``` c#
public static class PersonalNumber
{
    public static IStringAssertion IsNotValidPersonalNumber(this IStringAssertionU source) {
        source.Extend(x =>
        {
            return AssertResult.New(!Luhn.IsValid(x.ToIntArray()), Resources.IsNotValidPersonalNumber);
        });
        return (IStringAssertion)source;
    }
}
```
This method extends the IStringAssertionU interface with a swedish personal number check for strings according to the
Luhn algorithm. Note the U at the end of the interface and that it is not included in the return type. This is simply because the method should be available directly after the If() statement and not only after other assertions have been made. The Extend method will not show up in code completion but its there and it expects a function that receives the string (value) being checked and returns an AssertResult that takes the condition followed by the error message. Use &lt;name&gt; in the error message for replacement with parameter name

**The following types can be extended**
- ITimeSpanAssertionU / ITimeSpanAssertion
- IStringAssertionU / IStringAssertion / IRegexAssertion
- INumberAssertionU<T> / INumberAssertion<T>
- INullableAssertionU<T> / INullableAssertion<T>
- IMixedTypeAssertionU<T, U> / IMixedTypeAssertion<T, U>
- IEnumerableAssertionU / IEnumerableAssertion
- IDictionaryAssertionU<T1, T2> / IDictionaryAssertion<T1, T2>
- IDateTimeAssertionU / IDateTimeAssertion
- IClassAssertionU<T> / IClassAssertion<T>
- IBoolAssertionU / IBoolAssertion
- IAssertionU / IAssertion
- IAssertionU<T> / IAssertion<T> / IAssertionOW<T>

###Features:###
- Chain validation checks 
- Short circuit validation 
- Built as portable library
- Throw or get errors as text 
- Merge multiple property validations into a single validation message
- Examines stack details to return row and file information when running in debug mode
- Easy to extend. Its built on extension methods, add your own extensions to extend with new validations.
- Throw your own exception types
- Supports nullable types
- Chain complex property validations
- Re-use predicates on other objects
- Can return number of validation errors
- Kind of fluent...
- Supports English and Swedish error messages (controlled by current culture) 

###Available (predefined) checks:###
- IsNull
- IsEmpty
- IsTrue
- IsFalse
- IsEmpty
- ContainsLessThan x elements
- ContainsMoreThan x elements
- IsZero
- IsNotEqualTo
- IsEqualTo
- IsSmallerThan
- IsGreaterThan
- IsPositive
- IsNegative
- IsEmptyOrWhitespace
- IsNotValidEmail
- IsShorterThan
- IsLongerThan
- DoesNotContainAlphaChars
- DoesNotContainDigit
- IsNotWithin 
- DoesNotContain text
- DoesNotStartWith
- DoesNotEndWith
- SameDayAs + Not
- SameMonthAs + Not
- SameYearAs + Not
- SameHourAs + Not
- SameMinuteAs + Not
- SameSecondAs + Not
- SameWeekDayAs + Not
- Match + Not (regex matching)
- IsValidUrl + Not
- ContainsKey + Not

Extensions
- Shipping/IsValidIMO
- Sweden/IsValidPersonalNumber
- Us/IsValidPhoneNumber
- Us/IsValidSocialSecurityNumber
- Us/IsValidZipCode

###The framework target profile supports###
- .Net
- Windows 8
- Windows Phone 8.1
