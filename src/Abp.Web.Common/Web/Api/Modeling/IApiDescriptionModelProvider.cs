namespace Abp.Web.Api.Modeling
{
    /// <summary>
    /// API描述模型提供者
    /// </summary>
    public interface IApiDescriptionModelProvider
    {
        /// <summary>
        /// 应用程序API描述模型
        /// </summary>
        /// <returns></returns>
        ApplicationApiDescriptionModel CreateModel();
    }
}