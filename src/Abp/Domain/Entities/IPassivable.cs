namespace Abp.Domain.Entities
{
    /// <summary>
    /// This interface is used to make an entity active/passive.
    /// 此接口用来标记实体 active/passive
    /// </summary>
    public interface IPassivable
    {
        /// <summary>
        /// True: This entity is active.False: This entity is not active.
        /// True: 实体激活。 False: 实体没激活
        /// </summary>
        bool IsActive { get; set; }
    }
}