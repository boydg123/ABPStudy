using System;

namespace Abp.Modules
{
    /// <summary>
    /// Used to define dependencies of an ABP module to other modules.It should be used for a class derived from <see cref="AbpModule"/>.
    /// 用于定义abp模块间的依赖.它用于继承自 <see cref="AbpModule"/>的类.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class DependsOnAttribute : Attribute
    {
        /// <summary>
        /// Types of depended modules.
        /// 依赖模块的类型
        /// </summary>
        public Type[] DependedModuleTypes { get; private set; }

        /// <summary>
        /// Used to define dependencies of an ABP module to other modules.
        /// 用于定义abp模块间的依赖
        /// </summary>
        /// <param name="dependedModuleTypes">Types of depended modules / 依赖模块的类型</param>
        public DependsOnAttribute(params Type[] dependedModuleTypes)
        {
            DependedModuleTypes = dependedModuleTypes;
        }
    }
}