using MediatR.Usage.Api.Data.Models;
using MediatR.Usage.Api.Data.Teacher.Notifications;
using MediatR.Usage.Api.Data.Teacher.Requests.Commands;
using MediatR.Usage.Api.Data.Teacher.Requests.Queries;
using Microsoft.AspNetCore.Mvc;

namespace MediatR.Usage.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class TeacherController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TeacherController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> GetTeachers()
        {
            var teachers = await _mediator.Send(new GetTeachersQuery());
            return Ok(teachers);
        }

        [HttpPost]
        public async Task<ActionResult> SaveTeacher(TeacherEntity teacherModel)
        {
            var teacherResult = await _mediator.Send(new SaveTeacherCommand(teacherModel));
            await _mediator.Publish(new TeacherAddedNotification(teacherResult));
            return StatusCode(201);
        }
    }
}
