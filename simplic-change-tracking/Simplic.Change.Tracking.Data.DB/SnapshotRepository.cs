using Dapper;
using Simplic.Change.Tracking.Interfaces;
using Simplic.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.Change.Tracking.Data.DB
{
    public class SnapshotRepository : ISnapshotRepository
    {
        private ISqlService sqlService;
        public SnapshotRepository(ISqlService sqlService)

        {
            this.sqlService = sqlService;
        }
        //Change later to Change_Tracking
        public string TableName => "ChangeTracking_Snapshot";

        public bool Save(Snapshot snapshot)
        {
            string sql = $"Insert into {TableName} (ChangeTrackingId, JsonSnapshot) Values (:changeTrackingId, :jsonSnapshot) ";

            sqlService.OpenConnection((c) =>
            {
                c.Execute(sql, new
                {
                    changeTrackingId = snapshot.ChangeTrackingId ,
                    jsonSnapshot = snapshot.JsonSnapshot
                });
            });
            return true;
        }
    }
}
