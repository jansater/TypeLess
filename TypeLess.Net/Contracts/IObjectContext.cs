using System;
using System.Collections;
using System.Data.Objects;

namespace TypeLess.Net.Contracts
{
    public interface IObjectContext : IDisposable
    {
        int SaveChanges(SaveOptions options);
        void ChangeObjectState<T>(T entity, System.Data.EntityState newState);
        void ApplyOriginalValues<T>(T originalEntity) where T : class;
        bool ProxyCreationEnabled { get; set; }
        void Refresh(RefreshMode refreshMode, IEnumerable colection);
        void Refresh(RefreshMode refreshMode, object entity);

        //IObjectSet<T...> EntityName { get; }
        
    }
}
