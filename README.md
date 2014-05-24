RS.Assert (Another pragmatic argument validation lib)
=========

**Example usage:**
```
string email = "some text";
email.If("Email")
  .IsNull()
  .IsNotValidEmail()
  .IsLongerThan(5).ThenThrow();
```

- *Example exception debug: Email must be a valid email address and must be shorter than 6 characters at SomeMethod, line number 27 in file Asserts.cs*
- *Example exception not debug: Email must be a valid email address and must be shorter than 6 characters*

or if the parameter name is not important in the exception output
```
email.If().IsNull().IsNotValidEmail().ThenThrow();
```
check if valid
```
var precondition = email.If().IsNull().IsNotValidEmail();
if (!precondition.IsValid) {
    //Get errors
    string errors = precondition.ToString();
}
```
more complex checks
```
double a=1.0,b=2.0,c=3.0;
a.If().IsFalse(x => x > b && x < c, "a must be between b and c").ThenThrow();
```
- *Example output: Double a must be between b and c*

short circuit validation (default for isNull check)
```
string email = "some text";
  email.If("Email")
    .IsNotValidEmail().StopIfNotValid()
    .IsLongerThan(5).ThenThrow();
```
- *Example output: Email must be a valid email address*

**Features:**
- Chain validation checks 
- Short circuit validation 
- Throw or get errors as text 
- Merge multiple property validations into a single validation message
- Examines stack details to return row and file information when running in debug mode
- Easy to extend. Its built on extension methods, add your own extensions to extend with new validations.
- Throw your own exception types
- Kind of fluent...

**Available checks:**
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

