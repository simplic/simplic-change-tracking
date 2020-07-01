using Simplic.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.Change.Tracking
{
    public interface IChangeTrackingRepository 
    {
        /// <summary>
        /// Save the change tracking obj
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool Save(ChangeTracking obj);
        
        /// <summary>
        /// Get a change tracking based on a long id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ChangeTracking Get(long id);

        /// <summary>
        /// Gets an ienumerable that contains all changes based on a primary key object
        /// </summary>
        /// <param name="primaryKey"></param>
        /// <returns></returns>
        IEnumerable<ChangeTracking> GetChanges(object primaryKey);
        
        /// <summary>
        /// Gets the json as an byte array based on the long ident
        /// </summary>
        /// <param name="ident"></param>
        /// <returns></returns>
        byte[] GetJsonAsByteArray(long ident);

        /// <summary>
        /// Gets an ienumerable that contains all changes based on the change tracking key and the data column which is empty by default
        /// </summary>
        /// <param name="changeTrackingKey"></param>
        /// <param name="dataColumn"></param>
        /// <returns></returns>
        IEnumerable<ChangeTracking> GetChangesWithObject(ChangeTrackingKey changeTrackingKey, string dataColumn = "");

        /// <summary>
        /// Gets an ienumerable that contains all deleted based on the table name
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        IEnumerable<ChangeTracking> GetAllDeleted(string tableName);

    }
}
