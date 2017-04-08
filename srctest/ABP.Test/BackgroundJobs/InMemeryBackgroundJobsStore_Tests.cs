using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Shouldly;
using Abp.BackgroundJobs;

namespace ABP.Test.BackgroundJobs
{
    public class InMemeryBackgroundJobsStore_Tests
    {
        private readonly InMemoryBackgroundJobStore _store;
        public InMemeryBackgroundJobsStore_Tests()
        {
            _store = new InMemoryBackgroundJobStore();
        }

        [Fact]
        public async Task Test_All()
        {
            var jobInfo = new BackgroundJobInfo()
            {
                JobType = "TestType",
                JobArgs = "{}"
            };

            await _store.InsertAsync(jobInfo);
            (await _store.GetWaitingJobsAsync(1000)).Count.ShouldBe(1);

            await _store.DeleteAsync(jobInfo);
            (await _store.GetWaitingJobsAsync(1000)).Count.ShouldBe(0);
        }
    }
}
