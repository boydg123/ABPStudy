using System;
using System.Data.Entity;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Reflection;
using EntityFramework.DynamicFilters;

namespace Abp.EntityFramework.Uow
{
    /// <summary>
    /// 工作单元EF动态过滤器执行者
    /// </summary>
    public class EfDynamicFiltersUnitOfWorkFilterExecuter : IEfUnitOfWorkFilterExecuter
    {
        /// <summary>
        /// 应用禁用过滤器
        /// </summary>
        /// <param name="unitOfWork">工作单元</param>
        /// <param name="filterName">过滤器名称</param>
        public void ApplyDisableFilter(IUnitOfWork unitOfWork, string filterName)
        {
            foreach (var activeDbContext in unitOfWork.As<EfUnitOfWork>().GetAllActiveDbContexts())
            {
                activeDbContext.DisableFilter(filterName);
            }
        }

        /// <summary>
        /// 应用启用过滤器
        /// </summary>
        /// <param name="unitOfWork">工作单元</param>
        /// <param name="filterName">过滤器名称</param>
        public void ApplyEnableFilter(IUnitOfWork unitOfWork, string filterName)
        {
            foreach (var activeDbContext in unitOfWork.As<EfUnitOfWork>().GetAllActiveDbContexts())
            {
                activeDbContext.EnableFilter(filterName);
            }
        }

        /// <summary>
        /// 应用过滤器参数值
        /// </summary>
        /// <param name="unitOfWork">工作单元</param>
        /// <param name="filterName">过滤器名称</param>
        /// <param name="parameterName">参数名</param>
        /// <param name="value">用户设置参数的值</param>
        public void ApplyFilterParameterValue(IUnitOfWork unitOfWork, string filterName, string parameterName, object value)
        {
            foreach (var activeDbContext in unitOfWork.As<EfUnitOfWork>().GetAllActiveDbContexts())
            {
                if (TypeHelper.IsFunc<object>(value))
                {
                    activeDbContext.SetFilterScopedParameterValue(filterName, parameterName, (Func<object>)value);
                }
                else
                {
                    activeDbContext.SetFilterScopedParameterValue(filterName, parameterName, value);
                }
            }
        }

        /// <summary>
        /// 应用当前过滤器
        /// </summary>
        /// <param name="unitOfWork">工作单元</param>
        /// <param name="dbContext">数据库上下文</param>
        public void ApplyCurrentFilters(IUnitOfWork unitOfWork, DbContext dbContext)
        {
            foreach (var filter in unitOfWork.Filters)
            {
                if (filter.IsEnabled)
                {
                    dbContext.EnableFilter(filter.FilterName);
                }
                else
                {
                    dbContext.DisableFilter(filter.FilterName);
                }

                foreach (var filterParameter in filter.FilterParameters)
                {
                    if (TypeHelper.IsFunc<object>(filterParameter.Value))
                    {
                        dbContext.SetFilterScopedParameterValue(filter.FilterName, filterParameter.Key, (Func<object>)filterParameter.Value);
                    }
                    else
                    {
                        dbContext.SetFilterScopedParameterValue(filter.FilterName, filterParameter.Key, filterParameter.Value);
                    }
                }
            }
        }
    }
}
