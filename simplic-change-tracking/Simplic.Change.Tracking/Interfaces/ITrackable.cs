using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.Change.Tracking
{

    public interface ITrackable
    {
        bool IsTrackable { get; set; }
    }
}
