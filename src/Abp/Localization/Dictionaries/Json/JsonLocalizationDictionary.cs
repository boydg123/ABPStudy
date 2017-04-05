using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Abp.Collections.Extensions;
using Abp.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Abp.Localization.Dictionaries.Json
{
    /// <summary>
    /// This class is used to build a localization dictionary from json.
    /// 此类用于从Json中建立一个本地化字典
    /// </summary>
    /// <remarks>
    /// Use static Build methods to create instance of this class.
    /// 使用静态的Build方法创建该类的实例
    /// </remarks>
    public class JsonLocalizationDictionary : LocalizationDictionary
    {
        /// <summary>
        ///     Private constructor.
        ///     私有构造函数
        /// </summary>
        /// <param name="cultureInfo">Culture of the dictionary / 字典文化信息</param>
        private JsonLocalizationDictionary(CultureInfo cultureInfo)
            : base(cultureInfo)
        {
        }

        /// <summary>
        ///     Builds an <see cref="JsonLocalizationDictionary" /> from given file.
        ///     从给定文件构建一个<see cref="JsonLocalizationDictionary" />
        /// </summary>
        /// <param name="filePath">Path of the file / 文件的路径</param>
        public static JsonLocalizationDictionary BuildFromFile(string filePath)
        {
            try
            {
                return BuildFromJsonString(File.ReadAllText(filePath));
            }
            catch (Exception ex)
            {
                throw new AbpException("Invalid localization file format! " + filePath, ex);
            }
        }

        /// <summary>
        ///     Builds an <see cref="JsonLocalizationDictionary" /> from given json string.
        ///     从给定json字符串构建一个<see cref="JsonLocalizationDictionary" />
        /// </summary>
        /// <param name="jsonString">Json string / json字符串</param>
        public static JsonLocalizationDictionary BuildFromJsonString(string jsonString)
        {
            JsonLocalizationFile jsonFile;
            try
            {
                jsonFile = JsonConvert.DeserializeObject<JsonLocalizationFile>(
                    jsonString,
                    new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    });
            }
            catch (JsonException ex)
            {
                throw new AbpException("Can not parse json string. " + ex.Message);
            }

            var cultureCode = jsonFile.Culture;
            if (string.IsNullOrEmpty(cultureCode))
            {
                throw new AbpException("Culture is empty in language json file.");
            }

            var dictionary = new JsonLocalizationDictionary(new CultureInfo(cultureCode));
            var dublicateNames = new List<string>();
            foreach (var item in jsonFile.Texts)
            {
                if (string.IsNullOrEmpty(item.Key))
                {
                    throw new AbpException("The key is empty in given json string.");
                }

                if (dictionary.Contains(item.Key))
                {
                    dublicateNames.Add(item.Key);
                }

                dictionary[item.Key] = item.Value.NormalizeLineEndings();
            }

            if (dublicateNames.Count > 0)
            {
                throw new AbpException(
                    "A dictionary can not contain same key twice. There are some duplicated names: " +
                    dublicateNames.JoinAsString(", "));
            }

            return dictionary;
        }
    }
}