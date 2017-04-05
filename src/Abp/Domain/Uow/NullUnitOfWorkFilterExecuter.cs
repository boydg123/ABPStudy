namespace Abp.Domain.Uow
{
    /// <summary>
    /// 工作单元Null对象过滤器执行器
    /// </summary>
    public class NullUnitOfWorkFilterExecuter : IUnitOfWorkFilterExecuter
    {
        /// <summary>
        /// 用于禁用过滤器
        /// </summary>
        /// <param name="unitOfWork">工作单元</param>
        /// <param name="filterName">过滤器名称</param>
        public void ApplyDisableFilter(IUnitOfWork unitOfWork, string filterName)
        {
            
        }

        /// <summary>
        /// 用于启用过滤器
        /// </summary>
        /// <param name="unitOfWork">工作单元</param>
        /// <param name="filterName">过滤器名称</param>
        public void ApplyEnableFilter(IUnitOfWork unitOfWork, string filterName)
        {
            
        }

        /// <summary>
        /// 用于过滤器参数值
        /// </summary>
        /// <param name="unitOfWork">工作单元</param>
        /// <param name="filterName">过滤器名称</param>
        /// <param name="parameterName">参数名</param>
        /// <param name="value">值</param>
        public void ApplyFilterParameterValue(IUnitOfWork unitOfWork, string filterName, string parameterName, object value)
        {
            
        }
    }
}