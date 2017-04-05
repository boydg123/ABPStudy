using System;

namespace Abp
{
    /// <summary>
    /// Used to represent a named type selector.
    /// 表示一个命名的selector
    /// </summary>
    public class NamedTypeSelector
    {
        /// <summary>
        /// Name of the selector.
        /// 查找器的名称
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Predicate.
        /// 查找条件
        /// </summary>
        public Func<Type, bool> Predicate { get; set; }

        /// <summary>
        /// Creates new <see cref="NamedTypeSelector"/> object.
        /// 创建一个 <see cref="NamedTypeSelector"/> 对象.
        /// </summary>
        /// <param name="name">Name / 查找器的名称</param>
        /// <param name="predicate">Predicate / 查找条件</param>
        public NamedTypeSelector(string name, Func<Type, bool> predicate)
        {
            Name = name;
            Predicate = predicate;
        }
    }
}