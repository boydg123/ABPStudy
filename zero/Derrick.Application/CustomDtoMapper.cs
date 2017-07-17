using AutoMapper;
using Derrick.Authorization.Users;
using Derrick.Authorization.Users.Dto;

namespace Derrick
{
    /// <summary>
    /// 自定义Dto映射器
    /// </summary>
    internal static class CustomDtoMapper
    {
        /// <summary>
        /// 映射前
        /// </summary>
        private static volatile bool _mappedBefore;
        /// <summary>
        /// 同步对象
        /// </summary>
        private static readonly object SyncObj = new object();
        /// <summary>
        /// 创建映射
        /// </summary>
        /// <param name="mapper">映射配置表达式</param>
        public static void CreateMappings(IMapperConfigurationExpression mapper)
        {
            lock (SyncObj)
            {
                if (_mappedBefore)
                {
                    return;
                }

                CreateMappingsInternal(mapper);

                _mappedBefore = true;
            }
        }
        /// <summary>
        /// 内部创建映射
        /// </summary>
        /// <param name="mapper">映射配置表达式</param>
        private static void CreateMappingsInternal(IMapperConfigurationExpression mapper)
        {
            mapper.CreateMap<User, UserEditDto>()
                .ForMember(dto => dto.Password, options => options.Ignore())
                .ReverseMap()
                .ForMember(user => user.Password, options => options.Ignore());
        }
    }
}