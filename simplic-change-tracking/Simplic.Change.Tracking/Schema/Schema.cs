using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.Change.Tracking.Schemas
{
    public class Schema
    {   
        /// <summary>
        /// Get or sets the properties where the key is string and the value is the property schema 
        /// </summary>
        public IDictionary<string, PropertySchema> Properties { get; set; } = new Dictionary<string, PropertySchema>();
    }
}
