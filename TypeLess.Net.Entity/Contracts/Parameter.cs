using System;

namespace TypeLess.Net.Entity.Contracts
{
    public class Parameter
    {
        public String Name { get; set; }
        public object Value { get; set; }

        public Parameter()
        {

        }

        public Parameter(string name, object value)
        {
            this.Name = name;
            this.Value = value;
        }
    }
}
