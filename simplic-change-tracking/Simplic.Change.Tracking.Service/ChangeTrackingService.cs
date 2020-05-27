using KellermanSoftware.CompareNetObjects;
using Newtonsoft.Json;
using Simplic.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simplic.Change.Tracking.Service
{
    public class ChangeTrackingService : IChangeTrackingService
    {
        IChangeTrackingRepository requestChangeRepository;
        ISessionService sessionService;

        /// <summary>
        /// Constructor for dependency injection
        /// </summary>
        /// <param name="requestChangeRepository"></param>
        /// <param name="sessionService"></param>
        public ChangeTrackingService(IChangeTrackingRepository requestChangeRepository, ISessionService sessionService)
        {
            this.requestChangeRepository = requestChangeRepository;
            this.sessionService = sessionService;
        }


        /// <summary>
        /// Gets a json string of the changes based on the difference between an old value and a new value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        /// <returns>json serialized string</returns>
        public string DetailedCompare<T>(T oldValue, T newValue)
        {
            IList<Variance> variances = new List<Variance>();


            ComparisonConfig comparisonConfig = new ComparisonConfig
            {
                MaxDifferences = 999,
                CaseSensitive = true,
                

            };
            CompareLogic compareLogic = new CompareLogic(comparisonConfig);
            ComparisonResult result = compareLogic.Compare(oldValue, newValue);

            if (!result.AreEqual)
            {
                foreach (var difference in result.Differences)
                {
                    Variance variance = new Variance
                    {
                        Property = difference.PropertyName,
                        OldValue = difference.Object1Value,
                        NewValue = difference.Object2Value
                    };

                    variances.Add(variance);
                }

            }
            var json = JsonConvert.SerializeObject(variances);
            return json;
        }

        /// <summary>
        /// Gets the Request change based on an int 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ChangeTracking Get(Int64 id)
        {
            return requestChangeRepository.Get(id);
        }

        /// <summary>
        /// Saves the request changes poco into database
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Save(ChangeTracking obj)
        {
            return requestChangeRepository.Save(obj);
        }

        /// <summary>
        /// Tracks the changes and saves them 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="obj"></param>
        /// <param name="crudType">Either insert/update or delete</param>
        /// <param name="tableName"></param>
        /// <param name="snapshot">a copy</param>
        public void TrackChange<TModel, TId>(object obj, CrudType crudType, string tableName, object snapshot, object primaryKey)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            ChangeTracking requestChange = new ChangeTracking
            {

                UserId = sessionService.CurrentSession.UserId,
                TableName = tableName,
                TimeStampChange = DateTime.Now,
                CrudType = crudType,

            };
            requestChange = SetPrimaryKey<TModel, TId>(requestChange, primaryKey);

            if (obj is ITrackable trackable && trackable.Snapshot != null)
            {
                requestChange.JsonObject = DetailedCompare<TModel>((TModel)trackable.Snapshot, (TModel)obj);
            }
            else
            {
                snapshot = CreateDeepCopy<TModel>(snapshot);
                requestChange.JsonObject = DetailedCompare<TModel>((TModel)snapshot, (TModel)obj);
            }
            Save(requestChange);
        }
        
        /// <summary>
        /// Creates a deep copy based on json serialize and deserialize 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public TModel CreateDeepCopy<TModel>(object obj)
        {
            var jsonString = JsonConvert.SerializeObject((TModel)obj);
            TModel snapshot = JsonConvert.DeserializeObject<TModel>(jsonString);
            return snapshot;
        }

        /// <summary>
        /// Method to check if there is any primary key 
        /// Array contains Guid->Guid, Id ->int, Ident->int, Key->string
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="requestChange"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        private ChangeTracking SetPrimaryKey<TModel, TId>(ChangeTracking requestChange, object primaryKey)
        {

            if (primaryKey is Guid guid)
            {
                requestChange.DataGuid = guid;
            }
            else if (primaryKey is int || primaryKey is long)
            {
                requestChange.DataLong = (long)primaryKey;
            }
            else
            {
                requestChange.DataString = primaryKey?.ToString() ?? "<unset>";
            }

            return requestChange;
        }

        /// <summary>
        /// Return true if the obj implements the interface IsTrackable
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool IsTrackable<TModel>(object obj)
        {
            if ((TModel)obj is ITrackable)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Get all changes based on a primary key of object 
        /// </summary>
        /// <param name="primaryKey"></param>
        /// <returns></returns>
        public IEnumerable<ChangeTracking> GetChanges(object primaryKey)
        {
            return requestChangeRepository.GetChanges(primaryKey);
        }


        /// <summary>
        /// Gets the json string as a byte array
        /// </summary>
        /// <param name="ident"></param>
        /// <returns></returns>
        public byte[] GetJsonAsByteArray(long ident)
        {
            return requestChangeRepository.GetJsonAsByteArray(ident);
        }

        /// <summary>
        /// Gets a json string thats based shows the property, old value and new value of all changes based on a identifier
        /// </summary>
        /// <param name="ident"></param>
        /// <returns></returns>
        public string GetJson(long ident)
        {
            var arr = GetJsonAsByteArray(ident);
            return Encoding.UTF8.GetString(arr);
        }

        /// <summary>
        /// Gets a Ienumerable of type change tracking based on an object which contains a primary key
        /// </summary>
        /// <param name="primaryKey"></param>
        /// <returns></returns>
        public IEnumerable<ChangeTracking> GetChangesWithObject(object poco, string dataColumn = "")
        {
            var infos = poco.GetType().GetProperties();
            ;
            object value = null;
            foreach (var info in infos)
            {

                switch (info.Name)
                {
                    case ("Guid"):
                        value = info.GetValue(poco);
                        dataColumn = "DataGuid";
                        break;
                    case ("Identifier"):
                        value = info.GetValue(poco);
                        dataColumn = "DataString";
                        break;
                    case ("Id"):
                    case ("Ident"):
                        value = info.GetValue(poco);
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
            return requestChangeRepository.GetChangesWithObject(value, dataColumn);
        }

    }
}
