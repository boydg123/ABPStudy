namespace Abp.Organizations
{
    /// <summary>
    /// This interface is implemented entities those may have an <see cref="OrganizationUnit"/>.
    /// 此接口被那些可以有<see cref="OrganizationUnit"/>的实体实现
    /// </summary>
    public interface IMayHaveOrganizationUnit
    {
        /// <summary>
        /// <see cref="OrganizationUnit"/>'s Id which this entity belongs to.Can be null if this entity is not related to any <see cref="OrganizationUnit"/>.
        /// 实体的<see cref="OrganizationUnit"/> ID，如果实体没有关联任何<see cref="OrganizationUnit"/>，则可以为null。
        /// </summary>
        long? OrganizationUnitId { get; set; }
    }
}