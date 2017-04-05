using System;
using System.Collections.Generic;
using System.Transactions;

namespace Abp.Domain.Uow
{
    /// <summary>
    /// Unit of work options.
    /// 工作单元选项
    /// </summary>
    public class UnitOfWorkOptions
    {
        /// <summary>
        /// Scope option.
        /// 事物范围选项
        /// </summary>
        public TransactionScopeOption? Scope { get; set; }

        /// <summary>
        /// Is this UOW transactional?Uses default value if not supplied.
        /// 此工作单元是否支持事物。默认不支持。
        /// </summary>
        public bool? IsTransactional { get; set; }

        /// <summary>
        /// Timeout of UOW As milliseconds.Uses default value if not supplied.
        /// 此工作单元的超时时间为毫秒。如果不支持，就使用默认值
        /// </summary>
        public TimeSpan? Timeout { get; set; }

        /// <summary>
        /// If this UOW is transactional, this option indicated the isolation level of the transaction.
        /// 如果工作单元支持事务，此选项指定事件的隔离级别
        /// Uses default value if not supplied.
        /// 如果不支持，就使用默认值
        /// </summary>
        public IsolationLevel? IsolationLevel { get; set; }

        /// <summary>
        /// This option should be set to <see cref="TransactionScopeAsyncFlowOption.Enabled"/> if unit of work is used in an async scope.
        /// 如果工作单使用异步，此选项可惟设置为<see cref="TransactionScopeAsyncFlowOption.Enabled"/>
        /// </summary>
        public TransactionScopeAsyncFlowOption? AsyncFlowOption { get; set; }

        /// <summary>
        /// Can be used to enable/disable some filters. 
        /// 可用来启动/禁用某些过滤器
        /// </summary>
        public List<DataFilterConfiguration> FilterOverrides { get; private set; }

        /// <summary>
        /// Creates a new <see cref="UnitOfWorkOptions"/> object.
        /// 创建一个新的 <see cref="UnitOfWorkOptions"/> 对象
        /// </summary>
        public UnitOfWorkOptions()
        {
            FilterOverrides = new List<DataFilterConfiguration>();
        }

        /// <summary>
        /// 为没有提供者的选项填充默认值
        /// </summary>
        /// <param name="defaultOptions"></param>
        internal void FillDefaultsForNonProvidedOptions(IUnitOfWorkDefaultOptions defaultOptions)
        {
            //TODO: Do not change options object..?

            if (!IsTransactional.HasValue)
            {
                IsTransactional = defaultOptions.IsTransactional;
            }

            if (!Scope.HasValue)
            {
                Scope = defaultOptions.Scope;
            }

            if (!Timeout.HasValue && defaultOptions.Timeout.HasValue)
            {
                Timeout = defaultOptions.Timeout.Value;
            }

            if (!IsolationLevel.HasValue && defaultOptions.IsolationLevel.HasValue)
            {
                IsolationLevel = defaultOptions.IsolationLevel.Value;
            }
        }
    }
}