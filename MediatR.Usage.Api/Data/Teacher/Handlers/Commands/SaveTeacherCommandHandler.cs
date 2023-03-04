using MediatR.Usage.Api.Data.Models;
using MediatR.Usage.Api.Data.Teacher.Requests.Commands;

namespace MediatR.Usage.Api.Data.Teacher.Handlers.Commands
{
    public class SaveTeacherCommandHandler : IRequestHandler<SaveTeacherCommand, TeacherEntity>
    {
        private readonly SchoolDbContext _context;

        public SaveTeacherCommandHandler(SchoolDbContext context)
        {
            _context = context;
        }

        public async Task<TeacherEntity> Handle(SaveTeacherCommand request, CancellationToken cancellationToken)
        {
            _context.Teachers.Add(request.teacherModel);
            _context.SaveChanges();

            return request.teacherModel;
        }
    }
}
