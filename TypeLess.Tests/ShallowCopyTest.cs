using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using TypeLess;

namespace TypeLess.Tests
{
    public class ClassWithPrivateCtor {
        private ClassWithPrivateCtor ()
	    {

	    }

        public static ClassWithPrivateCtor CreateNew() {
            return new ClassWithPrivateCtor();
        }
    }

    public class ClassWithPublicProperties {
        public int IntProp { get; set; }
        public string StringProp { get; set; }
    }

    public class ClassWithPrivateProperties
    {
        private int IntProp { get; set; }
        private string StringProp { get; set; }

        public void SetValues(int iProp, string sProp) {
            this.IntProp = iProp;
            this.StringProp = sProp;
        }

        public int GetIntProp() {
            return IntProp;
        }

        public string GetStringProp() {
            return StringProp;
        }
    }

    public class ClassWithFields {
        private int _intProp;
        private string _stringProp;

        public void SetProps(int iProp, string sProp) {
            _intProp = iProp;
            _stringProp = sProp;
        }

        public int GetIntProp() { return _intProp; }
        public string GetStringProp() { return _stringProp; }
    }

    public class ClassWithObjectProperties {

        public ClassWithFields OtherObj { get; set; }

    }

    public class ClassWithObjectFields
    {
         private ClassWithFields _OtherObj;

         public void SetValue(ClassWithFields val) {
             _OtherObj = val;
         }

         public ClassWithFields GetValue() {
             return _OtherObj;
         }

    }

    public class ClassWithArray {
        public ClassWithFields[] Arr { get; set; }

        public static ClassWithArray New() {
            var c = new ClassWithArray();
            c.Arr = new ClassWithFields[0];
            return c;
        }

    }

    public class ClassWithEnumerable
    {
        public IEnumerable<ClassWithFields> List { get; set; }

        public static ClassWithEnumerable New()
        {
            var c = new ClassWithEnumerable();
            c.List = new List<ClassWithFields>();
            return c;
        }

    }

    public class ClassWithGeneric
    {
        public int? intVal { get; set; }

        public static ClassWithGeneric New()
        {
            var c = new ClassWithGeneric();
            c.intVal = 34;
            return c;
        }
    }

    public class InheritedClass : ClassWithPublicProperties
    {
        
    }

    public class ShallowCopyTest
    {

        [Fact]
        public void CanCreateInstanceFromPrivateCtor() {
            ClassWithPrivateCtor c = ClassWithPrivateCtor.CreateNew();
            var newObj = c.ShallowCopy();
            Assert.NotNull(newObj);
        }

        [Fact]
        public void CanCopyPublicProperties() {
            var c = new ClassWithPublicProperties();
            c.IntProp = 2;
            c.StringProp = "string";

            var newC = c.ShallowCopy();
            Assert.Equal(c.IntProp, newC.IntProp);
            Assert.Equal(c.StringProp, newC.StringProp);
        }

        [Fact]
        public void CanCopyPrivateProperties()
        {
            var c = new ClassWithPrivateProperties();
            c.SetValues(2, "string");
           
            var newC = c.ShallowCopy();
            Assert.Equal(c.GetIntProp(), newC.GetIntProp());
            Assert.Equal(c.GetStringProp(), newC.GetStringProp());
        }

        [Fact]
        public void CanCopyFields()
        {
            var c = new ClassWithFields();
            c.SetProps(2, "string");

            var newC = c.ShallowCopy();
            Assert.Equal(c.GetIntProp(), newC.GetIntProp());
            Assert.Equal(c.GetStringProp(), newC.GetStringProp());
        }

        [Fact]
        public void CanCopyGenericProperties()
        {
            var c = new ClassWithGeneric();
            var newC = c.ShallowCopy();
            Assert.Equal(c.intVal, newC.intVal);
        }

        [Fact]
        public void ObjectPropertiesAreNotCopied()
        {
            var c = new ClassWithObjectProperties();
            c.OtherObj = new ClassWithFields();
            
            var newC = c.ShallowCopy();
            Assert.Null(newC.OtherObj);
        }

        [Fact]
        public void ObjectFieldsAreNotCopied()
        {
            var c = new ClassWithObjectFields();
            c.SetValue(new ClassWithFields());

            var newC = c.ShallowCopy();
            Assert.Null(newC.GetValue());
        }

        [Fact]
        public void ObjectArraysAreNotCopied()
        {
            var c = ClassWithArray.New();
            var newC = c.ShallowCopy();
            Assert.Null(newC.Arr);
        }

        [Fact]
        public void ObjectEnumerablesAreNotCopied()
        {
            var c = ClassWithEnumerable.New();
            var newC = c.ShallowCopy();
            Assert.Null(newC.List);
        }

        [Fact]
        public void CanCopyInheritedProperties()
        {
            var c = new InheritedClass();
            c.IntProp = 2;
            c.StringProp = "string";
            var newC = c.ShallowCopy();

            Assert.Equal(c.IntProp, newC.IntProp);
            Assert.Equal(c.StringProp, newC.StringProp);
        }


        
    }

    
}
