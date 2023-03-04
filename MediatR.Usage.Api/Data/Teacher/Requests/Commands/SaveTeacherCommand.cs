using MediatR.Usage.Api.Data.Models;

namespace MediatR.Usage.Api.Data.Teacher.Requests.Commands
{
    public record SaveTeacherCommand(TeacherEntity teacherModel) : IRequest<TeacherEntity>;
}
