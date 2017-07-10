namespace Derrick.Chat
{
    /// <summary>
    /// 聊天功能检查器
    /// </summary>
    public interface IChatFeatureChecker
    {
        /// <summary>
        /// 检查聊天功能
        /// </summary>
        /// <param name="sourceTenantId">源商户ID</param>
        /// <param name="targetTenantId">目标商户ID</param>
        void CheckChatFeatures(int? sourceTenantId, int? targetTenantId);
    }
}
