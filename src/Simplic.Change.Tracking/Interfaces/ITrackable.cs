﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.Change.Tracking
{

    public interface ITrackable
    {
        /// <summary>
        /// Gets and sets the state if the poco is trackable
        /// </summary>
        bool IsTrackable { get; set; }
        
        /// <summary>
        /// Gets or sets the snapshot
        /// </summary>
        object Snapshot { get; set; }

        /// <summary>
        /// Gets or sets the crud type - 0 = insert, 1 = update, 2 = delete
        /// </summary>
        CrudType CrudType { get; set; }
        
    }
}
