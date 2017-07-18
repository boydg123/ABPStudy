using Abp.Domain.Entities;

namespace Abp.Test.Domain.Entities
{
    /// <summary>
    /// 部门实体
    /// </summary>
    public class Department : Entity
    {
        /// <summary>
        /// 部门名称
        /// </summary>
        public string Name { get; set; }
    }
}
