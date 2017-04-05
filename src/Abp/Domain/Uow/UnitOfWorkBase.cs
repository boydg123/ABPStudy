using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Abp.Extensions;
using Abp.Runtime.Session;
using Castle.Core;

namespace Abp.Domain.Uow
{
    /// <summary>
    /// Base for all Unit Of Work classes.
    /// 工作单元基类
    /// </summary>
    public abstract class UnitOfWorkBase : IUnitOfWork
    {
        /// <summary>
        /// ID
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// 外部UOW的引用，如果存在
        /// </summary>
        [DoNotWire]
        public IUnitOfWork Outer { get; set; }

        /// <summary>
        /// 该事件在工作单元成功完成时被激发
        /// </summary>
        public event EventHandler Completed;

        /// <summary>
        /// 该事件在工作单元失败时被激发
        /// </summary>
        public event EventHandler<UnitOfWorkFailedEventArgs> Failed;

        /// <summary>
        /// 该事件在工作单元销毁时被激发
        /// </summary>
        public event EventHandler Disposed;

        /// <summary>
        /// 工作单元选项
        /// </summary>
        public UnitOfWorkOptions Options { get; private set; }

        /// <summary>
        /// 从工作单元获取数据过滤器配置
        /// </summary>
        public IReadOnlyList<DataFilterConfiguration> Filters
        {
            get { return _filters.ToImmutableList(); }
        }
        private readonly List<DataFilterConfiguration> _filters;

        /// <summary>
        /// Gets default UOW options.
        /// 获取默认的UOW选项
        /// </summary>
        protected IUnitOfWorkDefaultOptions DefaultOptions { get; }

        /// <summary>
        /// Gets the connection string resolver.
        /// 获取连接字符串解析器
        /// </summary>
        protected IConnectionStringResolver ConnectionStringResolver { get; }

        /// <summary>
        /// Gets a value indicates that this unit of work is disposed or not.
        /// 此工作单元是否销毁
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Reference to current ABP session.
        /// 当前ABP系统Session引用
        /// </summary>
        public IAbpSession AbpSession { protected get; set; }

        /// <summary>
        /// 工作单元执行器
        /// </summary>
        protected IUnitOfWorkFilterExecuter FilterExecuter { get; }

        /// <summary>
        /// Is <see cref="Begin"/> method called before?
        /// <see cref="Begin"/> 方法是否已被调用
        /// </summary>
        private bool _isBeginCalledBefore;

        /// <summary>
        /// Is <see cref="Complete"/> method called before?
        /// <see cref="Complete"/> 方法是否已被调用
        /// </summary>
        private bool _isCompleteCalledBefore;

        /// <summary>
        /// Is this unit of work successfully completed.
        /// 此工作单元，是否成功完成
        /// </summary>
        private bool _succeed;

        /// <summary>
        /// A reference to the exception if this unit of work failed.
        /// 导致工作单元失败的异常
        /// </summary>
        private Exception _exception;

        /// <summary>
        /// 租户ID
        /// </summary>
        private int? _tenantId;

        /// <summary>
        /// Constructor.
        /// 构造函数
        /// </summary>
        protected UnitOfWorkBase(
            IConnectionStringResolver connectionStringResolver, 
            IUnitOfWorkDefaultOptions defaultOptions,
            IUnitOfWorkFilterExecuter filterExecuter)
        {
            FilterExecuter = filterExecuter;
            DefaultOptions = defaultOptions;
            ConnectionStringResolver = connectionStringResolver;

            Id = Guid.NewGuid().ToString("N");
            _filters = defaultOptions.Filters.ToList();

            AbpSession = NullAbpSession.Instance;
        }

        /// <summary>
        /// 使用给定的选项，开启一个工作单元
        /// </summary>
        /// <param name="options">工作单元选项</param>
        public void Begin(UnitOfWorkOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException("options");
            }

            PreventMultipleBegin();
            Options = options; //TODO: Do not set options like that, instead make a copy?

            SetFilters(options.FilterOverrides);

            SetTenantId(AbpSession.TenantId);

            BeginUow();
        }

        /// <summary>
        /// 现在，保存工作单元中所有的修改。
        /// 这个方法在需要应用修改时调用。
        /// 注意，如果工作单元是事务性的，不仅保存变更，还会在事务失败时，回滚事务。
        /// 一般不用显式调用SaveChages，因为工作单元会自动保存所有变更
        /// </summary>
        public abstract void SaveChanges();

        /// <summary>
        /// 现在，保存工作单元中所有的修改。
        /// 这个方法在需要应用修改时调用。
        /// 注意，如果工作单元是事务性的，不仅保存变更，还会在事务失败时，回滚事务。
        /// 一般不用显式调用SaveChages，因为工作单元会自动保存所有变更
        /// </summary>
        public abstract Task SaveChangesAsync();

        /// <summary>
        /// 禁用一个或多个数据过滤器
        /// 如果一个过滤器被禁用，它将不会有作任何操作
        /// 使用using语句来使用该方法，可以在需要时启用过滤器
        /// </summary>
        /// <param name="filterNames">一个或多个标准过滤器的名称</param>
        /// <returns>能恢复禁用前效果的对象</returns>
        public IDisposable DisableFilter(params string[] filterNames)
        {
            //TODO: Check if filters exists?

            var disabledFilters = new List<string>();

            foreach (var filterName in filterNames)
            {
                var filterIndex = GetFilterIndex(filterName);
                if (_filters[filterIndex].IsEnabled)
                {
                    disabledFilters.Add(filterName);
                    _filters[filterIndex] = new DataFilterConfiguration(_filters[filterIndex], false);
                }
            }

            disabledFilters.ForEach(ApplyDisableFilter);

            return new DisposeAction(() => EnableFilter(disabledFilters.ToArray()));
        }

        /// <summary>
        /// 启用一个或多个数据过滤器
        /// 如果一个过滤器被启用，它将不会有作任何操作
        /// 使用using语句来使用该方法，可以在需要时禁用过滤器
        /// </summary>
        /// <param name="filterNames">一个或多个标准过滤器的名称</param>
        /// <returns>能恢复启用前效果的对象</returns>
        public IDisposable EnableFilter(params string[] filterNames)
        {
            //TODO: Check if filters exists?

            var enabledFilters = new List<string>();

            foreach (var filterName in filterNames)
            {
                var filterIndex = GetFilterIndex(filterName);
                if (!_filters[filterIndex].IsEnabled)
                {
                    enabledFilters.Add(filterName);
                    _filters[filterIndex] = new DataFilterConfiguration(_filters[filterIndex], true);
                }
            }

            enabledFilters.ForEach(ApplyEnableFilter);

            return new DisposeAction(() => DisableFilter(enabledFilters.ToArray()));
        }

        /// <summary>
        /// 检查一个过滤器是否被禁用
        /// </summary>
        /// <param name="filterName">过滤器的名称</param>
        /// <returns></returns>
        public bool IsFilterEnabled(string filterName)
        {
            return GetFilter(filterName).IsEnabled;
        }

        /// <summary>
        /// 设置（重载）过滤器参数的名称
        /// </summary>
        /// <param name="filterName">过滤器的名称</param>
        /// <param name="parameterName">参数名称</param>
        /// <param name="value">要给参数设置的值</param>
        /// <returns></returns>
        public IDisposable SetFilterParameter(string filterName, string parameterName, object value)
        {
            var filterIndex = GetFilterIndex(filterName);

            var newfilter = new DataFilterConfiguration(_filters[filterIndex]);

            //Store old value / 存储旧值
            object oldValue = null;
            var hasOldValue = newfilter.FilterParameters.ContainsKey(parameterName);
            if (hasOldValue)
            {
                oldValue = newfilter.FilterParameters[parameterName];
            }

            newfilter.FilterParameters[parameterName] = value;

            _filters[filterIndex] = newfilter;

            ApplyFilterParameterValue(filterName, parameterName, value);

            return new DisposeAction(() =>
            {
                //Restore old value
                if (hasOldValue)
                {
                    SetFilterParameter(filterName, parameterName, oldValue);
                }
            });
        }

        /// <summary>
        /// 设置租户ID
        /// </summary>
        /// <param name="tenantId">租户ID</param>
        /// <returns></returns>
        public IDisposable SetTenantId(int? tenantId)
        {
            var oldTenantId = _tenantId;
            _tenantId = tenantId;

            var mustHaveTenantEnableChange = tenantId == null
                ? DisableFilter(AbpDataFilters.MustHaveTenant)
                : EnableFilter(AbpDataFilters.MustHaveTenant);

            var mayHaveTenantChange = SetFilterParameter(AbpDataFilters.MayHaveTenant, AbpDataFilters.Parameters.TenantId, tenantId);
            var mustHaveTenantChange = SetFilterParameter(AbpDataFilters.MustHaveTenant, AbpDataFilters.Parameters.TenantId, tenantId ?? 0);

            return new DisposeAction(() =>
            {
                mayHaveTenantChange.Dispose();
                mustHaveTenantChange.Dispose();
                mustHaveTenantEnableChange.Dispose();
                _tenantId = oldTenantId;
            });
        }

        /// <summary>
        /// 获取租户ID
        /// </summary>
        /// <returns></returns>
        public int? GetTenantId()
        {
            return _tenantId;
        }

        /// <summary>
        /// 结束此工作单元
        /// 保存所有的更变，如果有事务，并提交事务
        /// </summary>
        public void Complete()
        {
            PreventMultipleComplete();
            try
            {
                CompleteUow();
                _succeed = true;
                OnCompleted();
            }
            catch (Exception ex)
            {
                _exception = ex;
                throw;
            }
        }

        /// <summary>
        /// 结束此工作单元-异步
        /// 保存所有的更变，如果有事务，并提交事务
        /// </summary>
        public async Task CompleteAsync()
        {
            PreventMultipleComplete();
            try
            {
                await CompleteUowAsync();
                _succeed = true;
                OnCompleted();
            }
            catch (Exception ex)
            {
                _exception = ex;
                throw;
            }
        }

        /// <summary>
        /// 销毁工作单元
        /// </summary>
        public void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            IsDisposed = true;

            if (!_succeed)
            {
                OnFailed(_exception);
            }

            DisposeUow();
            OnDisposed();
        }

        /// <summary>
        /// Can be implemented by derived classes to start UOW.
        /// 需要在派生类中实现，以便开始工作单元
        /// </summary>
        protected virtual void BeginUow()
        {
            
        }

        /// <summary>
        /// Should be implemented by derived classes to complete UOW.
        /// 需要在派生类中实现，以便完成工作单元
        /// </summary>
        protected abstract void CompleteUow();

        /// <summary>
        /// Should be implemented by derived classes to complete UOW.
        /// 需要在派生类中实现，以便完成工作单元 - 异步
        /// </summary>
        protected abstract Task CompleteUowAsync();

        /// <summary>
        /// Should be implemented by derived classes to dispose UOW.
        /// 需要在派生类中实现，以便销毁工作单元
        /// </summary>
        protected abstract void DisposeUow();

        /// <summary>
        /// 用于禁用过滤器
        /// </summary>
        /// <param name="filterName">过滤器名称</param>
        protected virtual void ApplyDisableFilter(string filterName)
        {
            FilterExecuter.ApplyDisableFilter(this, filterName);
        }

        /// <summary>
        /// 用于启用过滤器
        /// </summary>
        /// <param name="filterName">过滤器名称</param>
        protected virtual void ApplyEnableFilter(string filterName)
        {
            FilterExecuter.ApplyEnableFilter(this, filterName);
        }

        /// <summary>
        /// 用于过滤器参数值
        /// </summary>
        /// <param name="filterName">过滤器名称</param>
        /// <param name="parameterName">参数名</param>
        /// <param name="value">用户设置参数的值</param>
        protected virtual void ApplyFilterParameterValue(string filterName, string parameterName, object value)
        {
            FilterExecuter.ApplyFilterParameterValue(this, filterName, parameterName, value);
        }

        /// <summary>
        /// 解析连接字符串
        /// </summary>
        /// <param name="args">字符串解析参数</param>
        /// <returns></returns>
        protected virtual string ResolveConnectionString(ConnectionStringResolveArgs args)
        {
            return ConnectionStringResolver.GetNameOrConnectionString(args);
        }

        /// <summary>
        /// Called to trigger <see cref="Completed"/> event.
        /// 调用触发 <see cref="Completed"/> 事件
        /// </summary>
        protected virtual void OnCompleted()
        {
            Completed.InvokeSafely(this);
        }

        /// <summary>
        /// Called to trigger <see cref="Failed"/> event.
        /// 调用触发 <see cref="Failed"/> 事件
        /// </summary>
        /// <param name="exception">Exception that cause failure / 导致失败的异常</param>
        protected virtual void OnFailed(Exception exception)
        {
            Failed.InvokeSafely(this, new UnitOfWorkFailedEventArgs(exception));
        }

        /// <summary>
        /// Called to trigger <see cref="Disposed"/> event.
        /// 调用触发 <see cref="Disposed"/> 事件
        /// </summary>
        protected virtual void OnDisposed()
        {
            Disposed.InvokeSafely(this);
        }

        /// <summary>
        /// 阻止多次调用Begin
        /// </summary>
        private void PreventMultipleBegin()
        {
            if (_isBeginCalledBefore)
            {
                throw new AbpException("This unit of work has started before. Can not call Start method more than once.");
            }

            _isBeginCalledBefore = true;
        }

        /// <summary>
        /// 阻止多次调用Complete
        /// </summary>
        private void PreventMultipleComplete()
        {
            if (_isCompleteCalledBefore)
            {
                throw new AbpException("Complete is called before!");
            }

            _isCompleteCalledBefore = true;
        }

        /// <summary>
        /// 设置过滤器
        /// </summary>
        /// <param name="filterOverrides">数据过滤器配置列表</param>
        private void SetFilters(List<DataFilterConfiguration> filterOverrides)
        {
            for (var i = 0; i < _filters.Count; i++)
            {
                var filterOverride = filterOverrides.FirstOrDefault(f => f.FilterName == _filters[i].FilterName);
                if (filterOverride != null)
                {
                    _filters[i] = filterOverride;
                }
            }

            if (AbpSession.TenantId == null)
            {
                ChangeFilterIsEnabledIfNotOverrided(filterOverrides, AbpDataFilters.MustHaveTenant, false);
            }
        }

        /// <summary>
        /// 如果没有重写过滤器配置，改变过滤器的状态（是否激活）
        /// </summary>
        /// <param name="filterOverrides">数据过滤器列表</param>
        /// <param name="filterName">过滤器名称</param>
        /// <param name="isEnabled">是否启用</param>
        private void ChangeFilterIsEnabledIfNotOverrided(List<DataFilterConfiguration> filterOverrides, string filterName, bool isEnabled)
        {
            if (filterOverrides.Any(f => f.FilterName == filterName))
            {
                return;
            }

            var index = _filters.FindIndex(f => f.FilterName == filterName);
            if (index < 0)
            {
                return;
            }

            if (_filters[index].IsEnabled == isEnabled)
            {
                return;
            }

            _filters[index] = new DataFilterConfiguration(filterName, isEnabled);
        }

        /// <summary>
        /// 获取过滤器
        /// </summary>
        /// <param name="filterName">过滤器名称</param>
        /// <returns></returns>
        private DataFilterConfiguration GetFilter(string filterName)
        {
            var filter = _filters.FirstOrDefault(f => f.FilterName == filterName);
            if (filter == null)
            {
                throw new AbpException("Unknown filter name: " + filterName + ". Be sure this filter is registered before.");
            }

            return filter;
        }

        /// <summary>
        /// 获取过滤器索引
        /// </summary>
        /// <param name="filterName">过滤器名称</param>
        /// <returns></returns>
        private int GetFilterIndex(string filterName)
        {
            var filterIndex = _filters.FindIndex(f => f.FilterName == filterName);
            if (filterIndex < 0)
            {
                throw new AbpException("Unknown filter name: " + filterName + ". Be sure this filter is registered before.");
            }

            return filterIndex;
        }
    }
}