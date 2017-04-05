namespace Abp.BackgroundJobs
{
    /// <summary>
    /// Defines interface of a background job.
    /// 定义后台作业的接口
    /// </summary>
    public interface IBackgroundJob<in TArgs>
    {
        /// <summary>
        /// Executes the job with the <see cref="args"/>.
        /// 执行作业用<see cref="args"/>参数
        /// </summary>
        /// <param name="args">Job arguments. / 作业参数</param>
        void Execute(TArgs args);
    }
}