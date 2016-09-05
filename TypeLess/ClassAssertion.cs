using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using TypeLess.DataTypes;
using TypeLess.Properties;
using System.Reflection;
using System.Linq;

namespace TypeLess
{
    public interface IClassAssertionU<T> : IAssertionU<T> where T : class
    {
        IClassAssertion<T> Or(T obj, string withName = null);
        IClassAssertion<T> IsInvalid { get; }
        IClassAssertion<T> IsNull { get; }
        IClassAssertion<T> IsNotNull { get; }
        new IClassAssertion<T> IsTrue(Func<T, bool> assertFunc, string errMsg = null);
        new IClassAssertion<T> IsFalse(Func<T, bool> assertFunc, string errMsg = null);
        new IClassAssertion<T> IsNotEqualTo(T comparedTo);
        new IClassAssertion<T> IsEqualTo(T comparedTo);
        IClassAssertion<T> IsNotEqualTo<S>(S comparedTo);
        IClassAssertion<T> IsEqualTo<S>(S comparedTo);
        IClassAssertion<T> IsPartOf(IEnumerable<T> enumerable);
        IClassAssertion<T> IsNotPartOf(IEnumerable<T> enumerable);
        /// <summary>
        /// Expect statements to test validity. This effects how error messages are added. In the normal case this property is false and 
        /// assertion methods are expected to test against a negative statement such as if x is smaller than or equal to 0 then throw e.
        /// This means that the error message is added when the statement is true. This property will inverse so that error messages are added
        /// when the statement is false so when you check x == 0 then the error message is added when x is not 0
        /// </summary>
        new IClassAssertion<T> EvalPositive { get; }

        /// <summary>
        /// Returns a positive if all property values in T match the same properties on object S otherwise negative. This requires that S contains a property of same type and name otherwise an
        /// expception will be thrown.
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <param name="item">The item.</param>
        /// <returns>IClassAssertion&lt;T&gt;.</returns>
        IClassAssertion<T> PropertyValuesMatch<S>(S item, params string[] ignoreProperties);

        /// <summary>
        /// Inverse of PropertyValuesMatch. The propertyChanged handler will be invoked for each not matching property and will contain values for
        /// the property name, expected value, actual value. It should return false if the changed property should be ignored otherwise true;
        /// expception will be thrown.
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <param name="item">The item.</param>
        /// <returns>IClassAssertion&lt;T&gt;.</returns>
        IClassAssertion<T> PropertyValuesDoNotMatch<S>(S item, Func<string, object, object, bool> propertyChanged = null, params string[] ignoreProperties);
    }

    public interface IClassAssertion<T> : IClassAssertionU<T>, IAssertion<T> where T : class
    {
        
    }

#if !DEBUG
    [DebuggerStepThrough]
#endif
    internal class ClassAssertion<T> : Assertion<T>, IClassAssertion<T> where T : class
    {
     
        public ClassAssertion(string s, T source, string file, int? lineNumber, string caller)
            :base (s, source, file, lineNumber, caller) {}

        public IClassAssertion<T> Combine(IClassAssertion<T> otherAssertion)
        {
            return (IClassAssertion<T>)base.Or(otherAssertion);
        }

        public new IClassAssertion<T> StopIfNotValid
        {
            get {
                base.IgnoreFurtherChecks = true;
                return this;
            }
        }

        public IClassAssertion<T> Or(T obj, string withName = null) {
            var ca = new ClassAssertion<T>(withName, obj, null, null, null);
            AddWithOr(ca);
            return this;
        }

        /// <summary>
        /// Determines whether the specified source is null. Automatically stops further processing if source is null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public IClassAssertion<T> IsNull
        {
            get
            {
                Extend(x =>
                {
                    if (x == null)
                    {
                        var temp = StopIfNotValid;
                        return AssertResult.New(true, Resources.IsNull);
                    }
                    return AssertResult.New(false);
                });
                return this;
            }

        }

        public IClassAssertion<T> IsNotNull
        {
            get
            {
                Extend(x =>
                {
                    if (x != null)
                    {
                        var temp = StopIfNotValid;
                        return AssertResult.New(true, Resources.IsNotNull);
                    }
                    return AssertResult.New(false);
                });
                return this;
            }

        }

        /// <summary>
        /// Make a call to this class IsValid method to determine whether the specified target object is valid. Normally used to define validation checks in for example dto's. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The target object.</param>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        /// <returns></returns>
        public IClassAssertion<T> IsInvalid
        {
            get {
                Extend(x =>
                {
                    if (x != null)
                    {
                        string errMsg = null;
                        int errCount = 0;
                        dynamic d = x;
                        try
                        {
                            ObjectAssertion classAssertions = null;
                            classAssertions = d.IsInvalid() as ObjectAssertion;
                            
                            if (classAssertions != null)
                            {
                                errMsg = classAssertions.ToString(out errCount);     
                            }
                        }
                        catch (RuntimeBinderException)
                        {
                            throw new System.MissingMemberException("You must define method public ObjectAssertion IsInvalid() {} in class " + typeof(T).Name);
                        }

                        return AssertResult.New(errCount > 0, errMsg);
                    }
                    else {
                        return AssertResult.New(x.If().IsNull.True, Name + " must not be null");
                    }
                    
                });
                return this;
            }

        }

        public new IClassAssertion<T> IsTrue(Func<T, bool> assertFunc, string errMsg = null)
        {
            return (IClassAssertion<T>)base.IsTrue(assertFunc, errMsg);
        }

        public new IClassAssertion<T> IsFalse(Func<T, bool> assertFunc, string errMsg = null)
        {
            return (IClassAssertion<T>)base.IsFalse(assertFunc, errMsg);
        }

        public new IClassAssertion<T> IsNotEqualTo(T comparedTo)
        {
            return (IClassAssertion<T>)base.IsNotEqualTo(comparedTo);
        }

        public new IClassAssertion<T> IsEqualTo(T comparedTo)
        {
            return (IClassAssertion<T>)base.IsEqualTo(comparedTo);
        }


        public IClassAssertion<T> IsNotEqualTo<S>(S comparedTo)
        {
            Extend(x =>
            {
                if (x == null)
                {
                    return AssertResult.New(comparedTo != null, Resources.IsNotEqualTo, comparedTo);
                }

                return AssertResult.New(!x.Equals(comparedTo), Resources.IsNotEqualTo, comparedTo == null ? "null" : comparedTo.ToString());
            });
            return this;
        }

        public IClassAssertion<T> IsEqualTo<S>(S comparedTo)
        {
            Extend(x =>
            {
                if (x == null)
                {
                    return AssertResult.New(comparedTo == null, Resources.IsEqualTo, comparedTo);
                }

                return AssertResult.New(x.Equals(comparedTo), Resources.IsEqualTo, comparedTo == null ? "null" : comparedTo.ToString());
            });
            return this;
        }

        public IClassAssertion<T> IsPartOf(IEnumerable<T> enumerable)
        {
            Extend(x =>
            {
                if (enumerable == null || x == null)
                {
                    return AssertResult.New(false, Resources.IsPartOf);
                }

                return AssertResult.New(enumerable.Contains(x), Resources.IsPartOf);
            });
            return this;
        }

        public IClassAssertion<T> IsNotPartOf(IEnumerable<T> enumerable)
        {
            Extend(x =>
            {
                if (enumerable == null || x == null)
                {
                    return AssertResult.New(true, Resources.IsNotPartOf);
                }

                return AssertResult.New(!enumerable.Contains(x), Resources.IsNotPartOf);
            });
            return this;
        }


        public new IClassAssertion<T> EvalPositive
        {
            get {
                return (IClassAssertion<T>)base.EvalPositive;
            }
        }
        
        internal bool PropertiesMatch(T source, object target, Func<string, object, object, bool> propertyChanged = null, params string[] ignoreProperties)
        {
            if (target == null && source != null)
            {
                return false;
            }
            else if (target == null && source == null)
            {
                return true;
            }
            else if (target != null && source == null)
            {
                return false;
            }

            if (ignoreProperties == null) {
                ignoreProperties = new string[0];
            }

            var sourceProperties = source.GetType().GetTypeInfo().DeclaredProperties.Where(x => !ignoreProperties.Any(y => String.Equals(y, x.Name, StringComparison.CurrentCultureIgnoreCase)));
            var targetProperties = target.GetType().GetTypeInfo().DeclaredProperties.Where(x => !ignoreProperties.Any(y => String.Equals(y, x.Name, StringComparison.CurrentCultureIgnoreCase))).ToList();

            var containsMatchingProps = sourceProperties.All(sourceProp =>
            {
                var targetProp = targetProperties.Where(y => y.Name == sourceProp.Name).FirstOrDefault();
                if (targetProp == null)
                {
                    throw new MissingMemberException("Missing member in target: " + sourceProp.Name);
                }
                
                var expectedValue = sourceProp.GetValue(source);
                var actualValue = targetProp.GetValue(target);

                if (sourceProp.PropertyType != targetProp.PropertyType)
                {
                    actualValue = Convert.ChangeType(actualValue, sourceProp.PropertyType);
                }

                if (expectedValue == null)
                {
                    if (actualValue == null)
                    {
                        return true;
                    }
                    else
                    {
                        if (propertyChanged != null) {
                            if (!propertyChanged(sourceProp.Name, expectedValue, actualValue))
                            {
                                //then ignore this diff
                                return true;
                            }
                        }
                        return false;
                    }
                }

                var equals = expectedValue.Equals(actualValue);

                if (!equals && propertyChanged != null) {
                    if (!propertyChanged(sourceProp.Name, expectedValue, actualValue))
                    {
                        //then ignore this diff
                        return true;
                    }
                }

                return equals;
            });

            return containsMatchingProps;
        }

        internal IEnumerable<Difference> DiffProperties(T source, object target, params string[] ignoreProperties)
        {
            var diff = new List<Difference>();

            if (target == null && source != null)
            {
                return diff;
            }
            else if (target == null && source == null)
            {
                return diff;
            }
            else if (target != null && source == null)
            {
                return diff;
            }

            if (ignoreProperties == null)
            {
                ignoreProperties = new string[0];
            }

            var sourceProperties = source.GetType().GetTypeInfo().DeclaredProperties.Where(x => !ignoreProperties.Any(y => String.Equals(y, x.Name, StringComparison.CurrentCultureIgnoreCase)));
            var targetProperties = target.GetType().GetTypeInfo().DeclaredProperties.Where(x => !ignoreProperties.Any(y => String.Equals(y, x.Name, StringComparison.CurrentCultureIgnoreCase))).ToList();
            
            foreach (var sourceProp in sourceProperties)
            {
                var targetProp = targetProperties.Where(y => y.Name == sourceProp.Name).FirstOrDefault();
                if (targetProp == null)
                {
                    throw new MissingMemberException("Missing member in target: " + sourceProp.Name);
                }
                
                var expectedValue = sourceProp.GetValue(source);
                var actualValue = targetProp.GetValue(target);

                if (sourceProp.PropertyType != targetProp.PropertyType)
                {
                    actualValue = Convert.ChangeType(actualValue, sourceProp.PropertyType);
                }

                if (expectedValue == null)
                {
                    if (actualValue == null)
                    {
                        continue;
                    }
                    else
                    {
                        diff.Add(new Difference()
                        {
                            Property = sourceProp.Name,
                            SourceValue = null,
                            TargetValue = actualValue
                        });
                    }
                }
                else {
                    var equals = expectedValue.Equals(actualValue);

                    if (!equals)
                    {
                        diff.Add(new Difference()
                        {
                            Property = sourceProp.Name,
                            SourceValue = expectedValue,
                            TargetValue = actualValue
                        });
                    }
                }
                
            }

            return diff;
        }

        public IClassAssertion<T> PropertyValuesMatch<S>(S item, params string[] ignoreProperties)
        {
            Extend(x =>
            {
                return AssertResult.New(PropertiesMatch(x, item, ignoreProperties: ignoreProperties), "Property values do not match"); 
            });
            return this;
        }


        public IClassAssertion<T> PropertyValuesDoNotMatch<S>(S item, Func<string, object, object, bool> propertyChanged = null, params string[] ignoreProperties)
        {
            Extend(x =>
            {
                return AssertResult.New(!PropertiesMatch(x, item, propertyChanged, ignoreProperties), "Property values match");
            });
            return this;
        }
    }
}
