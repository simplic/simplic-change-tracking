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
        /// Ident serves as the unique identitifer 
        /// </summary>
        public Int64 Ident { get; set; } 
        
        /// <summary>
        /// Json string thats stores the changes
        /// </summary>
        public string JsonObject { get; set; }


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
        public DateTime TimeStampChange { get; set; }

        /// <summary>
        /// Gets or sets the user id
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the data id
        /// </summary>
        public Guid DataGuid { get; set; }
    }
}
