using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.Change.Tracking
{
    public class ChangeTrackingKey 
    {
        /// <summary>
        /// Gets or sets the primary key based on the object
        /// </summary>
        public object PrimaryKey { get; set; }


        /// <summary>
        /// Gets or sets the object type as string 
        /// </summary>
        public string ObjectType { get; set; }
    }
}
