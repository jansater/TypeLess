using System;
namespace RS.Assert
{
    public interface IAssertion
    {
        bool IsValid { get; }
        string ToString();
    }
}
