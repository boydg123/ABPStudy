using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Threading;
using Abp.Configuration.Startup;
using Abp.Dependency;

namespace Abp.Localization.Sources.Resource
{
    /// <summary>
    /// This class is used to simplify to create a localization source that uses resource a file.
    /// 这个类用于简单地创建一个使用资源文件的本地化源
    /// </summary>
    public class ResourceFileLocalizationSource : ILocalizationSource, ISingletonDependency
    {
        /// <summary>
        /// Unique Name of the source.
        /// 唯一性名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Reference to the <see cref="ResourceManager"/> object related to this localization source.
        /// 与本地化源相关的<see cref="ResourceManager"/>对象引用
        /// </summary>
        public ResourceManager ResourceManager { get; private set; }

        /// <summary>
        /// 本地化配置
        /// </summary>
        private ILocalizationConfiguration _configuration;

        /// <param name="name">Unique Name of the source / 源的唯一性名称</param>
        /// <param name="resourceManager">Reference to the <see cref="ResourceManager"/> object related to this localization source / 与本地化源相关的<see cref="ResourceManager"/>对象引用</param>
        public ResourceFileLocalizationSource(string name, ResourceManager resourceManager)
        {
            Name = name;
            ResourceManager = resourceManager;
        }

        /// <summary>
        /// This method is called by ABP before first usage.
        /// 此方法在第一次使用前被ABP调用
        /// </summary>
        public virtual void Initialize(ILocalizationConfiguration configuration, IIocResolver iocResolver)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// 使用给定的名称获取当前语言下的本地化字符串
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>本地化字符串</returns>
        public virtual string GetString(string name)
        {
            var value = GetStringOrNull(name);
            if (value == null)
            {
                return ReturnGivenNameOrThrowException(name, Thread.CurrentThread.CurrentUICulture);
            }

            return value;
        }


        /// <summary>
        //获取给定名称和区域的本地化字符串
        /// </summary>
        /// <param name="name">键名称</param>
        /// <param name="culture">区域信息</param>
        /// <returns>本地化字符串</returns>
        public virtual string GetString(string name, CultureInfo culture)
        {
            var value = GetStringOrNull(name, culture);
            if (value == null)
            {
                return ReturnGivenNameOrThrowException(name, culture);
            }

            return value;
        }

        /// <summary>
        /// 获取当前语言中给定名称的本地化字符串。如果找不到返回NULL。
        /// </summary>
        /// <param name="name">键名称</param>
        /// <param name="tryDefaults">true:回退到默认语言如果在当前区域没有发现</param>
        /// <returns>本地化字符串</returns>
        public string GetStringOrNull(string name, bool tryDefaults = true)
        {
            //WARN: tryDefaults is not implemented!
            return ResourceManager.GetString(name);
        }

        /// <summary>
        /// 获取当前语言中给定名称的本地化字符串。如果找不到返回NULL。
        /// </summary>
        /// <param name="name">键名称</param>
        /// <param name="culture">区域信息</param>
        /// <param name="tryDefaults">true:回退到默认语言如果在当前区域没有发现</param>
        /// <returns>本地化字符串</returns>
        public string GetStringOrNull(string name, CultureInfo culture, bool tryDefaults = true)
        {
            //WARN: tryDefaults is not implemented!
            return ResourceManager.GetString(name, culture);
        }

        /// <summary>
        /// 获取所有本地化字符串
        /// </summary>
        /// <param name="includeDefaults">true:回退到默认语言文本如果当前区域没有发现。</param>
        /// <returns></returns>
        public virtual IReadOnlyList<LocalizedString> GetAllStrings(bool includeDefaults = true)
        {
            return GetAllStrings(Thread.CurrentThread.CurrentUICulture, includeDefaults);
        }

        /// <summary>
        /// 获取所有本地化字符串
        /// </summary>
        /// <param name="culture">区域信息</param>
        /// <param name="includeDefaults">true:回退到默认语言文本如果当前区域没有发现。</param>
        /// <returns></returns>
        public virtual IReadOnlyList<LocalizedString> GetAllStrings(CultureInfo culture, bool includeDefaults = true)
        {
            return ResourceManager
                .GetResourceSet(culture, true, includeDefaults)
                .Cast<DictionaryEntry>()
                .Select(entry => new LocalizedString(entry.Key.ToString(), entry.Value.ToString(), culture))
                .ToImmutableList();
        }

        /// <summary>
        /// 返回指定名称或抛出异常
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="culture">区域信息</param>
        /// <returns></returns>
        protected virtual string ReturnGivenNameOrThrowException(string name, CultureInfo culture)
        {
            return LocalizationSourceHelper.ReturnGivenNameOrThrowException(_configuration, Name, name, culture);
        }
    }
}
