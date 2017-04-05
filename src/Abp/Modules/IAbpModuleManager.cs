using System;
using System.Collections.Generic;

namespace Abp.Modules
{
    /// <summary>
    /// ABP模块管理器接口
    /// </summary>
    public interface IAbpModuleManager
    {
        /// <summary>
        /// 启动模块的模块信息
        /// </summary>
        AbpModuleInfo StartupModule { get; }

        /// <summary>
        /// 模块信息只读列表
        /// </summary>
        IReadOnlyList<AbpModuleInfo> Modules { get; }

        /// <summary>
        /// 初始化模块
        /// </summary>
        /// <param name="startupModule"></param>
        void Initialize(Type startupModule);

        /// <summary>
        /// 启动模块
        /// </summary>
        void StartModules();

        /// <summary>
        /// 关闭模块
        /// </summary>
        void ShutdownModules();
    }
}