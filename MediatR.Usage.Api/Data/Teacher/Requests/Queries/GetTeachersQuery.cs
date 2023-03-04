using MediatR.Usage.Api.Data.Models;

namespace MediatR.Usage.Api.Data.Teacher.Requests.Queries
{
    public record GetTeachersQuery : IRequest<IEnumerable<TeacherEntity>>;
}
