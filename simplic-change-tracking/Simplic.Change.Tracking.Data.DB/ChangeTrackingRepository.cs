using Dapper;
using Simplic.Cache;
using Simplic.Data.Sql;
using Simplic.Sql;
using System;
using System.Collections.Generic;

namespace Simplic.Change.Tracking.Data.DB
{
    public class ChangeTrackingRepository : IChangeTrackingRepository
    {
        private ISqlService sqlService;
        public ChangeTrackingRepository(ISqlService sqlService)
           
        {
            this.sqlService = sqlService;
        }
        //Change later to Change_Tracking
        public string TableName => "ChangeTracking";




        public ChangeTracking Get(Int64 id)
        {
            return sqlService.OpenConnection((c) =>
            {
                return c.QueryFirstOrDefault<ChangeTracking>($"Select * From {TableName} where Ident = :Ident",
                    new { Ident = id });
            });

        }

        public IEnumerable<ChangeTracking> GetChanges(object primaryKey)
        {
            return sqlService.OpenConnection((c) =>
            {
                return c.Query<ChangeTracking>($"Select * From {TableName} where DataGuid = :primaryKey or  DataLong = :primaryKey or DataString = :primaryKey",
                    new {  primaryKey });
            });
        }

        public bool Save(ChangeTracking obj)
        {
            string sql = $"Insert into {TableName} ( JsonObject, DataGuid, CrudType, TableName, TimeStampChange, UserId, UserName)" +
                    $"Values ( :JsonObject, :DataGuid, :CrudType, :TableName, :TimeStampChange, :UserId, :UserName) ";

            sqlService.OpenConnection((c) =>
            {
                c.Execute(sql, new { JsonObject = obj.JsonObject, DataGuid = obj.DataGuid,
                CrudType = obj.Type, TableName = obj.TableName, TimeStampChange = obj.TimeStampChange, UserId = obj.UserId, UserName = obj.UserName});
            });
            return true;
        }

    }
}
