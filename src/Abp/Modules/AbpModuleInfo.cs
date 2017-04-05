using System;
using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;

namespace Abp.Modules
{
    /// <summary>
    /// Used to store all needed information for a module.
    /// 用来存储一个模块所有信息
    /// </summary>
    public class AbpModuleInfo
    {
        /// <summary>
        /// The assembly which contains the module definition.
        /// 包含模块定义的程序集
        /// </summary>
        public Assembly Assembly { get; }

        /// <summary>
        /// Type of the module.
        /// 模块的类型
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// Instance of the module.
        /// 模块实例
        /// </summary>
        public AbpModule Instance { get; }

        /// <summary>
        /// All dependent modules of this module.
        /// 此模块的所有依赖模块
        /// </summary>
        public List<AbpModuleInfo> Dependencies { get; }

        /// <summary>
        /// Creates a new AbpModuleInfo object.
        /// 创建一个新的AbpModuleInfo对象.
        /// </summary>
        public AbpModuleInfo([NotNull] Type type, [NotNull] AbpModule instance)
        {
            Check.NotNull(type, nameof(type));
            Check.NotNull(instance, nameof(instance));

            Type = type;
            Instance = instance;
            Assembly = Type.Assembly;

            Dependencies = new List<AbpModuleInfo>();
        }

        /// <summary>
        /// 重写ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Type.AssemblyQualifiedName ??
                   Type.FullName;
        }
    }
}