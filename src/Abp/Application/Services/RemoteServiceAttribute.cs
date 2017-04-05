using System;
using System.Reflection;
using Abp.Reflection.Extensions;

namespace Abp.Application.Services
{
    /// <summary>
    /// 远程服务特性
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Method)]
    public class RemoteServiceAttribute : Attribute
    {
        /// <summary>
        /// 是否启用。 Default: true.
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 是否启用元数据。Default: true.
        /// </summary>
        public bool IsMetadataEnabled { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="isEnabled"></param>
        public RemoteServiceAttribute(bool isEnabled = true)
        {
            IsEnabled = isEnabled;
            IsMetadataEnabled = true;
        }

        /// <summary>
        /// 是否启用
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public virtual bool IsEnabledFor(Type type)
        {
            return IsEnabled;
        }

        /// <summary>
        /// 是否启用
        /// </summary>
        /// <param name="method">方法</param>
        /// <returns></returns>
        public virtual bool IsEnabledFor(MethodInfo method)
        {
            return IsEnabled;
        }

        /// <summary>
        /// 是否启用元数据
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public virtual bool IsMetadataEnabledFor(Type type)
        {
            return IsMetadataEnabled;
        }

        /// <summary>
        /// 是否启用元数据
        /// </summary>
        /// <param name="method">方法</param>
        /// <returns></returns>
        public virtual bool IsMetadataEnabledFor(MethodInfo method)
        {
            return IsMetadataEnabled;
        }

        /// <summary>
        /// 是否显示启用
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public static bool IsExplicitlyEnabledFor(Type type)
        {
            var remoteServiceAttr = type.GetSingleAttributeOrNull<RemoteServiceAttribute>();
            return remoteServiceAttr != null && remoteServiceAttr.IsEnabledFor(type);
        }

        /// <summary>
        /// 是否显示禁用
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public static bool IsExplicitlyDisabledFor(Type type)
        {
            var remoteServiceAttr = type.GetSingleAttributeOrNull<RemoteServiceAttribute>();
            return remoteServiceAttr != null && !remoteServiceAttr.IsEnabledFor(type);
        }

        /// <summary>
        /// 元数据是否显示启用
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public static bool IsMetadataExplicitlyEnabledFor(Type type)
        {
            var remoteServiceAttr = type.GetSingleAttributeOrNull<RemoteServiceAttribute>();
            return remoteServiceAttr != null && remoteServiceAttr.IsMetadataEnabledFor(type);
        }

        /// <summary>
        /// 元数据是否显示禁用
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public static bool IsMetadataExplicitlyDisabledFor(Type type)
        {
            var remoteServiceAttr = type.GetSingleAttributeOrNull<RemoteServiceAttribute>();
            return remoteServiceAttr != null && !remoteServiceAttr.IsMetadataEnabledFor(type);
        }

        /// <summary>
        /// 元数据是否显示禁用
        /// </summary>
        /// <param name="method">方法</param>
        /// <returns></returns>
        public static bool IsMetadataExplicitlyDisabledFor(MethodInfo method)
        {
            var remoteServiceAttr = method.GetSingleAttributeOrNull<RemoteServiceAttribute>();
            return remoteServiceAttr != null && !remoteServiceAttr.IsMetadataEnabledFor(method);
        }

        /// <summary>
        /// 元数据是否显示启用
        /// </summary>
        /// <param name="method">方法</param>
        /// <returns></returns>
        public static bool IsMetadataExplicitlyEnabledFor(MethodInfo method)
        {
            var remoteServiceAttr = method.GetSingleAttributeOrNull<RemoteServiceAttribute>();
            return remoteServiceAttr != null && remoteServiceAttr.IsMetadataEnabledFor(method);
        }
    }
}