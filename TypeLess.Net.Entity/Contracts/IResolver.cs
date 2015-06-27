using System;
using System.Collections.Generic;

namespace TypeLess.Net.Entity.Contracts
{
    public interface IResolver 
    {
        T Resolve<T>();
        T Resolve<T>(Type type);
        //IRepository<T> ResolveRepository<T>() where T : class;
    }
}
