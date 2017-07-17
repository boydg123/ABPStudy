using System.Collections.Generic;

namespace Derrick.Tenants.Dashboard.Dto
{
    /// <summary>
    /// 获取激活成员Output
    /// </summary>
    public class GetMemberActivityOutput
    {
        /// <summary>
        /// 成员总数量
        /// </summary>
        public List<int> TotalMembers { get; set; }
        /// <summary>
        /// 新成员数量
        /// </summary>
        public List<int> NewMembers { get; set; }
    }
}