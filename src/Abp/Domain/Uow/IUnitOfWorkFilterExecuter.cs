namespace Abp.Domain.Uow
{
    /// <summary>
    /// 工作单元过滤器执行
    /// </summary>
    public interface IUnitOfWorkFilterExecuter
    {
        /// <summary>
        /// 用于禁用过滤器
        /// </summary>
        /// <param name="unitOfWork">工作单元</param>
        /// <param name="filterName">过滤器名称</param>
        void ApplyDisableFilter(IUnitOfWork unitOfWork, string filterName);

        /// <summary>
        /// 用于启用过滤器
        /// </summary>
        /// <param name="unitOfWork">工作单元</param>
        /// <param name="filterName">过滤器名称</param>
        void ApplyEnableFilter(IUnitOfWork unitOfWork, string filterName);

        /// <summary>
        /// 用于过滤器参数值
        /// </summary>
        /// <param name="unitOfWork">工作单元</param>
        /// <param name="filterName">过滤器名称</param>
        /// <param name="parameterName">参数名</param>
        /// <param name="value">用户设置参数的值</param>
        void ApplyFilterParameterValue(IUnitOfWork unitOfWork, string filterName, string parameterName, object value);
    }
}
