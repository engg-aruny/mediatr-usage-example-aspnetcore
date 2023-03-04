namespace MediatR.Usage.Api.Data.Teacher.Notifications
{
    public class AddTeacherAccountToIdentityServerHandler : INotificationHandler<TeacherAddedNotification>
    {
        private readonly ILogger _logger;
        public AddTeacherAccountToIdentityServerHandler(ILogger<AddTeacherAccountToIdentityServerHandler> logger)
        {
            _logger = logger;
        }

        public async Task Handle(TeacherAddedNotification notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Teacher {notification.teacher.FirstName} has been added to identity server");
            await Task.CompletedTask;
        }
    }
}
