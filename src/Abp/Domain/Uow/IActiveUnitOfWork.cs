using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Abp.Domain.Uow
{
    /// <summary>
    /// This interface is used to work with active unit of work.This interface can not be injected.Use <see cref="IUnitOfWorkManager"/> instead.
    /// 此接口用来激活工作单元，此接口不能被注入。使用 <see cref="IUnitOfWorkManager"/> 代替.
    /// </summary>
    public interface IActiveUnitOfWork
    {
        /// <summary>
        /// This event is raised when this UOW is successfully completed.
        /// 该事件在工作单元成功完成时被激发
        /// </summary>
        event EventHandler Completed;

        /// <summary>
        /// This event is raised when this UOW is failed.
        /// 该事件在工作单元失败时被激发
        /// </summary>
        event EventHandler<UnitOfWorkFailedEventArgs> Failed;

        /// <summary>
        /// This event is raised when this UOW is disposed.
        /// 该事件在工作单元销毁时被激发
        /// </summary>
        event EventHandler Disposed;

        /// <summary>
        /// Gets if this unit of work is transactional.
        /// 获取工作单元是否是事务性的
        /// </summary>
        UnitOfWorkOptions Options { get; }

        /// <summary>
        /// Gets data filter configurations for this unit of work.
        /// 从工作单元获取数据过滤器配置
        /// </summary>
        IReadOnlyList<DataFilterConfiguration> Filters { get; }

        /// <summary>
        /// Is this UOW disposed?
        /// 此工作单元是否销毁
        /// </summary>
        bool IsDisposed { get; }

        /// <summary>
        /// Saves all changes until now in this unit of work.
        /// 保存工作单元中所有的修改
        /// This method may be called to apply changes whenever needed.
        /// 这个方法在需要应用修改时调用
        /// Note that if this unit of work is transactional, saved changes are also rolled back if transaction is rolled back.
        /// No explicit call is needed to SaveChanges generally, 
        /// 注意，如果工作单元是事务性的，不仅保存变更，还会在事务失败时，回滚事务。
        /// since all changes saved at end of a unit of work automatically.
        /// 一般不用显式调用SaveChages，因为工作单元会自动保存所有变更
        /// </summary>
        void SaveChanges();

        /// <summary>
        /// Saves all changes until now in this unit of work.
        /// 保存工作单元中所有的修改
        /// This method may be called to apply changes whenever needed.
        /// 这个方法在需要应用修改时调用
        /// Note that if this unit of work is transactional, saved changes are also rolled back if transaction is rolled back.
        /// No explicit call is needed to SaveChanges generally, 
        /// 注意，如果工作单元是事务性的，不仅保存变更，还会在事务失败时，回滚事务。
        /// since all changes saved at end of a unit of work automatically.
        /// 一般不用显式调用SaveChages，因为工作单元会自动保存所有变更
        /// </summary>
        Task SaveChangesAsync();

        /// <summary>
        /// Disables one or more data filters.
        /// 禁用一个或多个数据过滤器
        /// Does nothing for a filter if it's already disabled. 
        /// 如果一个过滤器被禁用，它将不会有作任何操作
        /// Use this method in a using statement to re-enable filters if needed.
        /// 使用using语句来使用该方法，可以在需要时启用过滤器
        /// </summary>
        /// <param name="filterNames">One or more filter names. <see cref="AbpDataFilters"/> for standard filters. / 一个或多个标准过滤器的名称</param>
        /// <returns>A <see cref="IDisposable"/> handle to take back the disable effect. / 能收回禁用效果</returns>
        IDisposable DisableFilter(params string[] filterNames);

        /// <summary>
        /// Enables one or more data filters.
        /// 启用一个或多个数据过滤器
        /// Does nothing for a filter if it's already enabled.
        /// 如果一个过滤器被启用，它将不会有作任何操作
        /// Use this method in a using statement to re-disable filters if needed.
        /// 使用using语句来使用该方法，可以在需要时禁用过滤器
        /// </summary>
        /// <param name="filterNames">One or more filter names. <see cref="AbpDataFilters"/> for standard filters. / 一个或多个标准过滤器的名称</param>
        /// <returns>A <see cref="IDisposable"/> handle to take back the enable effect. / 能收回启用效果</returns>
        IDisposable EnableFilter(params string[] filterNames);

        /// <summary>
        /// Checks if a filter is enabled or not.
        /// 检查一个过滤器是否被禁用
        /// </summary>
        /// <param name="filterName">Name of the filter. <see cref="AbpDataFilters"/> for standard filters. / <see cref="AbpDataFilters"/>过滤器的名称</param>
        bool IsFilterEnabled(string filterName);

        /// <summary>
        /// Sets (overrides) value of a filter parameter.
        /// 设置（重载）过滤器参数的名称
        /// </summary>
        /// <param name="filterName">Name of the filter / 过滤器名称</param>
        /// <param name="parameterName">Parameter's name / 参数名称</param>
        /// <param name="value">Value of the parameter to be set / 要给参数设置的值</param>
        IDisposable SetFilterParameter(string filterName, string parameterName, object value);

        /// <summary>
        /// Sets/Changes Tenant's Id for this UOW.
        /// 设置/修改当前工作单元租户的ID
        /// </summary>
        /// <param name="tenantId">The tenant id. / 租户ID</param>
        IDisposable SetTenantId(int? tenantId);

        /// <summary>
        /// Gets Tenant Id for this UOW.
        /// 获取当前工作单元租户的ID
        /// </summary>
        /// <returns></returns>
        int? GetTenantId();
    }
}