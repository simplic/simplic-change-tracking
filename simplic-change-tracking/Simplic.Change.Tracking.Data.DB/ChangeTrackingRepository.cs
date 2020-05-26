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
            if(primaryKey is Guid guid)
            {
                return sqlService.OpenConnection((c) =>
                {
                    return c.Query<ChangeTracking>($"Select DataGuid, CrudType, TableName, TimeStampChange, UserId, DataLong, DataString, UserName, Ident From {TableName} where DataGuid = :primaryKey",
                        new { primaryKey = guid });
                });
            }
            return sqlService.OpenConnection((c) =>
            {
                return c.Query<ChangeTracking>($"Select DataGuid, CrudType, TableName, TimeStampChange, UserId, DataLong, DataString, UserName, Ident From {TableName} where DataLong = :primaryKey or DataString = :primaryKey",
                    new { primaryKey = primaryKey });
            });
        }

        public byte[] GetJsonAsByteArray(long ident)
        {
            return sqlService.OpenConnection((c) =>
            {
                return c.QuerySingleOrDefault<byte[]>($"Select JsonObject From {TableName} where Ident = :primaryKey",
                    new { primaryKey = ident });
            });
        }

        public bool Save(ChangeTracking obj)
        {
            string sql = $"Insert into {TableName} ( JsonObject, DataGuid, CrudType, TableName, TimeStampChange, UserId, UserName)" +
                    $"Values ( :JsonObject, :DataGuid, :CrudType, :TableName, :TimeStampChange, :UserId, :UserName) ";

            sqlService.OpenConnection((c) =>
            {
                c.Execute(sql, new { JsonObject = obj.JsonObject, DataGuid = obj.DataGuid,
                CrudType = obj.CrudType, TableName = obj.TableName, TimeStampChange = obj.TimeStampChange, UserId = obj.UserId, UserName = obj.UserName});
            });
            return true;
        }

        public IEnumerable<ChangeTracking> GetChangesWithObject(object poco, string dataColumn = "")
        {
            return sqlService.OpenConnection((c) =>
            {
                return c.Query<ChangeTracking>($"Select DataGuid, CrudType, TableName, TimeStampChange, UserId, DataLong, DataString, UserName, Ident From {TableName} where {dataColumn} = :primaryKey ",
                    new { primaryKey = poco });
            });

        }

    }
}
