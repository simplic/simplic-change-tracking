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
        public ChangeTrackingDisplayName(string key)
        {
            this.key = key;
        }
        /// <summary>
        /// Is the general key
        /// </summary>
        public string Key 
        {
            get => key;
            set => key = value;
        }
        public string TargetWord { get; set; } = "";
    }
    public class Person
    {
        [ChangeTrackingDisplayName("hr_key", TargetWord ="")]
        public string Name { get; set; }
    }
}
