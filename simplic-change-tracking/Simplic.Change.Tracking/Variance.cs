using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.Change.Tracking
{
    public class Variance
    {
        /// <summary>
        /// Gets or sets the name of the property that changed
        /// </summary>
        public string Property { get; set; }

        /// <summary>
        /// Gets or sets the old value 
        /// </summary>
        public object OldValue { get; set; }

        /// <summary>
        /// Gets or sets the new value
        /// </summary>
        public object NewValue { get; set; }

        /// <summary>
        /// Gets or sets the attribute key which is used to get the translation
        /// </summary>
        public string LocalizationKey { get; set; }
    }
}
