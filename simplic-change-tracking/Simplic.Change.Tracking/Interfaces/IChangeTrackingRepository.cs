using Simplic.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.Change.Tracking
{
    public interface IChangeTrackingRepository 
    {
        bool Save(ChangeTracking obj);
        ChangeTracking Get(Int64 id);
        IEnumerable<ChangeTracking> GetChanges(object primaryKey);
        
    }
}
