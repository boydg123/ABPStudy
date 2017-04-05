using System;
using Newtonsoft.Json;

namespace Abp.Localization
{
    /// <summary>
    /// This class can be used to serialize <see cref="ILocalizableString"/> to <see cref="string"/> during serialization.
    /// 此类用于在序列化期间序列化<see cref="ILocalizableString"/> 到字符串
    /// It does not work for deserialization.
    /// 它不能用于反序列化
    /// </summary>
    public class LocalizableStringToStringJsonConverter : JsonConverter
    {
        /// <summary>
        /// 写Json
        /// </summary>
        /// <param name="writer">Json编写器</param>
        /// <param name="value">值</param>
        /// <param name="serializer">Json序列化器</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            var localizableString = (ILocalizableString) value;
            writer.WriteValue(localizableString.Localize(new LocalizationContext(LocalizationHelper.Manager)));
        }

        /// <summary>
        /// 读Json
        /// </summary>
        /// <param name="reader">Json读取器</param>
        /// <param name="objectType">对象类型</param>
        /// <param name="existingValue">存在的值</param>
        /// <param name="serializer">序列化器</param>
        /// <returns></returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 能否被转换
        /// </summary>
        /// <param name="objectType">类型</param>
        /// <returns></returns>
        public override bool CanConvert(Type objectType)
        {
            return typeof (ILocalizableString).IsAssignableFrom(objectType);
        }
    }
}