using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace Abp.Domain.Uow
{
    /// <summary>
    /// 工作单元默认选项实现 -- 将修饰符internal 改为 public(Derrick 2017/04/02)
    /// </summary>
    public class UnitOfWorkDefaultOptions : IUnitOfWorkDefaultOptions
    {
        /// <summary>
        /// 事物范围
        /// </summary>
        public TransactionScopeOption Scope { get; set; }

        /// <summary>
        /// 工作单元能否支持事务处理.Default: true.
        /// </summary>
        public bool IsTransactional { get; set; }

        /// <summary>
        /// 获取/设置工作单元的超时时间
        /// </summary>
        public TimeSpan? Timeout { get; set; }

        /// <summary>
        /// 获取/设置事务的隔离级别.如果 <see cref="IsTransactional"/> 为ture，它将被用到
        /// </summary>
        public IsolationLevel? IsolationLevel { get; set; }

        /// <summary>
        /// 获取所有的过滤器配置列表
        /// </summary>
        public IReadOnlyList<DataFilterConfiguration> Filters
        {
            get { return _filters; }
        }
        private readonly List<DataFilterConfiguration> _filters;

        /// <summary>
        /// 注册一个数据过虑对象到工作单元
        /// </summary>
        /// <param name="filterName">过虑器名称</param>
        /// <param name="isEnabledByDefault">过虑器是否默认启用</param>
        public void RegisterFilter(string filterName, bool isEnabledByDefault)
        {
            if (_filters.Any(f => f.FilterName == filterName))
            {
                throw new AbpException("There is already a filter with name: " + filterName);
            }

            _filters.Add(new DataFilterConfiguration(filterName, isEnabledByDefault));
        }

        /// <summary>
        /// 重写过虑器定义
        /// </summary>
        /// <param name="filterName">过虑器名称</param>
        /// <param name="isEnabledByDefault">过虑器是否默认启用</param>
        public void OverrideFilter(string filterName, bool isEnabledByDefault)
        {
            _filters.RemoveAll(f => f.FilterName == filterName);
            _filters.Add(new DataFilterConfiguration(filterName, isEnabledByDefault));
        }

        /// <summary>
        /// 创建一个<see cref="UnitOfWorkDefaultOptions"/>对象
        /// </summary>
        public UnitOfWorkDefaultOptions()
        {
            _filters = new List<DataFilterConfiguration>();
            IsTransactional = true;
            Scope = TransactionScopeOption.Required;
        }
    }
}