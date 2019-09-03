using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeLess;
using Xunit;

namespace TypeLess.Tests
{
    public class AssignmentSyntax
    {
        public string _obj;
        public string Prop { get; private set; }

        public AssignmentSyntax(string obj)
        {
            obj.AssignTo(ref _obj);
        }

        public AssignmentSyntax(string obj, string prop)
        {
            obj.AssignTo(ref _obj);
            Prop = prop.CheckedAssignment();
            
        }

    }

    public class AssignmentTests {
        [Fact]
        public void AssignmentSuccess() {

            var a = new AssignmentSyntax("hej");
            Assert.Equal("hej", a._obj);

        }

        [Fact]
        public void AssignmentFailure()
        {

            Assert.Throws<AssignmentException>(() =>
            {
                new AssignmentSyntax(null);
            });
            
        }

        [Fact]
        public void AssignmentPropertySuccess()
        {

            var a = new AssignmentSyntax("hej", "då");
            Assert.Equal("då", a.Prop);

        }

        [Fact]
        public void AssignmentPropertyFailure()
        {
            Assert.Throws<AssignmentException>(() =>
            {
                new AssignmentSyntax("sdfds", null);
            });
        }
    }
}
