namespace MediatR.Usage.Api.Data.Teacher.Notifications
{
    public class EmailSenderHandler : INotificationHandler<TeacherAddedNotification>
    {
        private readonly ILogger _logger;
        public EmailSenderHandler(ILogger<EmailSenderHandler> logger)
        {
            _logger = logger;
        }

        public async Task Handle(TeacherAddedNotification notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Welcome to school {notification.teacher.FirstName}, happy to onboard you");
            await Task.CompletedTask;
        }
    }
}
