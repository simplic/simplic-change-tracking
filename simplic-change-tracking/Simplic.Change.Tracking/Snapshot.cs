using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.Change.Tracking
{
    public class Snapshot
    {
        /// <summary>
        /// Primary Key stored as long
        /// </summary>
        public long Ident { get; set; }

        /// <summary>
        /// Foreign Key to connect to change tracking table
        /// </summary>
        public long ChangeTrackingId { get; set; }

        /// <summary>
        /// Gets or sets the json snapshot
        /// </summary>
        public string JsonSnapshot { get; set; }
    }
}
