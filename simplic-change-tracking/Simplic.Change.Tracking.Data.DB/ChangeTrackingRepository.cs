using Dapper;
using Simplic.Cache;
using Simplic.Data.Sql;
using Simplic.Sql;
using System;
using System.Collections.Generic;
using System.Text;

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



        /// <summary>
        /// Get the change tracking object based on id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ChangeTracking Get(long id)
        {
            return sqlService.OpenConnection((c) =>
            {
                return c.QueryFirstOrDefault<ChangeTracking>($"Select * From {TableName} where Ident = :Ident",
                    new { Ident = id });
            });

        }

        /// <summary>
        /// Get the changes based on a primaryKey, it differentiate between guid, long, int and string
        /// </summary>
        /// <param name="primaryKey"></param>
        /// <returns></returns>
        public IEnumerable<ChangeTracking> GetChanges(object primaryKey)
        {
            if (primaryKey is Guid guid)
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

        /// <summary>
        /// Gets the json as an array of bytes based on a long 
        /// </summary>
        /// <param name="ident"></param>
        /// <returns></returns>
        public byte[] GetJsonAsByteArray(long ident)
        {
            return sqlService.OpenConnection((c) =>
            {
                return c.QuerySingleOrDefault<byte[]>($"Select JsonObject From {TableName} where Ident = :primaryKey",
                    new { primaryKey = ident });
            });
        }

        /// <summary>
        /// Returns true if the save method is executed and the data is stored in the database
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Save(ChangeTracking obj)
        {
            string sql = $"Insert into {TableName} (Ident, JsonObject, DataGuid, CrudType, TableName, TimeStampChange, UserId, UserName, DataType)" +
                    $"Values (:Ident, :JsonObject, :DataGuid, :CrudType, :TableName, :TimeStampChange, :UserId, :UserName, :DataType) ";

            sqlService.OpenConnection((c) =>
            {
                c.Execute(sql, new
                {
                    Ident = obj.Ident,
                    JsonObject = Encoding.UTF8.GetBytes(obj.JsonObject),
                    DataGuid = obj.DataGuid,
                    CrudType = obj.CrudType,
                    TableName = obj.TableName,
                    TimeStampChange = obj.TimeStampChange,
                    UserId = obj.UserId,
                    UserName = obj.UserName,
                    DataType = obj.DataType
                }); ;
            });
            return true;
        }

        /// <summary>
        /// Get Changes based on an object 
        /// </summary>
        /// <param name="poco"></param>
        /// <param name="dataColumn"></param>
        /// <returns></returns>
        public IEnumerable<ChangeTracking> GetChangesWithObject(ChangeTrackingKey poco, string dataColumn = "")
        {


            var primaryKey = poco.PrimaryKey;
            if (primaryKey is Guid)
            {
                dataColumn = "DataGuid";
            }
            if (primaryKey is string)
            {
                dataColumn = "DataString";
            }
            if (primaryKey is int || primaryKey is long)
            {
                dataColumn = "DataLong";
            }


            return sqlService.OpenConnection((c) =>
            {
                return c.Query<ChangeTracking>($"Select DataGuid, CrudType, TableName, TimeStampChange, UserId, DataLong, DataString, UserName, Ident From {TableName} where {dataColumn} = :primaryKey and DataType = :DataType",
                    new
                    {
                        primaryKey = poco.PrimaryKey,
                        DataType = poco.ObjectType
                    });
            });

        }

        /// <summary>
        /// Gets all deleted change-tracking entries for a specific object or table
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public IEnumerable<ChangeTracking> GetAllDeleted(string tableName)
        {
            return sqlService.OpenConnection((c) =>
            {
                return c.Query<ChangeTracking>($"Select DataGuid, CrudType, TableName, TimeStampChange, UserId, DataLong, DataString, UserName, Ident From {TableName} where tableName = :tableName ",
                    new { tableName = tableName });
            });
        }

        public long GetNextIdent()
        {
            long index = 0;
            sqlService.OpenConnection((c) =>
            {
                index = c.QueryFirstOrDefault<long>($"SELECT GET_IDENTITY('{TableName}') ");
            });
            return index;
        }
    }
}
