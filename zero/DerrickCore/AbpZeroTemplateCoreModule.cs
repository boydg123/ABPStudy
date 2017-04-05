using System;
using System.Reflection;
using Abp.AutoMapper;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Modules;
using Abp.Net.Mail;
using Abp.Runtime.Session;
using Abp.Zero;
using Abp.Zero.Configuration;
using Abp.Zero.Ldap;
using Derrick.Authorization.Roles;
using Derrick.Authorization.Users;
using Derrick.Chat;
using Derrick.Configuration;
using Derrick.Debugging;
using Derrick.Features;
using Derrick.Friendships;
using Derrick.Friendships.Cache;
using Derrick.MultiTenancy;
using Derrick.Notifications;
using Derrick.Runtime.Session;

namespace Derrick
{
    /// <summary>
    /// Core (domain) module of the application.
    /// </summary>
    [DependsOn(typeof(AbpZeroCoreModule), typeof(AbpZeroLdapModule), typeof(AbpAutoMapperModule))]
    public class AbpZeroTemplateCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Auditing.IsEnabledForAnonymousUsers = true;

            //Declare entity types
            Configuration.Modules.Zero().EntityTypes.Tenant = typeof (Tenant);
            Configuration.Modules.Zero().EntityTypes.Role = typeof (Role);
            Configuration.Modules.Zero().EntityTypes.User = typeof (User);

            //Add/remove localization sources
            Configuration.Localization.Sources.Add(
                new DictionaryBasedLocalizationSource(
                    "AbpZeroTemplate",
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        Assembly.GetExecutingAssembly(),
                        "Derrick.Localization.AbpZeroTemplate"
                        )
                    )
                );

            //Adding feature providers
            Configuration.Features.Providers.Add<AppFeatureProvider>();

            //Adding setting providers
            Configuration.Settings.Providers.Add<AppSettingProvider>();

            //Adding notification providers
            Configuration.Notifications.Providers.Add<AppNotificationProvider>();

            //Enable this line to create a multi-tenant application.
            Configuration.MultiTenancy.IsEnabled = true;

            //Enable LDAP authentication (It can be enabled only if MultiTenancy is disabled!)
            //Configuration.Modules.ZeroLdap().Enable(typeof(AppLdapAuthenticationSource));

            //Configure roles
            AppRoleConfig.Configure(Configuration.Modules.Zero().RoleManagement);

            if (DebugHelper.IsDebug)
            {
                //Disabling email sending in debug mode
                IocManager.Register<IEmailSender, NullEmailSender>(DependencyLifeStyle.Transient);
            }

            Configuration.ReplaceService<IAbpSession, AspNetZeroAbpSession>();

            Configuration.Caching.Configure(FriendCacheItem.CacheName, cache =>
            {
                cache.DefaultSlidingExpireTime = TimeSpan.FromMinutes(30);
            });
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }

        public override void PostInitialize()
        {
            IocManager.RegisterIfNot<IChatCommunicator, NullChatCommunicator>();
            IocManager.Resolve<ChatUserStateWatcher>().Initialize();
        }
    }
}
