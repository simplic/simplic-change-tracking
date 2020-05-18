using System.Collections.Generic;

namespace Simplic.Change.Tracking
{
    public interface IRequestChangeService : IRequestChangeRepository
    {
        string DetailedCompare<T>(T oldValue, T newValue);
        void TrackChange<TModel, TId>(object obj, CrudType crudType, string tableName, object snapshot, object primaryKey);
        bool IsTrackable<TModel>(object obj);
        TModel CreateDeepCopy<TModel>(object obj);

    }
}
