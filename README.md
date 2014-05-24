RS.Assert (Another pragmatic argument validation lib)
=========

<strong>Example usage:</strong><br />
<code>
email.If("Email")
      .IsNull()
      .IsNotValidEmail().ThenThrow();
</code><br />
Example exception debug: Email must be a valid email address at SomeMethod, line number 27 in file Asserts.cs<br />
Example exception not debug: Email must be a valid email address<br />

//or if the parameter name is not important in the exception output<br />
<code>
email.If().IsNull().IsNotValidEmail().ThenThrow();
</code><br />
//check if valid
<code>
var precondition = email.If().IsNull().IsNotValidEmail();
if (!precondition.IsValid) {
    //Get errors
    string errors = precondition.ToString();
}
</code><br />

//more complex checks
<code>
double a=1.0,b=2.0,c=3.0;
a.If().IsFalse(x => x > b && x < c, "a must be between b and c").ThenThrow();
</code>
Example output: Double a must be between b and c at SomeMethod, line number 28 in file Asserts.cs


<strong>Features:</strong>
- Chain validation checks 
- Short circuit validation 
- Throw or get errors as text 
- Merge multiple property validations into a single validation message
- Examines stack details to return row and file information when running in debug mode
- Easy to extend. Its built on extension methods, add your own extensions to extend with new validations.
- Throw your own exception types
- Kind of fluent...

<strong>Available checks:</strong>
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
- IsNotWithin 

