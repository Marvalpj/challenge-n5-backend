using Application.Interface;
using Application.Querys.Dtos;
using Domain.Entities;
using Domain.Interfaces;
using ErrorOr;
using MediatR;

namespace Application.Querys.GetAllPermission
{
    public class GetAllPermissionQueryHandler : IRequestHandler<GetAllPermissionQuery, ErrorOr<IEnumerable<PermissionDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IElasticsearchRepository elasticsearchRepository;
        private readonly IKafkaProducer kafkaProducer;

        public GetAllPermissionQueryHandler(IUnitOfWork unitOfWork, IElasticsearchRepository elasticsearchRepository, IKafkaProducer kafkaProducer)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.elasticsearchRepository = elasticsearchRepository ?? throw new ArgumentNullException(nameof(elasticsearchRepository));
            this.kafkaProducer = kafkaProducer ?? throw new ArgumentNullException(nameof(kafkaProducer));
        }
        public async Task<ErrorOr<IEnumerable<PermissionDto>>> Handle(GetAllPermissionQuery request, CancellationToken cancellationToken)
        {
            try
            {
                await kafkaProducer.ProduceMessage("permission-topic", "get - permissions");

                IEnumerable<Permission> pElastic = await elasticsearchRepository.GetallAsync();
                if (pElastic is not null)
                    return pElastic.Select(p => new PermissionDto(
                        p.Id,
                        p.NameEmployee,
                        p.LastNameEmployee,
                        p.PermissionTypeId,
                        p.Date
                    )).ToList();

                IEnumerable<Permission> permission = await unitOfWork.Repository<Permission>().GetAllAsync();

                return permission.Select(p => new PermissionDto(
                    p.Id,
                    p.NameEmployee,
                    p.LastNameEmployee,
                    p.PermissionTypeId,
                    p.Date
                )).ToList();
            }
            catch (Exception ex)
            {
                return Error.Failure("CreatePermissionType.Failure", ex.Message);
            }
        }
    }
}
