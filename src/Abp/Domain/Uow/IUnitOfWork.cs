using System;

namespace Abp.Domain.Uow
{
    /// <summary>
    /// Defines a unit of work.This interface is internally used by ABP.
    /// 定义一个工作单元,此接口在ABP内部使用
    /// Use <see cref="IUnitOfWorkManager.Begin()"/> to start a new unit of work.
    /// 使用 <see cref="IUnitOfWorkManager"/> 开启一个新的工作单元
    /// </summary>
    public interface IUnitOfWork : IActiveUnitOfWork, IUnitOfWorkCompleteHandle
    {
        /// <summary>
        /// Unique id of this UOW.
        /// UOW唯一的ID
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Reference to the outer UOW if exists.
        /// 外部UOW的引用，如果存在
        /// </summary>
        IUnitOfWork Outer { get; set; }
        
        /// <summary>
        /// Begins the unit of work with given options.
        /// 使用给定的选项开启工作单元
        /// </summary>
        /// <param name="options">Unit of work options</param>
        void Begin(UnitOfWorkOptions options);
    }
}