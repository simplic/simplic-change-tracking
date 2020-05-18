using Dapper;
using Simplic.Cache;
using Simplic.Data.Sql;
using Simplic.Sql;
using System;

namespace Simplic.Change.Tracking.Data.DB
{
    public class RequestChangeRepository : IRequestChangeRepository
    {
        private ISqlService sqlService;
        public RequestChangeRepository(ISqlService sqlService)
           
        {
            this.sqlService = sqlService;
        }
        public string TableName => "ChangeTracking";




        public RequestChange Get(Int64 id)
        {
            return sqlService.OpenConnection((c) =>
            {
                return c.QueryFirstOrDefault<RequestChange>($"Select * From {TableName} where Ident = :Ident",
                    new { Ident = id });
            });

        }

        public bool Save(RequestChange obj)
        {
            string sql = $"Insert into {TableName} ( JsonObject, DataGuid, CrudType, TableName, TimeStampChange, UserId)" +
                    $"Values ( :JsonObject, :DataGuid, :CrudType, :TableName, :TimeStampChange, :UserId) ";

            sqlService.OpenConnection((c) =>
            {
                c.Execute(sql, new { JsonObject = obj.JsonObject, DataGuid = obj.DataGuid,
                CrudType = obj.Type, TableName = obj.TableName, TimeStampChange = obj.TimeStampChange, UserId = obj.UserId});
            });
            return true;
        }

    }
}
