using System.Collections.Generic;

namespace Abp.Auditing
{
    /// <summary>
    /// List of selector functions to select classes/interfaces to be audited.
    /// 获取审计的类/接口的selector函数的列表
    /// </summary>
    public interface IAuditingSelectorList : IList<NamedTypeSelector>
    {
        /// <summary>
        /// Removes a selector by name.
        /// 通过名称移除一个selector
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        bool RemoveByName(string name);
    }
}