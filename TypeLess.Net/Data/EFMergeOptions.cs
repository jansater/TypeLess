namespace TypeLess.Net.Data
{
    /// <summary>
    /// This enumeration mirrors the EF MergeOptions enumeration. We can't use the latter since we are using a portable project type
    /// </summary>
    public enum EFMergeOptions {
        // Summary:
        //     Objects that already exist in the object context are not loaded from the
        //     data source. This is the default behavior for queries or when calling the
        //     System.Data.Objects.DataClasses.EntityCollection<TEntity>.Load(System.Data.Objects.MergeOption)
        //     method on an System.Data.Objects.DataClasses.EntityCollection<TEntity>.
        AppendOnly = 0,
        //
        // Summary:
        //     Objects are always loaded from the data source. Any property changes made
        //     to objects in the object context are overwritten by the data source values.
        OverwriteChanges = 1,
        //
        // Summary:
        //     In the Entity Framework version 3.5 SP1, the unmodified properties of objects
        //     in the object context are overwritten with server values.
        PreserveChanges = 2,
        //
        // Summary:
        //     Objects are maintained in a System.Data.EntityState.Detached state and are
        //     not tracked in the System.Data.Objects.ObjectStateManager.
        NoTracking = 3,
    }
    
}
