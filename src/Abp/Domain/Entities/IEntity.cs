namespace Abp.Domain.Entities
{
    /// <summary>
    /// A shortcut of <see cref="IEntity{TPrimaryKey}"/> for most used primary key type (<see cref="int"/>).
    /// 一个快捷方式<see cref="IEntity{TPrimaryKey}"/>为大部分使用(<see cref="int"/>)类型作为主键
    /// </summary>
    public interface IEntity : IEntity<int>
    {

    }
}