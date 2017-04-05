using System;
using System.Collections.Generic;
using Abp.Extensions;

namespace Abp.Domain.Entities
{
    /// <summary>
    /// A shortcut of <see cref="Entity{TPrimaryKey}"/> for most used primary key type (<see cref="int"/>).
    /// 多数情况(<see cref="int"/>)作为主键类型时，使用<see cref="Entity{TPrimaryKey}"/> 的快捷方式.
    /// </summary>
    [Serializable]
    public abstract class Entity : Entity<int>, IEntity
    {

    }

    /// <summary>
    /// Basic implementation of IEntity interface. An entity can inherit this class of directly implement to IEntity interface. 
    /// IEntity接口的基本实现.实体可直接继承此类来实现IEntity接口
    /// </summary>
    /// <typeparam name="TPrimaryKey">Type of the primary key of the entity / 实体的主键类型</typeparam>
    [Serializable]
    public abstract class Entity<TPrimaryKey> : IEntity<TPrimaryKey>
    {
        /// <summary>
        /// Unique identifier for this entity.
        /// 实体的唯一标识
        /// </summary>
        public virtual TPrimaryKey Id { get; set; }

        /// <summary>
        /// Checks if this entity is transient (it has not an Id).
        /// 检查实体是否为临时实体(没有Id)
        /// </summary>
        /// <returns>True, if this entity is transient / 如果是临时实体则返回true</returns>
        public virtual bool IsTransient()
        {
            if (EqualityComparer<TPrimaryKey>.Default.Equals(Id, default(TPrimaryKey)))
            {
                return true;
            }

            //Workaround for EF Core since it sets int/long to min value when attaching to dbcontext
            if (typeof(TPrimaryKey) == typeof(int))
            {
                return Convert.ToInt32(Id) <= 0;
            }

            if (typeof(TPrimaryKey) == typeof(long))
            {
                return Convert.ToInt64(Id) <= 0;
            }

            return false;
        }

        /// <summary>
        /// 相等比较
        /// </summary>
        /// <param name="obj">obj对象</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Entity<TPrimaryKey>))
            {
                return false;
            }

            //Same instances must be considered as equal
            //相同的实例被认为是相等的
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            //Transient objects are not considered as equal
            //临时对象被认为是不相等的
            var other = (Entity<TPrimaryKey>)obj;
            if (IsTransient() && other.IsTransient())
            {
                return false;
            }

            //Must have a IS-A relation of types or must be same type
            //必须有IS-A关系或者相同类型
            var typeOfThis = GetType();
            var typeOfOther = other.GetType();
            if (!typeOfThis.IsAssignableFrom(typeOfOther) && !typeOfOther.IsAssignableFrom(typeOfThis))
            {
                return false;
            }

            if (this is IMayHaveTenant && other is IMayHaveTenant &&
                this.As<IMayHaveTenant>().TenantId != other.As<IMayHaveTenant>().TenantId)
            {
                return false;
            }

            if (this is IMustHaveTenant && other is IMustHaveTenant &&
                this.As<IMustHaveTenant>().TenantId != other.As<IMustHaveTenant>().TenantId)
            {
                return false;
            }

            return Id.Equals(other.Id);
        }

        /// <summary>
        /// 获取Hash Code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        /// <summary>
        /// ==操作符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(Entity<TPrimaryKey> left, Entity<TPrimaryKey> right)
        {
            if (Equals(left, null))
            {
                return Equals(right, null);
            }

            return left.Equals(right);
        }

        /// <summary>
        /// != 操作符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(Entity<TPrimaryKey> left, Entity<TPrimaryKey> right)
        {
            return !(left == right);
        }

        /// <summary>
        /// 重写ToString方法
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"[{GetType().Name} {Id}]";
        }
    }
}
