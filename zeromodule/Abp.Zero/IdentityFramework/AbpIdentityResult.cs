using System.Collections.Generic;
using Microsoft.AspNet.Identity;

namespace Abp.IdentityFramework
{
    /// <summary>
    /// ABP 标识结果
    /// </summary>
    public class AbpIdentityResult : IdentityResult
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public AbpIdentityResult()
        {
            
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="errors"></param>
        public AbpIdentityResult(IEnumerable<string> errors)
            : base(errors)
        {
            
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="errors"></param>
        public AbpIdentityResult(params string[] errors)
            :base(errors)
        {
            
        }
        /// <summary>
        /// 失败结果
        /// </summary>
        /// <param name="errors">错误消息集合</param>
        /// <returns></returns>
        public static AbpIdentityResult Failed(params string[] errors)
        {
            return new AbpIdentityResult(errors);
        }
    }
}