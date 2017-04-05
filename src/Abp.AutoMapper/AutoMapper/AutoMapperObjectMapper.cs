using Abp.Dependency;
using AutoMapper;
using IObjectMapper = Abp.ObjectMapping.IObjectMapper;

namespace Abp.AutoMapper
{
    /// <summary>
    /// AutoMapper 对象映射器
    /// </summary>
    public class AutoMapperObjectMapper : IObjectMapper, ISingletonDependency
    {
        /// <summary>
        /// AutoMapper映射器
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mapper">AutoMapper映射器</param>
        public AutoMapperObjectMapper(IMapper mapper)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// 转换一个对象至另一个对象。创建一个新的<see cref="TDestination"/>对象。
        /// </summary>
        /// <typeparam name="TDestination">目标对象的类型</typeparam>
        /// <param name="source">原对象</param>
        /// <returns></returns>
        public TDestination Map<TDestination>(object source)
        {
            return _mapper.Map<TDestination>(source);
        }

        /// <summary>
        /// 执行一个映射从原对象到一个已存在的目标对象
        /// </summary>
        /// <typeparam name="TSource">原对象类型</typeparam>
        /// <typeparam name="TDestination">目标对象类型</typeparam>
        /// <param name="source">原对象</param>
        /// <param name="destination">目标对象</param>
        /// <returns>映射操作后的对象</returns>
        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            return _mapper.Map(source, destination);
        }
    }
}
