﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TypeLess.Tests
{
   
    public class ClassAsserts
    {
        public class ClassA {
            public int? PropertyA { get; private set; }
            public string PropertyB { get; set; }

            public ClassA(int? a, string b)
            {
                this.PropertyA = a;
                this.PropertyB = b;
            }
        }

        public class ClassB {
            public int? PropertyA { get; private set; }
            public string PropertyB { get; set; }
        
            public ClassB(int? a, string b)
            {
                this.PropertyA = a;
                this.PropertyB = b;
            }
        }

        public class ClassAWithMissingProp {
            public string PropertyB { get; set; }

            public ClassAWithMissingProp(string b)
            {
                this.PropertyB = b;
            }
        }

        public class ClassAWithWrongPropType {
            public int PropertyA { get; private set; }
            public string PropertyB { get; set; }

            public ClassAWithWrongPropType(int a, string b)
            {
                this.PropertyA = a;
                this.PropertyB = b;
            }
        }

        [Fact]
        public void WhenMissingPropertyInTargetObjectThenExceptionIsRaised()
        {
            var classA = new ClassA(null, "Test");
            var classAMissingProp = new ClassAWithMissingProp("Test");

            Assert.Throws<MissingMemberException>(() =>
            {
                classA.If().PropertyValuesMatch(classAMissingProp).ThenReturn(true);
            });
        }

        [Fact]
        public void WhenPropertyInTargetObjectHasWrongTypeThenExceptionIsRaised()
        {
            var classA = new ClassA(1, "Test");
            var classAWithWrongPropType = new ClassAWithWrongPropType(1, "Test");

            Assert.Throws<InvalidCastException>(() =>
            {
                classA.If().PropertyValuesMatch(classAWithWrongPropType).ThenReturn(true);
            });
        }

        [Fact]
        public void WhenAllPropertiesMatchThenPositiveIsReturned() {

            var classA = new ClassA(null, "Test");
            var classB = new ClassB(null, "Test");

            Assert.True(classA.If().PropertyValuesMatch(classB).ThenReturn(true));
            Assert.False(classA.If().PropertyValuesDoNotMatch(classB).ThenReturn(true));
        }

        [Fact]
        public void WhenNotAllPropertiesMatchThenNegativeIsReturned() {
            var classA = new ClassA(1, "Test");
            var classB = new ClassB(2, "Test");

            var match = classA.If().PropertyValuesMatch(classB).ThenReturn(true);
            Assert.False(match);

            match = classA.If().PropertyValuesDoNotMatch(classB).ThenReturn(true);
            Assert.True(match);
        }

        [Fact]
        public void WhenPropertyDiffIsIgnoredThenPositiveResultIsReturned() {
            var classA = new ClassA(1, "Test");
            var classB = new ClassB(2, "Test");

            Assert.False(classA.If().PropertyValuesDoNotMatch(classB, (prop, expected, actual) =>
            {
                return false;
            }).ThenReturn(true));
        }
    }
}