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
            string sql = $"Insert into {TableName} ( JsonObject, DataGuid, CrudType, TableName, TimeStampChange, UserId, UserName, DataType)" +
                    $"Values ( :JsonObject, :DataGuid, :CrudType, :TableName, :TimeStampChange, :UserId, :UserName, :DataType) ";

            sqlService.OpenConnection((c) =>
            {
                c.Execute(sql, new { JsonObject = obj.JsonObject, DataGuid = obj.DataGuid,
                CrudType = obj.CrudType, TableName = obj.TableName, TimeStampChange = obj.TimeStampChange, UserId = obj.UserId, UserName = obj.UserName,
                    DataType = obj.DataType
                });
            });
            return true;
        }

        public IEnumerable<ChangeTracking> GetChangesWithObject(ChangeTrackingKey poco, string dataColumn = "")
        {
            var infos = poco.PrimaryKey.GetType().GetProperties();
            ;
            object value = null;
            foreach (var info in infos)
            {

                switch (info.Name)
                {
                    case ("Guid"):
                        value = info.GetValue(poco.PrimaryKey);
                        dataColumn = "DataGuid";
                        break;
                    case ("Identifier"):
                        value = info.GetValue(poco.PrimaryKey);
                        dataColumn = "DataString";
                        break;
                    case ("Id"):
                    case ("Ident"):
                        value = info.GetValue(poco.PrimaryKey);
                        if (value is Guid guid)
                        {
                            dataColumn = "DataGuid";
                        }
                        else
                        {
                            dataColumn = "DataLong";
                        }

                        break;

                    default:
                        break;

                }
            }

                //PlaceHolder if the datacolumn is null
                if (dataColumn.Equals(""))
            {
                dataColumn = "DataGuid";
            }
            return sqlService.OpenConnection((c) =>
            {
                return c.Query<ChangeTracking>($"Select DataGuid, CrudType, TableName, TimeStampChange, UserId, DataLong, DataString, UserName, Ident From {TableName} where {dataColumn} = :primaryKey and DataType = :DataType",
                    new { primaryKey = value, DataType = poco.ObjectType }) ;
            });

        }

        public IEnumerable<ChangeTracking> GetAllDeleted(string tableName)
        {
            return sqlService.OpenConnection((c) =>
            {
                return c.Query<ChangeTracking>($"Select DataGuid, CrudType, TableName, TimeStampChange, UserId, DataLong, DataString, UserName, Ident From {TableName} where tableName = :tableName ",
                    new { tableName = tableName });
            });
        }
    }
}
