using System.Collections.Generic;

namespace Simplic.Change.Tracking
{
    public interface IChangeTrackingService : IChangeTrackingRepository
    {
        string DetailedCompare<T>(T oldValue, T newValue);
        void TrackChange<TModel, TId>(object obj, CrudType crudType, string tableName, object snapshot, object primaryKey);
        bool IsTrackable<TModel>(object obj);
        TModel CreateDeepCopy<TModel>(object obj);

    }
}
