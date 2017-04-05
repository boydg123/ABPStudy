namespace Abp.Domain.Entities
{
    /// <summary>
    /// Used to standardize soft deleting entities.Soft-delete entities are not actually deleted,
    /// marked as IsDeleted = true in the database,but can not be retrieved to the application.
    /// 用于规范软删除实体，软删除没有实际删除。
    /// 在数据库中标记IsDeleted = true,但是在应用中无法检索到
    /// </summary>
    public interface ISoftDelete
    {
        /// <summary>
        /// Used to mark an Entity as 'Deleted'. 
        /// 用来标记实体是否删除
        /// </summary>
        bool IsDeleted { get; set; }
    }
}
