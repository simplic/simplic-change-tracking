using System.Collections.Generic;

namespace Simplic.Change.Tracking
{
    public interface IRequestChangeService : IRequestChangeRepository
    {
        string DetailedCompare<T>(T oldValue, T newValue);
    }
}
