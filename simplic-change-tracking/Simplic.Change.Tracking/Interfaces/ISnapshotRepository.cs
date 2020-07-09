using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.Change.Tracking.Interfaces
{
    public interface ISnapshotRepository 
    {
        bool Save(Snapshot snapshot);
    }
}
