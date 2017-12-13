using Abp.BackgroundJobs;
using Abp.Domain.Uow;
using Abp.Notifications;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abp.Test.Notifications
{
    [TestClass]
    public class NotificationPublisher_Tests : TestBaseWithLocalManager
    {
        private readonly NotificationPublisher _publisher;
        private readonly INotificationStore _store;
        private readonly IBackgroundJobManager _backgroundJobManager;

        public NotificationPublisher_Tests()
        {
            _store = Substitute.For<INotificationStore>();
            _backgroundJobManager = Substitute.For<IBackgroundJobManager>();
            _publisher = new NotificationPublisher(_store, _backgroundJobManager, Substitute.For<INotificationDistributer>());
            _publisher.UnitOfWorkManager = Substitute.For<IUnitOfWorkManager>();
            _publisher.UnitOfWorkManager.Current.Returns(Substitute.For<IActiveUnitOfWork>());
        }

        [TestMethod]
        public async Task Should_Publish_General_Notification()
        {
            var notificationData = CreateNotificationData();

            await _publisher.PublishAsync("TestNotification", notificationData, severity: NotificationSeverity.Success);
        }

        /// <summary>
        /// 创建通知数据
        /// </summary>
        /// <returns>通知数据对象</returns>
        private static NotificationData CreateNotificationData()
        {
            var notificationData = new NotificationData();
            notificationData["TestValue"] = 42;
            return notificationData;
        }
    }
}
