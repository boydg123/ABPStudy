using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Abp.Json
{
    /// <summary>
    /// Json扩展类
    /// </summary>
    public static class JsonExtensions
    {
        /// <summary>
        /// Converts given object to JSON string.
        /// 转换对象到Json字符串
        /// </summary>
        /// <returns></returns>
        public static string ToJsonString(this object obj, bool camelCase = false, bool indented = false)
        {
            var options = new JsonSerializerSettings();

            if (camelCase)
            {
                options.ContractResolver = new CamelCasePropertyNamesContractResolver();
            }

            if (indented)
            {
                options.Formatting = Formatting.Indented;
            }

            options.Converters.Insert(0, new AbpDateTimeConverter());

            return JsonConvert.SerializeObject(obj, options);
        }
    }
}
