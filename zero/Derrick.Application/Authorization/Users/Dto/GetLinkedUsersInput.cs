using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;

namespace Derrick.Authorization.Users.Dto
{
    /// <summary>
    /// 链接用户Input
    /// </summary>
    public class GetLinkedUsersInput : IPagedResultRequest, ISortedResultRequest, IShouldNormalize
    {
        /// <summary>
        /// 最大结果数量
        /// </summary>
        public int MaxResultCount { get; set; }
        /// <summary>
        /// 跳过数量
        /// </summary>
        public int SkipCount { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public string Sorting { get; set; }
        /// <summary>
        /// 规范化
        /// </summary>
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Username";
            }
        }
    }
}