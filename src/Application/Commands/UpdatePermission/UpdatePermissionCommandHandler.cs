using Domain.Entities;
using Domain.Errors;
using Domain.Interfaces;
using ErrorOr;
using MediatR;

namespace Application.Commands.UpdatePermission
{
    public sealed class UpdatePermissionCommandHandler : IRequestHandler<UpdatePermissionCommand, ErrorOr<Unit>>
    {
        private readonly IUnitOfWork unitOfWork;

        public UpdatePermissionCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<ErrorOr<Unit>> Handle(UpdatePermissionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (await unitOfWork.Repository<Permission>().GetByIdAsync(request.Id) is not Permission permission)
                    return DomainError.Permission.PermissionIdDoesNotExist;

                if (request.PermissionTypeId != null)
                    if (await unitOfWork.Repository<PermissionType>().GetByIdAsync(request.PermissionTypeId.Value) is not PermissionType permissionType)
                        return DomainError.PermissionType.PermissionTypeIdDoesNotExist;

                permission.UpdatePermission(request.NameEmployee, request.LastNameEmployee, request.PermissionTypeId, request.Date);

                unitOfWork.Repository<Permission>().Update(permission);
                await unitOfWork.SaveChangesAsync(cancellationToken);

                // hacer la parte de elastic 
                // await kafkaProducer.ProduceMessage("permission-topic", "modify - permission", JsonConvert.SerializeObject(permission));

                return Unit.Value;
            }
            catch (Exception ex)
            {
                return Error.Failure("CreatePermissionType.Failure", ex.Message);
            }
        }
    }
}
