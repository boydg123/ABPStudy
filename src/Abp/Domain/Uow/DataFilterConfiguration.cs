using System.Collections.Generic;

namespace Abp.Domain.Uow
{
    /// <summary>
    /// 数据过滤配置
    /// </summary>
    public class DataFilterConfiguration
    {
        /// <summary>
        /// 过滤器名称
        /// </summary>
        public string FilterName { get; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled { get; }

        /// <summary>
        /// 过滤器参数
        /// </summary>
        public IDictionary<string, object> FilterParameters { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filterName">过虑器名称</param>
        /// <param name="isEnabled">是否启用</param>
        public DataFilterConfiguration(string filterName, bool isEnabled)
        {
            FilterName = filterName;
            IsEnabled = isEnabled;
            FilterParameters = new Dictionary<string, object>();
        }

        /// <summary>
        /// 克隆一个数据过虑对象
        /// </summary>
        /// <param name="filterToClone">被克隆的对象</param>
        /// <param name="isEnabled">是否启用</param>
        internal DataFilterConfiguration(DataFilterConfiguration filterToClone, bool? isEnabled = null)
            : this(filterToClone.FilterName, isEnabled ?? filterToClone.IsEnabled)
        {
            foreach (var filterParameter in filterToClone.FilterParameters)
            {
                FilterParameters[filterParameter.Key] = filterParameter.Value;
            }
        }
    }
}