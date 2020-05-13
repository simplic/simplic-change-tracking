using System.Collections.Generic;

namespace Simplic.Change.Tracking
{
    public interface IRequestChangeService : IRequestChangeRepository
    {
        string DetailedCompare<T>(T oldValue, T newValue);
        void TrackChange<TModel>(object obj, CrudType crudType, string tableName, object snapshot);
        bool IsTrackable<TModel>(object obj);
    }
}
