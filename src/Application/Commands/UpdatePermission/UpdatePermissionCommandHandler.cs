using Application.Interface;
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
        private readonly IElasticsearchRepository elasticsearchRepository;

        public UpdatePermissionCommandHandler(IUnitOfWork unitOfWork, IElasticsearchRepository elasticsearchRepository)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.elasticsearchRepository = elasticsearchRepository ?? throw new ArgumentNullException(nameof(elasticsearchRepository));
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

                await elasticsearchRepository.UpdateIndex(permission);
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
