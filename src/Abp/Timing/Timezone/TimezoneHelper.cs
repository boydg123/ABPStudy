using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using Abp.Extensions;
using Abp.IO.Extensions;
using Abp.Xml.Extensions;

namespace Abp.Timing.Timezone
{
    /// <summary>
    /// A helper class for timezone operations
    /// 时区操作帮助类
    /// </summary>
    public static class TimezoneHelper
    {
        /// <summary>
        /// 时区映射字典
        /// </summary>
        static readonly Dictionary<string, string> TimeZoneMappings = new Dictionary<string, string>();
        static readonly object SyncObj = new object();

        /// <summary>
        /// Maps given windows timezone id to IANA timezone id
        /// 映射给定的Windows时区ID 于 IANA时区ID
        /// </summary>
        /// <param name="windowsTimezoneId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string WindowsToIana(string windowsTimezoneId)
        {
            if (windowsTimezoneId.Equals("UTC", StringComparison.OrdinalIgnoreCase))
            {
                return "Etc/UTC";
            }

            GetTimezoneMappings();

            if (TimeZoneMappings.ContainsKey(windowsTimezoneId))
            {
                return TimeZoneMappings[windowsTimezoneId];
            }

            throw new Exception(string.Format("Unable to map {0} to iana timezone.", windowsTimezoneId));
        }

        /// <summary>
        /// 时间转换
        /// </summary>
        /// <param name="date">要转换的时间</param>
        /// <param name="fromTimeZoneId">从源时区</param>
        /// <param name="toTimeZoneId">转到目标时区</param>
        /// <returns></returns>
        public static DateTime? Convert(DateTime? date, string fromTimeZoneId, string toTimeZoneId)
        {
            if (!date.HasValue)
            {
                return null;
            }

            var sourceTimeZone = TimeZoneInfo.FindSystemTimeZoneById(fromTimeZoneId);
            var destinationTimeZone = TimeZoneInfo.FindSystemTimeZoneById(toTimeZoneId);
            return TimeZoneInfo.ConvertTime(date.Value, sourceTimeZone, destinationTimeZone);
        }

        /// <summary>
        /// 从UTC转换时间
        /// </summary>
        /// <param name="date">要转换的时间</param>
        /// <param name="toTimeZoneId">转到目标时区</param>
        /// <returns></returns>
        public static DateTime? ConvertFromUtc(DateTime? date, string toTimeZoneId)
        {
            return Convert(date, "UTC", toTimeZoneId);
        }

        /// <summary>
        /// 获取时区映射
        /// </summary>
        private static void GetTimezoneMappings()
        {
            lock (SyncObj)
            {
                if (TimeZoneMappings.Count > 0)
                {
                    return;
                }
            }

            var assembly = Assembly.GetExecutingAssembly();
            var resourceNames = assembly.GetManifestResourceNames();

            var resourceName = resourceNames.First(r => r.Contains("WindowsZones.xml"));

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                var bytes = stream.GetAllBytes();
                var xmlString = Encoding.UTF8.GetString(bytes, 3, bytes.Length - 3); //Skipping byte order mark
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xmlString);
                var defaultMappingNodes = xmlDocument.SelectNodes("//supplementalData/windowsZones/mapTimezones/mapZone[@territory='001']");
                AddMappingsToDictionary(TimeZoneMappings, defaultMappingNodes);
            }
        }

        /// <summary>
        /// 添加时区映射至字典
        /// </summary>
        /// <param name="timeZoneMappings">时区映射字段</param>
        /// <param name="defaultMappingNodes">XML默认映射节点</param>
        private static void AddMappingsToDictionary(Dictionary<string, string> timeZoneMappings, XmlNodeList defaultMappingNodes)
        {
            lock (SyncObj)
            {
                if (TimeZoneMappings.Count > 0)
                {
                    return;
                }

                foreach (XmlNode defaultMappingNode in defaultMappingNodes)
                {
                    var windowsTimezoneId = defaultMappingNode.GetAttributeValueOrNull("other");
                    var ianaTimezoneId = defaultMappingNode.GetAttributeValueOrNull("type");
                    if (windowsTimezoneId.IsNullOrEmpty() || ianaTimezoneId.IsNullOrEmpty())
                    {
                        continue;
                    }

                    timeZoneMappings.Add(windowsTimezoneId, ianaTimezoneId);
                }
            }
        }
    }
}
