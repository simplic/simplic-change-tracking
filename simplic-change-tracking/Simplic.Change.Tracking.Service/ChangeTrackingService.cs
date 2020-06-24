using JsonDiffPatchDotNet;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Simplic.Change.Tracking.Schemas;
using Simplic.Session;
using System;
using System.Collections.Generic;
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
            var jsonOld = JsonConvert.SerializeObject(oldValue);
            var jsonNew = JsonConvert.SerializeObject(newValue);

            ChangeTrackingObject changeTrackingObject = new ChangeTrackingObject();
            List<object> list = new List<object>();
            var jdp = new JsonDiffPatch();
            //oldValue is null
            if (oldValue == null && newValue != null)
            {
                changeTrackingObject.Data = JObject.Parse(jsonNew);
                (list, changeTrackingObject.Schema) = GetAttributes(newValue);
            }
            //newValue is null
            else if (oldValue != null && newValue == null)
            {
                changeTrackingObject.Data = JObject.Parse(jsonOld);
                //Fill the schema with the old Value
                (list, changeTrackingObject.Schema) = GetAttributes(oldValue);

            }
            else
            {

                var diff = jdp.Diff(jsonOld, jsonNew);
                if (diff != null)
                {
                JObject jObject = JObject.Parse(diff);
                changeTrackingObject.Data = jObject;

                }
                //Fill the schema with the old Value
                (list,changeTrackingObject.Schema) = GetAttributes(oldValue);
                //Fill the schema with the new Value
                (_, changeTrackingObject.Schema) = GetAttributes(newValue,list, changeTrackingObject.Schema);
            }

            return JsonConvert.SerializeObject(changeTrackingObject);


        }

        /// <summary>
        /// Returns a schema to get the properties and the attribute assigned to it
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="alreadyCompared"></param>
        /// <param name="schema"></param>
        /// <returns></returns>
        (List<object>,Schema) GetAttributes(object obj, List<object> alreadyCompared = null, Schema schema = null)
        {
            if (schema == null)
                schema = new Schema();
            if (alreadyCompared == null)
                alreadyCompared = new List<object>();

            if (obj == null)
                return (alreadyCompared,schema);

            var infos = obj.GetType().GetProperties();

            foreach (var info in infos)
            {
                if (alreadyCompared.Contains(info.DeclaringType.FullName + "." + info.Name))
                {
                    continue;
                }
                else
                {
                    var attr = (ChangeTrackingDisplayName[])info.GetCustomAttributes(typeof(ChangeTrackingDisplayName), false);
                    var propSchema = new PropertySchema();
                    if (attr != null && attr.Length > 0)
                    {
                        propSchema.LocalizationKey = attr[0].Key;
                    }

                    propSchema.Type = info.PropertyType.FullName;
                    propSchema.Path = info.DeclaringType.FullName + "." + info.Name;


                    schema.Properties.Add(propSchema);


                    alreadyCompared.Add(info.DeclaringType.FullName + "." + info.Name);
                    if (!info.PropertyType.IsPrimitive && info.PropertyType.IsClass && !info.PropertyType.IsNested)
                        GetAttributes(info.GetValue(obj), alreadyCompared, schema);

                }
            }
            return (alreadyCompared, schema);
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
                UserName = sessionService.CurrentSession.UserName


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
            requestChange.DataType = obj.ToString();
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
        public IEnumerable<ChangeTracking> GetChangesWithObject(ChangeTrackingKey poco, string dataColumn = "")
        {
            return requestChangeRepository.GetChangesWithObject(poco, dataColumn);
        }

        public IEnumerable<ChangeTracking> GetAllDeleted(string tableName)
        {
            return requestChangeRepository.GetAllDeleted(tableName);
        }

        public object GetPrimaryKey(object poco)
        {
            var infos = poco.GetType().GetProperties();
            object key = null;
            foreach (var item in infos)
            {


                if (Attribute.IsDefined(item, typeof(TrackingKey)))
                {
                    var attr = (TrackingKey[])item.GetCustomAttributes(typeof(TrackingKey), false);
                    key = item.GetValue(poco);

                }
            }
            if (key == null)
            {
                throw new ChangeTrackingNotEnabledException();
            }
            return key;
        }
    }
}
