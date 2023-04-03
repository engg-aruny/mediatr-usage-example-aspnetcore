### What is MediatR
MediatR is a library that implements the mediator pattern in .NET applications. It provides an easy way to implement a mediator and use it to handle application logic, improving code organization and separation of concerns. In this blog post, we will explore how to use MediatR in an ASP.NET Core application.

### CQRS and the Mediator Pattern
CQRS (Command Query Responsibility Segregation) and Mediator pattern are two design pattern that is commonly used together in software development.

#### CQRS
CQRS (Command Query Responsibility Segregation) is a design principle that describes that the method should be categorized into two categories: **Commands** and **Queries**. Let's understand by a simple example. 

![CQRS With MediatR](https://www.dropbox.com/s/g2wxmadnl6fdc5m/cover-image-mediater.jpg?raw=1 "CQRS With MediatR")

_Consider a banking application that has a transfer method. The transfer method could be implemented as a command, which means that it would modify the state of the system by deducting the transferred amount from one account and adding it to another account. On the other hand, a query method could be used to retrieve the balance of an account without changing the state of the system._

Overall, the Command Query Segregation Principle is a useful design principle that can help improve the maintainability and reliability of software systems.

#### Mediator Pattern
Mediator Pattern is a design pattern that enables objects to not communicate directly with each other but instead send messages to the mediator, who then forwards the message to appropriate objects. This helps to implement flexible and scalable systems, as a new object can be added or removed without affecting the existing objects.

> **Note:** MediatR is an **"in-process"** implementation. All communication between the user interface and the data store happens via MediatR. This is **best fit** when interacting within the same process and **it's not a suitable approach** when wanting to interact across systems. You must use a message broker such as **Kafka or Azure Service Bus**. This allows us to decouple the systems and send messages between them.

![Mediator Pattern with mediatR](https://www.dropbox.com/s/xjoekkpu6z7whfo/mediateR-commiuncation.jpg?raw=1 "Mediator Pattern with mediatR")

_One real-life example of the mediator pattern is air traffic control. In air traffic control, there are multiple aircrafts in the sky, each with its own flight plan and destination. However, to ensure safe and efficient operations, each aircraft needs to communicate with other aircraft and air traffic control towers in the area._

> The [source code](https://github.com/engg-aruny/mediatr-usage-example-aspnetcore/archive/refs/heads/master.zip) for this article can be found on Github. This sample application contains examples of "School", you might need to create an empty "SchoolDb" database and also might require to update the connection string in appsetting.json. This code also uses the "Code First" approach so you will be able to initial data automatically.

### Consume MediatR in ASP.Net Core API

**Install Dependencies**

Go to Manage NuGet Packages and browse the following packages

- MediatR
- MediatR.Extensions.Microsoft.DependencyInjection

![Manage NuGet Packages](https://www.dropbox.com/s/0hykophln7153if/mediatr-nuget-package.jpg?raw=1 "Manage NuGet Packages")

Setup the program (You can download [source code](https://github.com/engg-aruny/mediatr-usage-example-aspnetcore/archive/refs/heads/master.zip)) and register the MediatR in program.cs/startup.cs

```csharp
builder.Services.AddMediatR(typeof(Program));
``` 

#### Implement CQRS with MediatR
Let's separate out **Query** and **Command** now using MediatR. Here is a sample example where **GetTeachersQuery** is responsible for retrieving the **Teachers** from the database by implementing **IRequest**

**First, we need to define the Request  by implementing IRequest**

**GetTeachersQuery** - You can find the class in sample code "\Data\Teacher\Requests\Queries"

```csharp
using MediatR.Usage.Api.Data.Models;

namespace MediatR.Usage.Api.Data.Teacher.Requests.Queries
{
    public record GetTeachersQuery : IRequest<IEnumerable<TeacherEntity>>;
}

```

**SaveTeacherCommand** - You can find the class in sample code "\Data\Teacher\Requests\Commands"

```csharp
using MediatR.Usage.Api.Data.Models;

namespace MediatR.Usage.Api.Data.Teacher.Requests.Commands
{
    public record SaveTeacherCommand(TeacherEntity teacherModel) : IRequest<TeacherEntity>;
}

```

> **Notice:** We are using **Record** vs **class** here. The difference between **class** and **record** types in C# is that a record has the main purpose of storing data, while a class defines responsibility. **Records** are immutable, while classes are not.

**Second, we need to define the RequestHandler by implementing IRequestHandler**

**GetTeachersQueryHandler**  You can find the class in sample code "\Data\Teacher\Handlers\Queries"

```csharp

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
```

**SaveTeacherCommandHandler**  You can find the class in sample code "\Data\Teacher\Handlers\Command"

```csharp
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

```

#### Implement communication between components with MediatR

You need to define your controller and inject dependency of **IMediator**

**GetTeachers** - Endpoint defined in TeacherController

```csharp
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
    }
}
```
**SaveTeacher** - Endpoint defined in TeacherController

```csharp
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

        [HttpPost]
        public async Task<ActionResult> SaveTeacher(TeacherEntity teacherModel)
        {
            var teacherResult = await _mediator.Send(new SaveTeacherCommand(teacherModel));
            return StatusCode(201);
        }
    }
}

```

> **_mediator.Send(....)** is a method on the Mediator object that is used to send a command or query to its corresponding handler. In this case, a new instance of the **SaveTeacherCommand**/**GetTeachersQuery** is being sent to the Mediator for handling.

##### Run application through swagger
![Swagger Output](https://www.dropbox.com/s/30r150vbgtywnm9/mediatr-swaggerout.jpg?raw=1 "Swagger Output")


### MediatR Notification  - For Multiple Handlers
We have observed that a single handler is capable of handling only one request. But what if we need multiple handlers to process a single request?

For Example:
- Sending a welcome onboard email to the teacher
- Add teacher to Identity Server

To demonstrate this we have created **TeacherAddedNotification** by implementing **INotification** from MediatR 

```csharp
using MediatR.Usage.Api.Data.Models;

namespace MediatR.Usage.Api.Data.Teacher.Notifications
{
    public record TeacherAddedNotification(TeacherEntity teacher) : INotification;
}

```

Create two separate handlers by implementing **INotificationHandler** to handle the following scope.

**EmailSenderHandler** - Sending a welcome onboard email to the teacher

```csharp
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

```

**AddTeacherAccountToIdentityServerHandler** Add teacher to Identity Server

```chsarp
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

```

**Publish the message** - slight update to existing controller method, after saving now we are publishing message to handlers

```csharp
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

        [HttpPost]
        public async Task<ActionResult> SaveTeacher(TeacherEntity teacherModel)
        {
            var teacherResult = await _mediator.Send(new SaveTeacherCommand(teacherModel));
            await _mediator.Publish(new TeacherAddedNotification(teacherResult));
            return StatusCode(201);
        }
    }
}
```
> **_mediator.Publish** is a method used in the Mediator pattern to publish an event or notification to one or more interested objects, called subscribers or observers.


### Here is a output
![Notification Output](https://www.dropbox.com/s/wys0xiy49obigwy/mediatr-notificationhandler.jpg?raw=1 "Notification Output")
