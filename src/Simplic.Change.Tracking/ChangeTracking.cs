﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.Change.Tracking
{
    public class ChangeTracking
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
        /// Gets or sets the enum, either it is a insert = 0 or update = 1 or delete =2
        /// </summary>
        public CrudType CrudType { get; set; }

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
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the data id as guid
        /// </summary>
        public Guid? DataGuid { get; set; }

        /// <summary>
        /// Gets or sets the data id as long
        /// </summary>
        public long? DataLong { get; set; }

        /// <summary>
        /// Gets or sets the data id as string
        /// </summary>
        public string DataString { get; set; }

        /// <summary>
        /// Gets or set the user name as string 
        /// </summary>
        public string UserName { get; set; }
    }
}
