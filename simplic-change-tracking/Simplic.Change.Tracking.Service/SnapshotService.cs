using Simplic.Change.Tracking.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.Change.Tracking.Service
{
    public class SnapshotService : ISnapshotService
    {
        ISnapshotRepository snapshotRepository;
        

        /// <summary>
        /// Constructor for dependency injection
        /// </summary>
        /// <param name="requestChangeRepository"></param>
        /// <param name="sessionService"></param>
        public SnapshotService(ISnapshotRepository snapshotRepository)
        {
            this.snapshotRepository = snapshotRepository;
        }

        public bool Save(Snapshot snapshot)
        {
            return snapshotRepository.Save(snapshot);
        }
    }
}
