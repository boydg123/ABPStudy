namespace Abp.Runtime.Validation
{
    /// <summary>
    /// This interface is used to normalize inputs before method execution.
    /// 此接口用于在方法执行之前对输入进行规范化。
    /// </summary>
    public interface IShouldNormalize
    {
        /// <summary>
        /// This method is called lastly before method execution (after validation if exists).
        /// 此方法在方法执行之前被调用（如果存在验证之后）
        /// </summary>
        void Normalize();
    }
}