using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.Change.Tracking
{
    public class ChangeTrackingKey 
    {
        private object primaryKey;
        private object trackableObject;
        public ChangeTrackingKey()
        {

        }
        public ChangeTrackingKey(object trackableObject, bool isPrimaryKey = false)
        {
            if (isPrimaryKey)
            {
                this.primaryKey = trackableObject;
            }
            else
            {
                this.trackableObject = trackableObject;
            }
        }

        
        /// <summary>
        /// Gets or sets the primary key based on the object
        /// </summary>
        public object PrimaryKey { get; set; }


        /// <summary>
        /// Gets or sets the object type as string 
        /// </summary>
        public string ObjectType { get; set; }

        /// <summary>
        /// Gets or sets the trackable object 
        /// </summary>
        public object TrackableObject { get; set; }
    }
}
