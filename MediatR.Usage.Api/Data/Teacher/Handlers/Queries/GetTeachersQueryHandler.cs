using MediatR.Usage.Api.Data.Models;
using MediatR.Usage.Api.Data.Teacher.Requests.Queries;
using Microsoft.EntityFrameworkCore;

namespace MediatR.Usage.Api.Data.Teacher.Handlers.Queries
{
    public class GetTeachersQueryHandler : IRequestHandler<GetTeachersQuery, IEnumerable<TeacherEntity>>
    {
        private readonly SchoolDbContext _context;
        public GetTeachersQueryHandler(SchoolDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TeacherEntity>> Handle(GetTeachersQuery request, CancellationToken cancellationToken)
        {
            return await _context.Teachers.ToArrayAsync(cancellationToken);
        }
    }
}
