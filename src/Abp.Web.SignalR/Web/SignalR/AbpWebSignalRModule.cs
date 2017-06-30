using System.Reflection;
using Abp.Modules;
using Castle.MicroKernel.Registration;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;

namespace Abp.Web.SignalR
{
    /// <summary>
    /// ABP SignalR integration module.
    /// ABP SignalR 集成模块
    /// </summary>
    [DependsOn(typeof(AbpKernelModule))]
    public class AbpWebSignalRModule : AbpModule
    {
        /// <summary>
        /// 初始化前
        /// </summary>
        public override void PreInitialize()
        {
            GlobalHost.DependencyResolver = new WindsorDependencyResolver(IocManager.IocContainer);
            UseAbpSignalRContractResolver();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }

        /// <summary>
        /// 使用Abp SignalR规则解析器
        /// </summary>
        private void UseAbpSignalRContractResolver()
        {
            var serializer = JsonSerializer.Create(
                new JsonSerializerSettings
                {
                    ContractResolver = new AbpSignalRContractResolver()
                });
            
            IocManager.IocContainer.Register(
                Component.For<JsonSerializer>().UsingFactoryMethod(() => serializer)
                );
        }
    }
}
