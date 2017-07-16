using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Organizations;
using Microsoft.AspNet.Identity;
using Derrick.Authorization.Roles;
using Derrick.Authorization.Users;
using Derrick.Chat;
using Derrick.Friendships;
using Derrick.Storage;

namespace Derrick.MultiTenancy.Demo
{
    /// <summary>
    /// Used to build demo data for new tenants.Creates sample organization units, users... etc.
    /// 用于为新商户生成Demo数据。创建简单的组织架构，用户等等
    /// It works only if in DEMO mode ("App.DemoMode" should be "true" in web.config). Otherwise, does nothing.
    /// 它将在Demo模式中使用。("App.DemoMode"在配置文件中应该配置为"true")否则，无效。
    /// </summary>
    public class TenantDemoDataBuilder : AbpZeroTemplateServiceBase, ITransientDependency
    {
        /// <summary>
        /// 判断是否是Demo模式
        /// </summary>
        public bool IsInDemoMode
        {
            get
            {
                return string.Equals(ConfigurationManager.AppSettings["App.DemoMode"], "true", StringComparison.InvariantCultureIgnoreCase);
            }
        }

        /// <summary>
        /// 组织架构管理器
        /// </summary>
        private readonly OrganizationUnitManager _organizationUnitManager;
        /// <summary>
        /// 用户管理器
        /// </summary>
        private readonly UserManager _userManager;
        /// <summary>
        /// 随机用户生成器
        /// </summary>
        private readonly RandomUserGenerator _randomUserGenerator;
        /// <summary>
        /// 二进制对象管理器
        /// </summary>
        private readonly IBinaryObjectManager _binaryObjectManager;
        /// <summary>
        /// APP文件夹描述
        /// </summary>
        private readonly IAppFolders _appFolders;
        /// <summary>
        /// 好友管理器
        /// </summary>
        private readonly IFriendshipManager _friendshipManager;
        /// <summary>
        /// 聊天消息仓储
        /// </summary>
        private readonly IRepository<ChatMessage, long> _chatMessageRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="organizationUnitManager">组织架构管理器</param>
        /// <param name="userManager">用户管理器</param>
        /// <param name="randomUserGenerator">随机用户生成器</param>
        /// <param name="binaryObjectManager">二进制对象管理器</param>
        /// <param name="appFolders">APP文件夹描述</param>
        /// <param name="friendshipManager">好友管理器</param>
        /// <param name="chatMessageManager">聊天消息管理器</param>
        /// <param name="chatMessageRepository">聊天消息仓储</param>
        public TenantDemoDataBuilder(
            OrganizationUnitManager organizationUnitManager,
            UserManager userManager,
            RandomUserGenerator randomUserGenerator,
            IBinaryObjectManager binaryObjectManager,
            IAppFolders appFolders,
            IFriendshipManager friendshipManager,
            IChatMessageManager chatMessageManager, 
            IRepository<ChatMessage, long> chatMessageRepository)
        {
            _organizationUnitManager = organizationUnitManager;
            _userManager = userManager;
            _randomUserGenerator = randomUserGenerator;
            _binaryObjectManager = binaryObjectManager;
            _appFolders = appFolders;
            _friendshipManager = friendshipManager;
            _chatMessageRepository = chatMessageRepository;
        }

        /// <summary>
        /// 生成 - 异步
        /// </summary>
        /// <param name="tenant">商户对象</param>
        /// <returns></returns>
        public async Task BuildForAsync(Tenant tenant)
        {
            if (!IsInDemoMode)
            {
                return;
            }

            using (CurrentUnitOfWork.SetTenantId(tenant.Id))
            {
                await BuildForInternalAsync(tenant);
                await CurrentUnitOfWork.SaveChangesAsync();
            }
        }

        /// <summary>
        /// 内部生成
        /// </summary>
        /// <param name="tenant">商户对象</param>
        /// <returns></returns>
        private async Task BuildForInternalAsync(Tenant tenant)
        {
            //Create Organization Units

            var organizationUnits = new List<OrganizationUnit>();

            var producing = await CreateAndSaveOrganizationUnit(organizationUnits, tenant, "Producing");

            var researchAndDevelopment = await CreateAndSaveOrganizationUnit(organizationUnits, tenant, "Research & Development", producing);

            var ivrProducts = await CreateAndSaveOrganizationUnit(organizationUnits, tenant, "IVR Related Products", researchAndDevelopment);
            var voiceTech = await CreateAndSaveOrganizationUnit(organizationUnits, tenant, "Voice Technologies", researchAndDevelopment);
            var inhouseProjects = await CreateAndSaveOrganizationUnit(organizationUnits, tenant, "Inhouse Projects", researchAndDevelopment);

            var qualityManagement = await CreateAndSaveOrganizationUnit(organizationUnits, tenant, "Quality Management", producing);
            var testing = await CreateAndSaveOrganizationUnit(organizationUnits, tenant, "Testing", producing);

            var selling = await CreateAndSaveOrganizationUnit(organizationUnits, tenant, "Selling");

            var marketing = await CreateAndSaveOrganizationUnit(organizationUnits, tenant, "Marketing", selling);
            var sales = await CreateAndSaveOrganizationUnit(organizationUnits, tenant, "Sales", selling);
            var custRelations = await CreateAndSaveOrganizationUnit(organizationUnits, tenant, "Customer Relations", selling);

            var supporting = await CreateAndSaveOrganizationUnit(organizationUnits, tenant, "Supporting");

            var buying = await CreateAndSaveOrganizationUnit(organizationUnits, tenant, "Buying", supporting);
            var humanResources = await CreateAndSaveOrganizationUnit(organizationUnits, tenant, "Human Resources", supporting);

            //Create users

            var users = _randomUserGenerator.GetRandomUsers(RandomHelper.GetRandom(12, 26), tenant.Id);
            foreach (var user in users)
            {
                //Create the user
                await _userManager.CreateAsync(user);
                await CurrentUnitOfWork.SaveChangesAsync();

                //Add to roles
                _userManager.AddToRole(user.Id, StaticRoleNames.Tenants.User);

                //Add to OUs
                var randomOus = RandomHelper.GenerateRandomizedList(organizationUnits).Take(RandomHelper.GetRandom(0, 3));
                foreach (var ou in randomOus)
                {
                    await _userManager.AddToOrganizationUnitAsync(user, ou);
                }

                //Set profile picture
                if (RandomHelper.GetRandom(100) < 70) //A user will have a profile picture in 70% probability.
                {
                    await SetRandomProfilePictureAsync(user);
                }
            }

            //Set a picture to admin!
            var admin = _userManager.FindByName(User.AdminUserName);
            await SetRandomProfilePictureAsync(admin);

            //Create Friendships
            var friends = RandomHelper.GenerateRandomizedList(users).Take(3).ToList();
            foreach (var friend in friends)
            {
                _friendshipManager.CreateFriendship(
                    new Friendship(
                        admin.ToUserIdentifier(),
                        friend.ToUserIdentifier(),
                        tenant.TenancyName,
                        friend.UserName,
                        friend.ProfilePictureId,
                        FriendshipState.Accepted)
                );

                _friendshipManager.CreateFriendship(
                    new Friendship(
                        friend.ToUserIdentifier(),
                        admin.ToUserIdentifier(),
                        tenant.TenancyName,
                        admin.UserName,
                        admin.ProfilePictureId,
                        FriendshipState.Accepted)
                );
            }

            //Create chat message
            var friendWithMessage = RandomHelper.GenerateRandomizedList(friends).First();
            _chatMessageRepository.InsertAndGetId(
                new ChatMessage(
                    friendWithMessage.ToUserIdentifier(),
                    admin.ToUserIdentifier(), 
                    ChatSide.Sender, 
                    L("Demo_SampleChatMessage"), 
                    ChatMessageReadState.Read
                )
            );

            _chatMessageRepository.InsertAndGetId(
                new ChatMessage(
                    admin.ToUserIdentifier(),
                    friendWithMessage.ToUserIdentifier(),
                    ChatSide.Receiver,
                    L("Demo_SampleChatMessage"),
                    ChatMessageReadState.Unread
                )
            );

        }

        /// <summary>
        /// 创建或保存组织架构
        /// </summary>
        /// <param name="organizationUnits">组织架构列表</param>
        /// <param name="tenant">商户</param>
        /// <param name="displayName">显示名称</param>
        /// <param name="parent">父组织，默认为Null</param>
        /// <returns></returns>
        private async Task<OrganizationUnit> CreateAndSaveOrganizationUnit(List<OrganizationUnit> organizationUnits, Tenant tenant, string displayName, OrganizationUnit parent = null)
        {
            var organizationUnit = new OrganizationUnit(tenant.Id, displayName, parent == null ? (long?)null : parent.Id);

            await _organizationUnitManager.CreateAsync(organizationUnit);
            await CurrentUnitOfWork.SaveChangesAsync();

            organizationUnits.Add(organizationUnit);

            return organizationUnit;
        }

        /// <summary>
        /// 设置随机图片 - 异步
        /// </summary>
        /// <param name="user">用户对象</param>
        /// <returns></returns>
        private async Task SetRandomProfilePictureAsync(User user)
        {
            try
            {
                //Save a random profile picture
                var storedFile = new BinaryObject(user.TenantId, GetRandomProfilePictureBytes());
                await _binaryObjectManager.SaveAsync(storedFile);

                //Update new picture on the user
                user.ProfilePictureId = storedFile.Id;
                await CurrentUnitOfWork.SaveChangesAsync();
            }
            catch
            {
                //we can ignore this exception                
            }
        }

        /// <summary>
        /// 获取随机图片字节
        /// </summary>
        /// <returns></returns>
        private byte[] GetRandomProfilePictureBytes()
        {
            var fileName = string.Format("sample-profile-{0}.jpg", (RandomHelper.GetRandom(1, 11)).ToString("00"));
            var fullPath = Path.Combine(_appFolders.SampleProfileImagesFolder, fileName);

            if (!File.Exists(fullPath))
            {
                throw new ApplicationException("Could not find sample profile picture on " + fullPath);
            }

            return File.ReadAllBytes(fullPath);
        }
    }
}
