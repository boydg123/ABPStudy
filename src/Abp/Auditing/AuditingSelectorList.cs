using System.Collections.Generic;

namespace Abp.Auditing
{
    /// <summary>
    /// 获取审计的类/接口的Selector委托列表
    /// </summary>
    internal class AuditingSelectorList : List<NamedTypeSelector>, IAuditingSelectorList
    {
        /// <summary>
        /// 通过名称移除一个selector
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>是否移除成功</returns>
        public bool RemoveByName(string name)
        {
            return RemoveAll(s => s.Name == name) > 0;
        }
    }
}