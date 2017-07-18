using Abp.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using Abp.Runtime.Caching.Memory;
using Abp.Runtime.Caching.Configuration;
using Abp.Configuration.Startup;
using Xunit;
using Shouldly;
using Abp;

namespace Abp.Test.Configuration
{
    /// <summary>
    /// 设置管理测试
    /// </summary>
    public class SettingManaer_Tests : TestBaseWithLocalManager
    {
        private const string MyAppLevelSetting = "MyAppLevelSetting";
        private const string MyAllLevelsSetting = "MyAllLevelsSetting";
        private const string MyNotInheritedSetting = "MyNotInheritedSetting";

        /// <summary>
        /// 获取一个设置管理器
        /// </summary>
        private SettingManager CreateManager()
        {
            return new SettingManager(CreateMockSettingDefinitionManager(), new AbpMemoryCacheManager(localIocManager, new CachingConfiguration(Substitute.For<IAbpStartupConfiguration>())));
        }

        /// <summary>
        /// 在没有存储没有Session时候获取的是默认值
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Should_Get_Default_Values_With_No_Store_And_No_Session()
        {
            var settingManager = CreateManager();

            (await settingManager.GetSettingValueAsync<int>(MyAppLevelSetting)).ShouldBe(42);
            (await settingManager.GetSettingValueAsync(MyAllLevelsSetting)).ShouldBe("application level defalut value");
        }

        /// <summary>
        /// 在没有Session时，从设置存储对象获取设置。
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Should_Get_Stored_Application_Value_With_No_Session()
        {
            var settingManager = CreateManager();
            settingManager.SettingStore = new MemorySettingStore();
            (await settingManager.GetSettingValueAsync<int>(MyAppLevelSetting)).ShouldBe(48);
            (await settingManager.GetSettingValueAsync(MyAllLevelsSetting)).ShouldBe("Application level stored value");
        }

        /// <summary>
        /// 获取正确的值
        /// </summary>
        [Fact]
        public async Task Should_Get_Correct_Values()
        {
            var session = new MyChangableSession();

            var settingManager = CreateManager();
            settingManager.SettingStore = new MemorySettingStore();
            settingManager.AbpSession = session;

            session.TenantId = 1;
            //继承测试

            session.UserId = 1;
            (await settingManager.GetSettingValueAsync(MyAllLevelsSetting)).ShouldBe("user 1 stored value");

            session.UserId = 2;
            (await settingManager.GetSettingValueAsync(MyAllLevelsSetting)).ShouldBe("user 2 stored value");
            //没有找到userid 为 3的设置，直接找的租户为1，用户为null的设置
            session.UserId = 3;
            (await settingManager.GetSettingValueAsync(MyAllLevelsSetting)).ShouldBe("tenant 1 stored value");
            //没有租户3用户3的设置，直接假设两个为null
            session.TenantId = 3;
            session.UserId = 3;
            (await settingManager.GetSettingValueAsync(MyAllLevelsSetting)).ShouldBe("Application level stored value");
            //TODO: 没大搞懂
            session.TenantId = 1;
            session.UserId = 1;
            (await settingManager.GetSettingValueForApplicationAsync(MyNotInheritedSetting)).ShouldBe("application value");
            (await settingManager.GetSettingValueForTenantAsync(MyNotInheritedSetting, session.TenantId.Value)).ShouldBe("default-value");
            (await settingManager.GetSettingValueAsync(MyNotInheritedSetting)).ShouldBe("default-value");
        }

        /// <summary>
        /// 获取所有设置的测试
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Should_Get_All_Values()
        {
            var settingManager = CreateManager();
            settingManager.SettingStore = new MemorySettingStore();
            var list = await settingManager.GetAllSettingValuesAsync();
            (await settingManager.GetAllSettingValuesAsync()).Count.ShouldBe(3);
            var listApp = await settingManager.GetAllSettingValuesForApplicationAsync();
            (await settingManager.GetAllSettingValuesForApplicationAsync()).Count.ShouldBe(3);
            //测试租户1的设置
            (await settingManager.GetAllSettingValuesForTenantAsync(1)).Count.ShouldBe(1);
            (await settingManager.GetAllSettingValuesForTenantAsync(2)).Count.ShouldBe(0);
            (await settingManager.GetAllSettingValuesForTenantAsync(3)).Count.ShouldBe(0);
            //测试租户1，用户分别为1和2的设置
            (await settingManager.GetAllSettingValuesForUserAsync(new UserIdentifier(1, 1))).Count.ShouldBe(1);
            (await settingManager.GetAllSettingValuesForUserAsync(new UserIdentifier(1, 2))).Count.ShouldBe(1);
            (await settingManager.GetAllSettingValuesForUserAsync(new UserIdentifier(1, 3))).Count.ShouldBe(0);
        }

        /// <summary>
        /// 修改设置值测试
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Should_Change_Setting_Values()
        {
            var session = new MyChangableSession();
            var settingManager = CreateManager();
            settingManager.SettingStore = new MemorySettingStore();
            settingManager.AbpSession = session;
            //应用程序级别修改
            await settingManager.ChangeSettingForApplicationAsync(MyAppLevelSetting, "53");
            await settingManager.ChangeSettingForApplicationAsync(MyAppLevelSetting, "54"); //修改两次应用程序级别设置
            await settingManager.ChangeSettingForApplicationAsync(MyAllLevelsSetting, "应用程序级别修改值");

            (await settingManager.SettingStore.GetSettingOrNullAsync(null, null, MyAppLevelSetting)).Value.ShouldBe("54");
            (await settingManager.GetSettingValueAsync<int>(MyAppLevelSetting)).ShouldBe(54);
            (await settingManager.GetSettingValueAsync(MyAllLevelsSetting)).ShouldBe("应用程序级别修改值");
            //租户级别修改
            session.TenantId = 1;
            await settingManager.ChangeSettingForTenantAsync(1, MyAllLevelsSetting, "租户1修改值");
            (await settingManager.GetSettingValueAsync(MyAllLevelsSetting)).ShouldBe("租户1修改值");
            //用户级别修改
            session.UserId = 5;
            await settingManager.ChangeSettingForUserAsync(5, MyAllLevelsSetting, "用户5修改值");
            (await settingManager.GetSettingValueAsync(MyAllLevelsSetting)).ShouldBe("用户5修改值");
        }

        [Fact]
        public async Task Should_Delete_Setting_Values_On_Default_Value()
        {
            var session = new MyChangableSession();
            var store = new MemorySettingStore();

            var settingManager = CreateManager();
            settingManager.SettingStore = store;
            settingManager.AbpSession = session;

            session.TenantId = 1;
            session.UserId = 1;
            //我们能获取到用户的个人存储值
            (await store.GetSettingOrNullAsync(1, 1, MyAllLevelsSetting)).ShouldNotBe(null);
            (await settingManager.GetSettingValueAsync(MyAllLevelsSetting)).ShouldBe("user 1 stored value");
            //这将为用户删除设置，因为它跟租户的设置相同
            await settingManager.ChangeSettingForUserAsync(1, MyAllLevelsSetting, "tenant 1 stored value");
            (await store.GetSettingOrNullAsync(1, 1, MyAllLevelsSetting)).ShouldBe(null);
            //我们能获取租户的设置值
            (await store.GetSettingOrNullAsync(1, null, MyAllLevelsSetting)).ShouldNotBe(null);
            (await settingManager.GetSettingValueAsync(MyAllLevelsSetting)).ShouldBe("tenant 1 stored value");
            //这将为租户删除设置，因为它跟应用程序级别设置相同
            await settingManager.ChangeSettingForTenantAsync(1, MyAllLevelsSetting, "Application level stored value");
            (await store.GetSettingOrNullAsync(1, 1, MyAllLevelsSetting)).ShouldBe(null);
            //我们获取应用程序设置
            (await store.GetSettingOrNullAsync(null, null, MyAllLevelsSetting)).ShouldNotBe(null);
            (await settingManager.GetSettingValueAsync(MyAllLevelsSetting)).ShouldBe("Application level stored value");
            //这将为应用程序删除设置，因为它跟默认设置相同
            await settingManager.ChangeSettingForApplicationAsync(MyAllLevelsSetting, "application level defalut value");
            (await store.GetSettingOrNullAsync(null, null, MyAllLevelsSetting)).ShouldBe(null);
            //没有设置值，就返回默认值
            (await settingManager.GetSettingValueAsync(MyAllLevelsSetting)).ShouldBe("application level defalut value");
        }

        /// <summary>
        /// 模拟一个定义管理器
        /// </summary>
        /// <returns></returns>
        private static ISettingDefinitionManager CreateMockSettingDefinitionManager()
        {
            var settings = new Dictionary<string, SettingDefinition>()
            {
                { MyAppLevelSetting,new SettingDefinition(MyAppLevelSetting,"42") },
                { MyAllLevelsSetting,new SettingDefinition(MyAllLevelsSetting,"application level defalut value",scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User) },
                { MyNotInheritedSetting,new SettingDefinition(MyNotInheritedSetting,"default-value",scopes: SettingScopes.Application | SettingScopes.Tenant,isInherited:false) }
            };

            var definitionManager = Substitute.For<ISettingDefinitionManager>();
            //实现方法
            definitionManager.GetSettingDefinition(Arg.Any<string>()).Returns(x => settings[x[0].ToString()]);
            definitionManager.GetAllSettingDefinitions().Returns(settings.Values.ToList());
            return definitionManager;
        }

        /// <summary>
        /// 内存存储设置(使用内存作为数据库)
        /// </summary>
        private class MemorySettingStore : ISettingStore
        {
            private readonly List<SettingInfo> _settings;
            public MemorySettingStore()
            {
                _settings = new List<SettingInfo>()
                {
                    new SettingInfo(null,null,MyAppLevelSetting,"48"),
                    new SettingInfo(null,null,MyAllLevelsSetting,"Application level stored value"),
                    new SettingInfo(1,null,MyAllLevelsSetting,"tenant 1 stored value"),
                    new SettingInfo(1, 1, MyAllLevelsSetting, "user 1 stored value"),
                    new SettingInfo(1, 2, MyAllLevelsSetting, "user 2 stored value"),
                    new SettingInfo(null, null, MyNotInheritedSetting, "application value"),
                };
            }

            public async Task CreateAsync(SettingInfo setting)
            {
                _settings.Add(setting);
            }

            public async Task DeleteAsync(SettingInfo setting)
            {
                _settings.RemoveAll(a => a.TenantId == setting.TenantId && a.UserId == setting.UserId && a.Name == setting.Name);
            }

            public Task<List<SettingInfo>> GetAllListAsync(int? tenantId, long? userId)
            {
                return Task.FromResult(_settings.Where(a => a.TenantId == tenantId && a.UserId == userId).ToList());
            }

            public Task<SettingInfo> GetSettingOrNullAsync(int? tenantId, long? userId, string name)
            {
                return Task.FromResult(_settings.FirstOrDefault(a => a.TenantId == tenantId && a.UserId == userId && a.Name == name));
            }

            public async Task UpdateAsync(SettingInfo setting)
            {
                var a = await GetSettingOrNullAsync(setting.TenantId, setting.UserId, setting.Name);
                if (a != null)
                {
                    a.Value = setting.Value;
                }
            }
        }
    }
}
