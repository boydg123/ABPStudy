using System;
using Abp.Modules;
using Abp.Threading;
using Owin;

namespace Abp.Owin
{
    /// <summary>
    /// OWIN extension methods for ABP.
    /// ABP OWIN模块的扩展方法
    /// </summary>
    public static class AbpOwinExtensions
    {
        /// <summary>
        /// This should be called as the first line for OWIN based applications for ABP framework.
        /// 这个方法应在第一行被调用在以OWIN为基础应用的ABP框架中
        /// </summary>
        /// <param name="app">The application.</param>
        public static void UseAbp(this IAppBuilder app)
        {
            ThreadCultureSanitizer.Sanitize();
        }

        /// <summary>
        /// Use this extension method if you don't initialize ABP in other way.Otherwise, use <see cref="UseAbp"/>.
        /// 如果不以其他方式初始化ABP，请使用此扩展方法。否则，使用<see cref="UseAbp"/>
        /// </summary>
        /// <param name="app">The application. / 应用程序</param>
        /// <typeparam name="TStartupModule">The type of the startup module. / 启动模块的类型</typeparam>
        public static void UseAbp<TStartupModule>(this IAppBuilder app)
            where TStartupModule : AbpModule
        {
            app.UseAbp<TStartupModule>(abpBootstrapper => { });
        }

        /// <summary>
        /// Use this extension method if you don't initialize ABP in other way.Otherwise, use <see cref="UseAbp"/>.
        /// 如果不以其他方式初始化ABP，请使用此扩展方法。否则，使用<see cref="UseAbp"/>
        /// </summary>
        /// <param name="app">The application. / 应用程序</param>
        /// <param name="configureAction">处理ABP启动模块的方法</param>
        /// <typeparam name="TStartupModule">The type of the startup module. / 启动模块的类型</typeparam>
        public static void UseAbp<TStartupModule>(this IAppBuilder app, Action<AbpBootstrapper> configureAction)
            where TStartupModule : AbpModule
        {
            app.UseAbp();

            var abpBootstrapper = app.Properties["_AbpBootstrapper.Instance"] as AbpBootstrapper;
            if (abpBootstrapper == null)
            {
                abpBootstrapper = AbpBootstrapper.Create<TStartupModule>();
                configureAction(abpBootstrapper);
                abpBootstrapper.Initialize();
            }
        }
    }
}