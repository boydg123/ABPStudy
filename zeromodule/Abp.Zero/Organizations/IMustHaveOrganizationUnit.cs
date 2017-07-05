namespace Abp.Organizations
{
    /// <summary>
    /// This interface is implemented entities those must have an <see cref="OrganizationUnit"/>.
    /// 此接口被那些必须拥有<see cref="OrganizationUnit"/>的实体实现
    /// </summary>
    public interface IMustHaveOrganizationUnit
    {
        /// <summary>
        /// <see cref="OrganizationUnit"/>'s Id which this entity belongs to.
        /// 实体的<see cref="OrganizationUnit"/> ID
        /// </summary>
        long OrganizationUnitId { get; set; }
    }
}