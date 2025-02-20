using Application.Commands.CreatePermission;
using Application.Commands.UpdatePermission;
using Application.Querys.GetAllPermission;
using Application.Querys.GetPermissionById;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : ApiController
    {
        private readonly ISender mediator;

        public PermissionController(ISender mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            Log.Information("Getting all permissions");

            var result = await mediator.Send(new GetAllPermissionQuery());

            return result.Match(
                p => Ok(p),
                errors => Problem(errors)
            );
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetById(long id)
        {
            Log.Information("Buscar permiso");

            var result = await mediator.Send(new GetByIdPermissionQuery(id));

            return result.Match(
                p => Ok(p),
                errors => Problem(errors)
            );
        }

        [HttpPost()]
        public async Task<IActionResult> Create(CreatePermissionCommand command)
        {
            Log.Information("Requesting permission for {NameEmployee} {LastNameEmployee}", command.NameEmployee, command.LastNameEmployee);

            var result = await mediator.Send(command);

            return result.Match(
                p => Ok(),
                errors => Problem(errors)
            );
        }

        [HttpPut()]
        public async Task<IActionResult> Update(UpdatePermissionCommand command)
        {
            Log.Information("Modifying permission with Id: {Id}", command.Id);

            var result = await mediator.Send(command);

            return result.Match(
                p => Ok(),
                errors => Problem(errors)
            );
        }

    }
}
