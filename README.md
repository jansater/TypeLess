RS.Assert (Another pragmatic argument validation lib)
=========

###Available on nuget: PM> Install-Package RS.Assert###

**What problem/annoyance is this trying to solve**
``` c#
public void Login(SomeDTO data) {
    if (data != null) {
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
using RS.Assert;
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
using RS.Assert;
...
public class SomeDTO {
    public string Name { get; set; }
    public string Email { get; set; }
    public int UserId { get; set; }

    public IEnumerable<IAssertion> IsInvalid() {
        yield return Name.If().IsNull.IsNotValidEmail;
        yield return Email.If().IsNull.IsEmptyOrWhitespace;
        yield return UserId.If().IsSmallerThan(0);
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
If.AnyOf(s1, "s1").Or(s2, "s2").Or(s3, "s3").IsNull.ThenThrow();
```
**And of course you can use multiple checks as in the very unreal example below**
```
If.AnyOf(d1, "1").And(d2, "2").And(d3, "3").IsSmallerThan(5).IsLargerThan(0).ThenThrow();
```


####And that is what you do with RS.Assert! Nothing more, nothing less####

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

or if the parameter name is not important in the exception output
``` c#
email.If().IsNull.IsNotValidEmail.ThenThrow();
```
####Use with normal if statement####
``` c#
var precondition = email.If().IsNull.IsNotValidEmail;
if (!precondition.IsValid) {
    //Get errors
    string errors = precondition.ToString();
}
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

####Combine validation output from multiple properties####
``` c#
DateTime d1 = new DateTime(2014,05,01);
DateTime d2 = new DateTime(2014,05,10);
string s1 = "abc";

var ifDateNotValid = new DateTime(2014, 05, 24).If().IsNotWithin(d1, d2);
var ifStringNotValid = s1.If("abc").IsShorterThan(4);

ifDateNotValid.Combine(ifStringNotValid).ThenThrow();
```
- *Example output: DateTime must be within 2014-05-01 00:00:00 and 2014-05-10 00:00:00. abc must be longer than 3 characters*

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
- IsLargerThan
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

###The framework target profile supports###
- .Net
- Windows 8
- Windows Phone 8.1
