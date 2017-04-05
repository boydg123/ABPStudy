using System;
using System.Collections.Generic;
using System.Transactions;

namespace Abp.Domain.Uow
{
    /// <summary>
    /// Used to get/set default options for a unit of work.
    /// 用于获取/设置工作单元默认选项
    /// </summary>
    public interface IUnitOfWorkDefaultOptions
    {
        /// <summary>
        /// Scope option.
        /// 事物范围
        /// </summary>
        TransactionScopeOption Scope { get; set; }

        /// <summary>
        /// Should unit of works be transactional.Default: true.
        /// 工作单元能否支持事务处理。Default: true.
        /// </summary>
        bool IsTransactional { get; set; }

        /// <summary>
        /// Gets/sets a timeout value for unit of works.
        /// 获取/设置工作单元的超时时间
        /// </summary>
        TimeSpan? Timeout { get; set; }

        /// <summary>
        /// Gets/sets isolation level of transaction.This is used if <see cref="IsTransactional"/> is true.
        /// 获取/设置事务的隔离级别,如果 <see cref="IsTransactional"/> 为ture，它将被用到
        /// </summary>
        IsolationLevel? IsolationLevel { get; set; }

        /// <summary>
        /// Gets list of all data filter configurations.
        /// 获取所有的过滤器配置列表
        /// </summary>
        IReadOnlyList<DataFilterConfiguration> Filters { get; }

        /// <summary>
        /// Registers a data filter to unit of work system.
        /// 注册一个数据过虑对象到工作单元
        /// </summary>
        /// <param name="filterName">Name of the filter. / 过虑器名称</param>
        /// <param name="isEnabledByDefault">Is filter enabled by default. / 过虑器是否默认启用</param>
        void RegisterFilter(string filterName, bool isEnabledByDefault);

        /// <summary>
        /// Overrides a data filter definition.
        /// 重写过虑器定义
        /// </summary>
        /// <param name="filterName">Name of the filter. / 过虑器名称</param>
        /// <param name="isEnabledByDefault">Is filter enabled by default. / 过虑器是否默认启用</param>
        void OverrideFilter(string filterName, bool isEnabledByDefault);
    }
}