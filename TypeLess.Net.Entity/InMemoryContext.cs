using System.Collections;
using System.Data.Objects;
using TypeLess.Net.Entity.Contracts;

namespace TypeLess.Net.Entity
{
    public class InMemoryContext : IObjectContext
    {
         public InMemoryContext()
        {

        }

        public void Dispose()
        {

        }

        public int SaveChanges(SaveOptions options)
        {
            return 0;
        }

        public void ChangeObjectState<T>(T entity, System.Data.EntityState newState)
        {
            
        }

        public void ApplyOriginalValues<T>(T originalEntity) where T : class
        {
            
        }

        public void Refresh(RefreshMode refreshMode, IEnumerable colection)
        {

        }

        public void Refresh(RefreshMode refreshMode, object entity)
        {

        }


        public bool ProxyCreationEnabled
        {
            get
            {
                return false;
            }
            set
            {
                //no op
            }
        }


        
    }
}
