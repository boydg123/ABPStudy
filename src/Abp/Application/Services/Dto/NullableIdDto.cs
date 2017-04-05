using System;

namespace Abp.Application.Services.Dto
{
    /// <summary>
    /// This DTO can be directly used (or inherited)to pass an nullable Id value to an application service method.
    /// 这个DTO可以直接使用（或继承）通过一个空ID值应用服务的方法
    /// </summary>
    /// <typeparam name="TId">Type of the Id / 类型ID</typeparam>
    [Serializable]
    public class NullableIdDto<TId>
        where TId : struct
    {
        /// <summary>
        /// 类型ID
        /// </summary>
        public TId? Id { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public NullableIdDto()
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="id">类型ID</param>
        public NullableIdDto(TId? id)
        {
            Id = id;
        }
    }

    /// <summary>
    /// A shortcut of <see cref="NullableIdDto{TId}"/> for <see cref="int"/>.
    /// <see cref="NullableIdDto{TId}"/> <see cref="int"/> 的快捷方式
    /// </summary>
    [Serializable]
    public class NullableIdDto : NullableIdDto<int>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public NullableIdDto()
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="id"></param>
        public NullableIdDto(int? id)
            : base(id)
        {

        }
    }
}