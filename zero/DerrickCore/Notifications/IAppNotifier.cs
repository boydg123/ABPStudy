using System.Threading.Tasks;
using Abp;
using Abp.Notifications;
using Derrick.Authorization.Users;
using Derrick.MultiTenancy;

namespace Derrick.Notifications
{
    public interface IAppNotifier
    {
        Task WelcomeToTheApplicationAsync(User user);

        Task NewUserRegisteredAsync(User user);

        Task NewTenantRegisteredAsync(Tenant tenant);

        Task SendMessageAsync(UserIdentifier user, string message, NotificationSeverity severity = NotificationSeverity.Info);
    }
}
