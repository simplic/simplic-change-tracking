using KellermanSoftware.CompareNetObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.Change.Tracking.Service
{
    public class RequestChangeService : IRequestChangeService
    {
        IRequestChangeRepository requestChangeRepository;
        public RequestChangeService(IRequestChangeRepository requestChangeRepository)
        {
            this.requestChangeRepository = requestChangeRepository;
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

        public RequestChange get(Int64 id)
        {
            return requestChangeRepository.get(id);
        }

        public bool save(RequestChange obj)
        {
            return requestChangeRepository.save(obj);
        }
    }
}
