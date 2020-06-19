using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace Simplic.Change.Tracking
{
    /// <summary>
    ///  change tracking will ignore this property or class
    /// </summary>
    public class IgnoreChangeTracking : Attribute
    {
        
        public IgnoreChangeTracking()
        {
            var a = new JsonIgnoreAttribute();    
            
        }
    }
}
