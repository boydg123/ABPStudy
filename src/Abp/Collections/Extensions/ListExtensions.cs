using System;
using System.Collections.Generic;

namespace Abp.Collections.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="IList{T}"/>.
    /// <see cref="IList{T}"/>扩展方法
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// Sort a list by a topological sorting, which consider their dependencies
        /// 通过拓扑关系排序，这里的拓扑关系指元素间的依赖关系
        /// </summary>
        /// <typeparam name="T">The type of the members of values. / 列表中元素的类型</typeparam>
        /// <param name="source">A list of objects to sort / 需要排序的列表</param>
        /// <param name="getDependencies">Function to resolve the dependencies / 解析依赖关系的委托</param>
        /// <returns></returns>
        public static List<T> SortByDependencies<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> getDependencies)
        {
            /* See: http://www.codeproject.com/Articles/869059/Topological-sorting-in-Csharp
             *      http://en.wikipedia.org/wiki/Topological_sorting
             */

            var sorted = new List<T>();
            var visited = new Dictionary<T, bool>();

            foreach (var item in source)
            {
                SortByDependenciesVisit(item, getDependencies, sorted, visited);
            }

            return sorted;
        }

        /// <summary>
        /// 通过拓扑关系排序，这里的拓扑关系指元素间的依赖关系
        /// </summary>
        /// <typeparam name="T">The type of the members of values. / 元素类型</typeparam>
        /// <param name="item">Item to resolve / 需要解析的项</param>
        /// <param name="getDependencies">Function to resolve the dependencies / 解析依赖关系的委托</param>
        /// <param name="sorted">List with the sortet items / 已排序的列表</param>
        /// <param name="visited">Dictionary with the visited items / 存放已访问项的字典</param>
        private static void SortByDependenciesVisit<T>(T item, Func<T, IEnumerable<T>> getDependencies, List<T> sorted, Dictionary<T, bool> visited)
        {
            bool inProcess;
            var alreadyVisited = visited.TryGetValue(item, out inProcess);

            if (alreadyVisited)
            {
                if (inProcess)
                {
                    throw new ArgumentException("Cyclic dependency found! Item: " + item);
                }
            }
            else
            {
                visited[item] = true;

                var dependencies = getDependencies(item);
                if (dependencies != null)
                {
                    foreach (var dependency in dependencies)
                    {
                        SortByDependenciesVisit(dependency, getDependencies, sorted, visited);
                    }
                }

                visited[item] = false;
                sorted.Add(item);
            }
        }
    }
}
