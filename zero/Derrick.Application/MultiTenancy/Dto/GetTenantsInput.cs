using Abp.Runtime.Validation;
using Derrick.Dto;

namespace Derrick.MultiTenancy.Dto
{
    /// <summary>
    /// 获取商户Input
    /// </summary>
    public class GetTenantsInput : PagedAndSortedInputDto, IShouldNormalize
    {
        /// <summary>
        /// 过滤条件
        /// </summary>
        public string Filter { get; set; }
        /// <summary>
        /// 标准化
        /// </summary>
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "TenancyName";
            }

            Sorting = Sorting.Replace("editionDisplayName", "Edition.DisplayName");
        }
    }
}

