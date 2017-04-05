using System.Collections.Generic;
using System.Reflection;

namespace Abp.Reflection
{
    /// <summary>
    /// This interface is used to get assemblies in the application.
    /// 这个接口用于获取整个应用程序所有的程序集
    /// It may not return all assemblies, but those are related with modules.
    /// 它可能无法返回所有程序集，但这些与模块相关
    /// </summary>
    public interface IAssemblyFinder
    {
        /// <summary>
        /// Gets all assemblies.
        /// 获取所有程序集
        /// </summary>
        /// <returns>List of assemblies / 程序集列表集合</returns>
        List<Assembly> GetAllAssemblies();
    }
}