using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.Change.Tracking
{
    /// <summary>
    /// For localization to set the name either to german or englisch
    /// </summary>
    public class ChangeTrackingDisplayName : Attribute
    {
        private string key;
        public ChangeTrackingDisplayName()
        {
        }
        /// <summary>
        /// Is the general key
        /// </summary>
        public string Key 
        {
            get;
            set;
        }
        //[ChangeTrackingDisplayName(Key = "hr_is_sunday", ValueConverter ="company_name_converter")] 
        public string ValueConverterName { get; set; }
        public string TargetWord { get; set; } = "";
    }

}
