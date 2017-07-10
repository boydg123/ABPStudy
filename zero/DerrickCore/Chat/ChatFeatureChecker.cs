using Abp.Application.Features;
using Abp.UI;
using Derrick.Features;

namespace Derrick.Chat
{
    /// <summary>
    /// 聊天功能检查器
    /// </summary>
    public class ChatFeatureChecker : AbpZeroTemplateDomainServiceBase, IChatFeatureChecker
    {
        /// <summary>
        /// 功能检查器引用
        /// </summary>
        private readonly IFeatureChecker _featureChecker;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="featureChecker">功能检查器</param>
        public ChatFeatureChecker(
            IFeatureChecker featureChecker
        )
        {
            _featureChecker = featureChecker;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sourceTenantId">源商户ID</param>
        /// <param name="targetTenantId">目标商户ID</param>
        public void CheckChatFeatures(int? sourceTenantId, int? targetTenantId)
        {
            CheckChatFeaturesInternal(sourceTenantId, targetTenantId, ChatSide.Sender);
            CheckChatFeaturesInternal(targetTenantId, sourceTenantId, ChatSide.Receiver);
        }
        /// <summary>
        /// 检查内部聊天功能
        /// </summary>
        /// <param name="sourceTenantId">源商户ID</param>
        /// <param name="targetTenantId">目标商户ID</param>
        /// <param name="side">聊天边</param>
        private void CheckChatFeaturesInternal(int? sourceTenantId, int? targetTenantId, ChatSide side)
        {
            var localizationPosfix = side == ChatSide.Sender ? "ForSender" : "ForReceiver";
            if (sourceTenantId.HasValue)
            {
                if (!_featureChecker.IsEnabled(sourceTenantId.Value, AppFeatures.ChatFeature))
                {
                    throw new UserFriendlyException(L("ChatFeatureIsNotEnabled" + localizationPosfix));
                }

                if (targetTenantId.HasValue)
                {
                    if (sourceTenantId == targetTenantId)
                    {
                        return;
                    }

                    if (!_featureChecker.IsEnabled(sourceTenantId.Value, AppFeatures.TenantToTenantChatFeature))
                    {
                        throw new UserFriendlyException(L("TenantToTenantChatFeatureIsNotEnabled" + localizationPosfix));
                    }
                }
                else
                {
                    if (!_featureChecker.IsEnabled(sourceTenantId.Value, AppFeatures.TenantToHostChatFeature))
                    {
                        throw new UserFriendlyException(L("TenantToHostChatFeatureIsNotEnabled" + localizationPosfix));
                    }
                }
            }
            else
            {
                if (targetTenantId.HasValue)
                {
                    if (!_featureChecker.IsEnabled(targetTenantId.Value, AppFeatures.TenantToHostChatFeature))
                    {
                        throw new UserFriendlyException(L("TenantToHostChatFeatureIsNotEnabled" + (side == ChatSide.Sender ? "ForReceiver" : "ForSender")));
                    }
                }
            }
        }
    }
}