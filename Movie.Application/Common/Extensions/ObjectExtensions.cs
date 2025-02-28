using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Movie.Application.Common.Extensions
{
    public static class ObjectExtensions
    {
        private static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
        };

        public static StringContent AsJson(this object value)
        {
            var json = JsonConvert.SerializeObject(value, JsonSerializerSettings);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }
    }
}