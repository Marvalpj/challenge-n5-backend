using Application.Interface;
using Application.Querys.Dtos;
using Domain.Entities;
using Domain.Errors;
using Domain.Interfaces;
using ErrorOr;
using MediatR;

namespace Application.Querys.GetPermissionById
{
    public sealed class GetByIdPermissionQueryHandler : IRequestHandler<GetByIdPermissionQuery, ErrorOr<PermissionDto>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IElasticsearchRepository elasticsearchRepository;

        public GetByIdPermissionQueryHandler(IUnitOfWork unitOfWork, IElasticsearchRepository elasticsearchRepository)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.elasticsearchRepository = elasticsearchRepository ?? throw new ArgumentNullException(nameof(elasticsearchRepository));
        }
        public async Task<ErrorOr<PermissionDto>> Handle(GetByIdPermissionQuery request, CancellationToken cancellationToken)
        {
            try
            {
                //await kafkaProducer.ProduceMessage("permission-topic", "get - permission");

                Permission pElastic = await elasticsearchRepository.GetByIdAsync(request.Id.ToString());
                if(pElastic is not null)
                    return ReturnPermission(pElastic);

                if (await unitOfWork.Repository<Permission>().GetByIdAsync(request.Id) is not Permission p)
                    return DomainError.Permission.PermissionIdDoesNotExist;

                return ReturnPermission(p);
            }
            catch (Exception ex)
            {
                return Error.Failure("CreatePermissionType.Failure", ex.Message);
            }
        }

        private PermissionDto ReturnPermission(Permission permission) => new PermissionDto( permission.Id, permission.NameEmployee, permission.LastNameEmployee, permission.PermissionTypeId, permission.Date);
    }
}
