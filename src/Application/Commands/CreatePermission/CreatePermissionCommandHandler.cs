using Application.Interface;
using Domain.Entities;
using Domain.Errors;
using Domain.Interfaces;
using ErrorOr;
using MediatR;
using Newtonsoft.Json;

namespace Application.Commands.CreatePermission
{
    public sealed class CreatePermissionCommandHandler : IRequestHandler<CreatePermissionCommand, ErrorOr<Unit>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IElasticsearchRepository elasticsearchRepository;
        private readonly IKafkaProducer kafkaProducer;

        public CreatePermissionCommandHandler(IUnitOfWork unitOfWork, IElasticsearchRepository elasticsearchRepository, IKafkaProducer kafkaProducer)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.elasticsearchRepository = elasticsearchRepository ?? throw new ArgumentNullException(nameof(elasticsearchRepository));
            this.kafkaProducer = kafkaProducer ?? throw new ArgumentNullException(nameof(kafkaProducer));
        }

        public async Task<ErrorOr<Unit>> Handle(CreatePermissionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await kafkaProducer.ProduceMessage("permission-topic", "request permission", JsonConvert.SerializeObject(request));
                
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

                await elasticsearchRepository.Index(permission);

                return Unit.Value;
            }
            catch (Exception ex)
            {
                return Error.Failure("CreatePermissionType.Failure", ex.Message);
            }
        }
    }
}
