using System;
using System.Threading.Tasks;
using Abp;
using Abp.Localization;
using Abp.Notifications;
using ICMSDemo.Authorization.Users;
using ICMSDemo.MultiTenancy;

namespace ICMSDemo.Notifications
{
    public class AppNotifier : ICMSDemoDomainServiceBase, IAppNotifier
    {
        private readonly INotificationPublisher _notificationPublisher;

        public AppNotifier(INotificationPublisher notificationPublisher)
        {
            _notificationPublisher = notificationPublisher;
        }

        public async Task WelcomeToTheApplicationTestAsync(User user)
        {
            await _notificationPublisher.PublishAsync(
                AppNotificationNames.WelcomeToTheApplication,
                new MessageNotificationData("A posssible loss event has been identified!"),
                severity: NotificationSeverity.Success,
                userIds: new[] { user.ToUserIdentifier() }
                );
        }

        public async Task WelcomeToTheApplicationAsync(User user)
        {
            await _notificationPublisher.PublishAsync(
                AppNotificationNames.WelcomeToTheApplication,
                new MessageNotificationData(L("WelcomeToTheApplicationNotificationMessage")),
                severity: NotificationSeverity.Success,
                userIds: new[] { user.ToUserIdentifier() }
                );
        }

        public async Task NewUserRegisteredAsync(User user)
        {
            var notificationData = new LocalizableMessageNotificationData(
                new LocalizableString(
                    "NewUserRegisteredNotificationMessage",
                    ICMSDemoConsts.LocalizationSourceName
                    )
                );

            notificationData["userName"] = user.UserName;
            notificationData["emailAddress"] = user.EmailAddress;

            await _notificationPublisher.PublishAsync(AppNotificationNames.NewUserRegistered, notificationData, tenantIds: new[] { user.TenantId });
        }

        public async Task NewTenantRegisteredAsync(Tenant tenant)
        {
            var notificationData = new LocalizableMessageNotificationData(
                new LocalizableString(
                    "NewTenantRegisteredNotificationMessage",
                    ICMSDemoConsts.LocalizationSourceName
                    )
                );

            notificationData["tenancyName"] = tenant.TenancyName;
            await _notificationPublisher.PublishAsync(AppNotificationNames.NewTenantRegistered, notificationData);
        }

        public async Task GdprDataPrepared(UserIdentifier user, Guid binaryObjectId)
        {
            var notificationData = new LocalizableMessageNotificationData(
                new LocalizableString(
                    "GdprDataPreparedNotificationMessage",
                    ICMSDemoConsts.LocalizationSourceName
                )
            );

            notificationData["binaryObjectId"] = binaryObjectId;

            await _notificationPublisher.PublishAsync(AppNotificationNames.GdprDataPrepared, notificationData, userIds: new[] { user });
        }

        //This is for test purposes
        public async Task SendMessageAsync(UserIdentifier user, string message, NotificationSeverity severity = NotificationSeverity.Info)
        {
            await _notificationPublisher.PublishAsync(
                "App.SimpleMessage",
                new MessageNotificationData(message),
                severity: severity,
                userIds: new[] { user }
                );
        }

        public async Task TenantsMovedToEdition(UserIdentifier user, string sourceEditionName, string targetEditionName)
        {
            var notificationData = new LocalizableMessageNotificationData(
                new LocalizableString(
                    "TenantsMovedToEditionNotificationMessage",
                    ICMSDemoConsts.LocalizationSourceName
                )
            );

            notificationData["sourceEditionName"] = sourceEditionName;
            notificationData["targetEditionName"] = targetEditionName;

            await _notificationPublisher.PublishAsync(AppNotificationNames.TenantsMovedToEdition, notificationData, userIds: new[] { user });
        }

        public Task<TResult> TenantsMovedToEdition<TResult>(UserIdentifier argsUser, int sourceEditionId, int targetEditionId)
        {
            throw new NotImplementedException();
        }

        public async Task SomeUsersCouldntBeImported(UserIdentifier argsUser, string fileToken, string fileType, string fileName)
        {
            var notificationData = new LocalizableMessageNotificationData(
                new LocalizableString(
                    "ClickToSeeInvalidUsers",
                    ICMSDemoConsts.LocalizationSourceName
                )
            );

            notificationData["fileToken"] = fileToken;
            notificationData["fileType"] = fileType;
            notificationData["fileName"] = fileName;

            await _notificationPublisher.PublishAsync(AppNotificationNames.DownloadInvalidImportUsers, notificationData, userIds: new[] { argsUser });
        }


        public async Task NotifyControlManager(UserIdentifier argsUser)
        {
            var notificationData = new LocalizableMessageNotificationData(
                new LocalizableString(
                    "NewWorkPaperCompleted",
                    ICMSDemoConsts.LocalizationSourceName
                )
            );


            await _notificationPublisher.PublishAsync(AppNotificationNames.NewWorkingPaperCreated, notificationData, userIds: new[] { argsUser });
        }       
        
        public async Task NotifyControlOfficerOfApproval(UserIdentifier argsUser)
        {
            var notificationData = new LocalizableMessageNotificationData(
                new LocalizableString(
                    "WorkingPaperApproved",
                    ICMSDemoConsts.LocalizationSourceName
                )
            );


            await _notificationPublisher.PublishAsync(AppNotificationNames.WorkingPaperApproved, notificationData, userIds: new[] { argsUser });
        }
    }
}