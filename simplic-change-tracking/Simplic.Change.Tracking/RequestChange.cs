using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.Change.Tracking
{
    public class RequestChange
    {
        /// <summary>
        /// Guid serves a the unique identitifer
        /// </summary>
        public Guid Guid { get; set; } = Guid.NewGuid();
        /// <summary>
        /// Json string thats stores the changes
        /// </summary>
        public string Json { get; set; }

        /// <summary>
        /// Gets or sets the old value - snapshot of the object
        /// </summary>
        public object OldValue { get; set; }

        /// <summary>
        /// Gets or sets the new value that could contain changes
        /// </summary>
        public object NewValue { get; set; }

        /// <summary>
        /// Gets or sets the enum, either it is a insert or update that means 1 or it is 0 thats means deleted
        /// </summary>
        public CrudType Type { get; set; }

        /// <summary>
        /// Gets or sets the table name of the object
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// Gets or sets the time stamp when the crud occured
        /// </summary>
        public DateTime TimeStamp { get; set; }

        /// <summary>
        /// Gets or sets the user id
        /// </summary>
        public Guid UserId { get; set; }
    }
}
