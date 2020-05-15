using KellermanSoftware.CompareNetObjects;
using Newtonsoft.Json;
using Simplic.Session;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Simplic.Change.Tracking.Service
{
    public class RequestChangeService : IRequestChangeService
    {
        IRequestChangeRepository requestChangeRepository;
        ISessionService sessionService;
        public RequestChangeService(IRequestChangeRepository requestChangeRepository, ISessionService sessionService)
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
                CaseSensitive = true

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
        public RequestChange Get(Int64 id)
        {
            return requestChangeRepository.Get(id);
        }

        /// <summary>
        /// Saves the request changes poco into database
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Save(RequestChange obj)
        {
            return requestChangeRepository.Save(obj);
        }

        /// <summary>
        /// Tracks the changes and saves the 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="obj"></param>
        /// <param name="crudType">Either insert/update or delete</param>
        /// <param name="tableName"></param>
        /// <param name="snapshot">a copy</param>
        public void TrackChange<TModel>(object obj, CrudType crudType, string tableName, object snapshot)
        {
            RequestChange requestChange = new RequestChange
            {

                UserId = sessionService.CurrentSession.UserId,
                TableName = tableName,
                TimeStampChange = DateTime.Now,
                Type = crudType,

            };
            requestChange = CheckForPrimaryKey<TModel>(requestChange, obj);

            if (obj is ITrackable trackable)
            {
                trackable.Snapshot = CreateDeepCopy<TModel>(obj);
                requestChange.JsonObject = DetailedCompare<TModel>((TModel)trackable.Snapshot, (TModel)obj);
            }
            else
            {
                snapshot = CreateDeepCopy<TModel>(snapshot);
                //object Snapshot = Get(GetId((TModel)obj));
                requestChange.JsonObject = DetailedCompare<TModel>((TModel)snapshot, (TModel)obj);
            }
            Save(requestChange);
        }

        public TModel CreateDeepCopy<TModel>(object obj)
        {
            var jsonString = JsonConvert.SerializeObject(obj);
            TModel snapshot = (TModel)JsonConvert.DeserializeObject(jsonString);
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
        private RequestChange CheckForPrimaryKey<TModel>(RequestChange requestChange, Object obj)
        {
            string[] keys = { "Guid", "Id", "Ident", "Key" };
            var info = (TModel)obj;
            var infos = info.GetType().GetProperties();
            foreach (var item in infos)
            {
                if (keys.Contains(item.Name))
                {
                    switch (item.Name)
                    {
                        case "Guid":
                            requestChange.DataGuid = (Guid)item.GetValue(obj);
                            break;
                        case "Id":
                        case "Ident":
                            {
                                requestChange.DataLong = (int)item.GetValue(obj);
                                break;
                            }
                        case "Key":
                            requestChange.DataString = (string)item.GetValue(obj);
                            break;
                        default:

                            break;

                    }
                }

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
    }
}
