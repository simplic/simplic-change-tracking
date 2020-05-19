using Simplic.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.Change.Tracking
{
    public interface IRequestChangeRepository 
    {
        bool Save(RequestChange obj);
        RequestChange Get(Int64 id);
        IEnumerable<RequestChange> GetChanges(object primaryKey);
        
    }
}
