using System;

namespace TypeLess.Net.Contracts
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
