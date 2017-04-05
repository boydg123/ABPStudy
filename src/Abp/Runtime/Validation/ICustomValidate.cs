namespace Abp.Runtime.Validation
{
    /// <summary>
    /// Defines interface that must be implemented by classes those must be validated with custom rules.So, implementing class can define it's own validation logic.
    /// 为需要自定义验证规则的类定义接口，因此实现此类能定义自己的验证逻辑
    /// </summary>
    public interface ICustomValidate
    {
        /// <summary>
        /// This method is used to validate the object.
        /// 此方法用于验证对象
        /// </summary>
        /// <param name="context">Validation context. / 自定义验证上下文</param>
        void AddValidationErrors(CustomValidationContext context);
    }
}