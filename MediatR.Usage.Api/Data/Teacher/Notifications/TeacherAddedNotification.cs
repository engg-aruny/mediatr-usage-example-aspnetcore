using MediatR.Usage.Api.Data.Models;

namespace MediatR.Usage.Api.Data.Teacher.Notifications
{
    public record TeacherAddedNotification(TeacherEntity teacher) : INotification;
}
