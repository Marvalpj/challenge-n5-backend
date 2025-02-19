using Domain.Entities;
using Domain.Errors;
using Domain.Interfaces;
using ErrorOr;
using MediatR;

namespace Application.Commands.CreatePermission
{
    public sealed class CreatePermissionCommandHandler : IRequestHandler<CreatePermissionCommand, ErrorOr<Unit>>
    {
        private readonly IUnitOfWork unitOfWork;

        public CreatePermissionCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<ErrorOr<Unit>> Handle(CreatePermissionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrEmpty(request.NameEmployee))
                    return DomainError.Permission.PermissionNameIsEmpty;


                if (string.IsNullOrEmpty(request.LastNameEmployee))
                    return DomainError.Permission.PermissionLastNameIsEmpty;

                if (await unitOfWork.Repository<PermissionType>().GetByIdAsync(request.PermissionTypeId) is not PermissionType permissionType)
                    return DomainError.PermissionType.PermissionTypeIdDoesNotExist;

                Permission permission = new Permission(
                    request.NameEmployee,
                    request.LastNameEmployee,
                    request.PermissionTypeId,
                    request.Date
                );

                await unitOfWork.Repository<Permission>().AddAsync(permission);
                await unitOfWork.SaveChangesAsync(cancellationToken);
                
                //var settings = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
                //await kafkaProducer.ProduceMessage("permission-topic", "request permission", JsonConvert.SerializeObject(permission, settings));

                return Unit.Value;
            }
            catch (Exception ex)
            {
                return Error.Failure("CreatePermissionType.Failure", ex.Message);
            }
        }
    }
}
