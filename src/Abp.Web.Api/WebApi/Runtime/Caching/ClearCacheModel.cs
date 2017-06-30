namespace Abp.WebApi.Runtime.Caching
{
    /// <summary>
    /// 清除缓存模型
    /// </summary>
    public class ClearCacheModel
    {
        public string Password { get; set; }

        public string[] Caches { get; set; }
    }
}