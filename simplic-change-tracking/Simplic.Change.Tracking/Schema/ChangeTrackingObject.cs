using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.Change.Tracking.Schemas
{
    public class ChangeTrackingObject
    {
        /// <summary>
        /// Gets or sets the schema which contains the dictionary 
        /// </summary>
        public Schema Schema { get; set; }

        /// <summary>
        /// Gets or sets the data which contains the json string 
        /// </summary>
        public JObject Data { get; set; }
    }
}
