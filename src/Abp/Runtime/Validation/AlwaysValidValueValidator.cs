using System;

namespace Abp.Runtime.Validation
{
    /// <summary>
    /// 总是有效值验证器
    /// </summary>
    [Validator("NULL")]
    [Serializable]
    public class AlwaysValidValueValidator : ValueValidatorBase
    {
        /// <summary>
        /// 是否有效
        /// </summary>
        /// <param name="value">值</param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            return true;
        }
    }
}