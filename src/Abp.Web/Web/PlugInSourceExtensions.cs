using System;
using System.Linq;
using System.Web.Compilation;
using Abp.Logging;
using Abp.PlugIns;

namespace Abp.Web
{
    /// <summary>
    /// 插件源列表
    /// </summary>
    public static class PlugInSourceListExtensions
    {
        /// <summary>
        /// 添加至编译管理器
        /// </summary>
        /// <param name="plugInSourceList">插件源列表</param>
        public static void AddToBuildManager(this PlugInSourceList plugInSourceList)
        {
            var plugInAssemblies = plugInSourceList
                .GetAllModules()
                .Select(m => m.Assembly)
                .Distinct()
                .ToList();

            foreach (var plugInAssembly in plugInAssemblies)
            {
                try
                {
                    LogHelper.Logger.Debug($"Adding {plugInAssembly.FullName} to BuildManager");
                    BuildManager.AddReferencedAssembly(plugInAssembly);
                }
                catch (Exception ex)
                {
                    LogHelper.Logger.Warn(ex.ToString(), ex);
                }
            }
        }
    }
}
