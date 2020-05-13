﻿using Simplic.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.Change.Tracking
{
    public interface IRequestChangeRepository 
    {
        bool save(RequestChange obj);
        RequestChange get(Int64 id);
    }
}
