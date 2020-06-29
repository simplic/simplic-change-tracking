using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.Change.Tracking
{
    
    public class ChangeTrackingJsonConverter : JsonConverter
    {
        private readonly Type[] _types;

        public ChangeTrackingJsonConverter(params Type[] types)
        {
            _types = types;
        }
        /// <summary>
        /// Will Write Json that ignore properties with the attribute IgnoreChangeTracking
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="serializer"></param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            JToken t = JToken.FromObject(value);

            if (t.Type != JTokenType.Object)
            {
                t.WriteTo(writer);
            }
            else
            {
                JObject o = (JObject)t;
                foreach (var item in value.GetType().GetProperties())
                {
                    var attr = (IgnoreChangeTracking[])item.GetCustomAttributes(typeof(IgnoreChangeTracking), false);
                    if (attr != null && attr.Length > 0)
                    {
                        o.Remove(item.Name);
                    }
                }



                o.WriteTo(writer);

            }

        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException("Unnecessary because CanRead is false. The type will skip the converter.");
        }

        public override bool CanRead
        {
            get { return false; }
        }

        public override bool CanConvert(Type objectType)
        {
            return _types.Any(t => t == objectType);
        }
    }
}
