using System.Collections.Generic;

namespace Abp.Localization.Dictionaries.Json
{
    /// <summary>
    /// Use it to serialize json file
    /// 用它来序列化一个json文件
    /// </summary>
    public class JsonLocalizationFile
    {
        /// <summary>
        /// Constructor
        /// 构造函数
        /// </summary>
        public JsonLocalizationFile()
        {
            Texts = new Dictionary<string, string>();
        }

        /// <summary>
        /// get or set the culture name; eg : en , en-us, zh-CN
        /// 获取或设置区域名称;例如：en , en-us, zh-CN
        /// </summary>
        public string Culture { get; set; }

        /// <summary>
        ///  Key value pairs
        ///  键-值对
        /// </summary>
        public Dictionary<string, string> Texts { get; private set; }
    }
}