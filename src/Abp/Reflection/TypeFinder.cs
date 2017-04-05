using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Abp.Collections.Extensions;
using Castle.Core.Logging;

namespace Abp.Reflection
{
    /// <summary>
    /// 类型查找器
    /// </summary>
    public class TypeFinder : ITypeFinder
    {
        /// <summary>
        /// 日志对象引用
        /// </summary>
        public ILogger Logger { get; set; }

        /// <summary>
        /// 程序集查找器
        /// </summary>
        private readonly IAssemblyFinder _assemblyFinder;
        private readonly object _syncObj = new object();
        private Type[] _types;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="assemblyFinder">程序集查找器</param>
        public TypeFinder(IAssemblyFinder assemblyFinder)
        {
            _assemblyFinder = assemblyFinder;
            Logger = NullLogger.Instance;
        }

        /// <summary>
        /// 类型查找
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <returns>类型集合</returns>
        public Type[] Find(Func<Type, bool> predicate)
        {
            return GetAllTypes().Where(predicate).ToArray();
        }

        /// <summary>
        /// 查找所有类型
        /// </summary>
        /// <returns>类型集合</returns>
        public Type[] FindAll()
        {
            return GetAllTypes().ToArray();
        }

        /// <summary>
        /// 获取所有类型
        /// </summary>
        /// <returns></returns>
        private Type[] GetAllTypes()
        {
            if (_types == null)
            {
                lock (_syncObj)
                {
                    if (_types == null)
                    {
                        _types = CreateTypeList().ToArray();
                    }
                }
            }

            return _types;
        }

        /// <summary>
        /// 创建类型集合
        /// </summary>
        /// <returns></returns>
        private List<Type> CreateTypeList()
        {
            var allTypes = new List<Type>();

            var assemblies = _assemblyFinder.GetAllAssemblies().Distinct();

            foreach (var assembly in assemblies)
            {
                try
                {
                    Type[] typesInThisAssembly;

                    try
                    {
                        typesInThisAssembly = assembly.GetTypes();
                    }
                    catch (ReflectionTypeLoadException ex)
                    {
                        typesInThisAssembly = ex.Types;
                    }

                    if (typesInThisAssembly.IsNullOrEmpty())
                    {
                        continue;
                    }

                    allTypes.AddRange(typesInThisAssembly.Where(type => type != null));
                }
                catch (Exception ex)
                {
                    Logger.Warn(ex.ToString(), ex);
                }
            }

            return allTypes;
        }
    }
}