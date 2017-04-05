namespace Abp
{
    /// <summary>
    /// 错误Code
    /// </summary>
    public interface IHasErrorCode
    {
        int Code { get; set; }
    }
}