namespace Abp.Web.Security.AntiForgery
{
    /// <summary>
    /// This interface is internally used by ABP framework and normally should not be used by applications.If it's needed, use
    /// <see cref="IAbpAntiForgeryManager"/> and cast to 
    /// <see cref="IAbpAntiForgeryValidator"/> to use 
    /// <see cref="IsValid"/> method.
    /// 此接口一般被ABP框架使用，而一般不被应用程序使用，如果需要使用，用<see cref="IsValid"/>方法将
    /// <see cref="IAbpAntiForgeryManager"/>转换成<see cref="IAbpAntiForgeryValidator"/>
    /// </summary>
    public interface IAbpAntiForgeryValidator
    {
        /// <summary>
        /// 是否有效
        /// </summary>
        /// <returns></returns>
        bool IsValid(string cookieValue, string tokenValue);
    }
}