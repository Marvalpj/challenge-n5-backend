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
            Log.Information("Buscar Permisos");

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
        public async Task<IActionResult> Create(CreatePermissionCommand createPermission)
        {
            Log.Information("Crear Permiso");

            var result = await mediator.Send(createPermission);

            return result.Match(
                p => Ok(),
                errors => Problem(errors)
            );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(UpdatePermissionCommand updatePermission, long id)
        {
            Log.Information("Actualizar Permisos");

            updatePermission.Id = id;
            var result = await mediator.Send(updatePermission);

            return result.Match(
                p => Ok(),
                errors => Problem(errors)
            );
        }

    }
}
