using Abp.Domain.Entities;

namespace ABP.Test.Domain.Entities
{
    /// <summary>
    /// 员工实体
    /// </summary>
    public class Worker : Entity
    {
        /// <summary>
        /// 员工名称
        /// </summary>
        public string Name { get; set; }
    }
}
