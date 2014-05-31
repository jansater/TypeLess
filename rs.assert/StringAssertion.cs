using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace TypeLess
{
    public interface IStringAssertionU : IAssertionU<string> {
        IStringAssertion IsNull { get; }
        IStringAssertion IsEmpty { get; }
        IStringAssertion IsEmptyOrWhitespace { get; }
        IStringAssertion IsNotValidEmail { get; }
        IStringAssertion DoesNotContainAlphaChars { get; }
        IStringAssertion DoesNotContainDigit { get; }
        new IStringAssertion IsEqualTo(String comparedTo);
        new IStringAssertion IsTrue(Func<string, bool> assertFunc, string msgIfFalse);
        new IStringAssertion IsFalse(Func<string, bool> assertFunc, string msgIfTrue);
        new IStringAssertion IsNotEqualTo(string comparedTo);
        
        IStringAssertion StopIfNotValid { get; }
        IStringAssertion IsShorterThan(int length);
        IStringAssertion IsLongerThan(int length);
        IStringAssertion DoesNotContain(string text);
        IStringAssertion DoesNotStartWith(string text);
        IStringAssertion DoesNotEndWith(string text);
        IStringAssertionU Or(string obj, string withName = null);

        IRegexAssertion Match(string regex, RegexOptions options = RegexOptions.None);
        IStringAssertion DoesNotMatch(string regex, RegexOptions options = RegexOptions.None);
    }

    public interface IStringAssertion : IStringAssertionU, IAssertion<string>
    {
        
    }

    public interface IRegexAssertion : IStringAssertion {
        /// <summary>
        /// Return result of group. Ex. $0 for the first group or ${group1} for named groups
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        string ThenReturnResultOf(string groupName);
    }

#if !DEBUG
    [DebuggerStepThrough]
#endif
    internal class StringAssertion : ClassAssertion<string>, IStringAssertion, IRegexAssertion
    {
        private List<StringAssertion> _childAssertions = new List<StringAssertion>();

        public StringAssertion(string s, string source, string file, int? lineNumber, string caller)
            : base(s, source, file, lineNumber, caller) { }

        internal new List<StringAssertion> ChildAssertions
        {
            get
            {
                return _childAssertions;
            }
        }

        public IStringAssertionU Or(string obj, string withName = null)
        {
            this.ChildAssertions.Add(new StringAssertion(withName, obj, null, null, null));
            return this;
        }

        public IStringAssertion Combine(IStringAssertion otherAssertion)
        {
            return (IStringAssertion)base.Combine(otherAssertion);
        }

        public new IStringAssertion StopIfNotValid
        {
            get
            {
                base.IgnoreFurtherChecks = true;
                return this;
            }
        }

        public new IStringAssertion IsNull
        {
            get
            {
                var x = base.IsNull;
                return this;
            }
        }

        public IStringAssertion IsEmpty
        {
            get
            {
                return this.IsEmpty();
            }
        }

        public IStringAssertion IsEmptyOrWhitespace
        {
            get
            {
                return this.IsEmptyOrWhitespace();
            }
        }

        public IStringAssertion IsNotValidEmail
        {
            get { return this.IsNotValidEmail(); }
        }

        public IStringAssertion DoesNotContainAlphaChars
        {
            get { return this.DoesNotContainAlphaChars(); }
        }

        public IStringAssertion DoesNotContainDigit
        {
            get { return this.DoesNotContainDigit(); }
        }

        public new IStringAssertion IsEqualTo(String comparedTo)
        {
            return (StringAssertion)AssertExtensions.IsEqualTo(this, comparedTo);
        }

        public IStringAssertion IsShorterThan(int length)
        {
            return StringAssertExtensions.IsShorterThan(this, length);
        }

        public IStringAssertion IsLongerThan(int length)
        {
            return StringAssertExtensions.IsLongerThan(this, length);
        }

        public IStringAssertion DoesNotContain(string text)
        {
            return StringAssertExtensions.DoesNotContain(this, text);
        }

        public IStringAssertion DoesNotStartWith(string text)
        {
            return StringAssertExtensions.DoesNotStartWith(this, text);
        }

        public IStringAssertion DoesNotEndWith(string text)
        {
            return StringAssertExtensions.DoesNotEndWith(this, text);
        }


        public new IStringAssertion IsTrue(Func<string, bool> assertFunc, string msgIfFalse)
        {
            return (IStringAssertion)base.IsTrue(assertFunc, msgIfFalse);
        }

        public new IStringAssertion IsFalse(Func<string, bool> assertFunc, string msgIfTrue)
        {
            return (IStringAssertion)base.IsFalse(assertFunc, msgIfTrue);
        }

        public new IStringAssertion IsNotEqualTo(string comparedTo)
        {
            return (IStringAssertion)base.IsNotEqualTo(comparedTo);
        }

        private Match _previousMatch;

        public IRegexAssertion Match(string regex, RegexOptions options = RegexOptions.None)
        {
            Extend(x =>
            {
                _previousMatch = Regex.Match(x, regex, options);

                if (_previousMatch.Success)
                {
                    return String.Format(CultureInfo.InvariantCulture, "must not match regular expression {0}", regex);
                }
                return null;
            });

            return this;
        }

        public IStringAssertion DoesNotMatch(string regex, RegexOptions options = RegexOptions.None)
        {
            Extend(x =>
            {
                _previousMatch = Regex.Match(x, regex, options);
                if (!_previousMatch.Success)
                {
                    return String.Format(CultureInfo.InvariantCulture, "must match regular expression {0}", regex);
                }
                return null;
            });

            return this;
        }

        public string ThenReturnResultOf(string groupName)
        {
            if (_previousMatch == null || !_previousMatch.Success) {
                return null;
            }

            return _previousMatch.Result(groupName);
        }

    }
}
